using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;
using System.Threading.Tasks;
using BrassKnuckles.EntityFramework.Systems;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Type of execution for a system.
    /// </summary>
    public enum SystemExecutionType
    {
        /// <summary>
        /// System renders images on-screen.
        /// </summary>
        Draw,
        /// <summary>
        /// System updates the state of entities or other game objects.
        /// </summary>
        Update
    }

    /// <summary>
    /// Manages the storage and execution of <see cref="IEntitySystem"/> instances.
    /// </summary>
    public sealed class SystemManager : IManager
    {
        #region Private fields

        private EntityWorld _world;
        private Dictionary<Type, IEntitySystem> _systems;
        private Dictionary<int, List<IEntitySystem>> _drawLayers;
        private Dictionary<int, List<IEntitySystem>> _updateLayers;
        private List<IEntitySystem> _mergedBag;
        private TaskFactory _taskFactory;
        private List<Task> _tasks;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new SystemManager instance using the provided <see cref="EntityWorld"/>.
        /// </summary>
        /// <param name="world"><see cref="EntityWorld"/> instance to associate with.</param>
        public SystemManager(EntityWorld world)
        {
            _world = world;
            _systems = new Dictionary<Type, IEntitySystem>();
            _drawLayers = new Dictionary<int, List<IEntitySystem>>();
            _updateLayers = new Dictionary<int, List<IEntitySystem>>();
            _mergedBag = new List<IEntitySystem>();
            _taskFactory = new TaskFactory(TaskScheduler.Default);
            _tasks = new List<Task>();
        }

        #endregion

        #region Private methods

        private void UpdateBagSync(List<IEntitySystem> bag)
        {
            foreach (IEntitySystem system in bag)
            {
                system.Process();
            }
        }

        private void UpdateBagAsync(List<IEntitySystem> bag)
        {
            Parallel.ForEach<IEntitySystem>(bag,
                (system) =>
                {
                    system.Process();
                });
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an <see cref="IEntitySystem"/> to the manager with the specified <see cref="SystemExecutionType"/> at the default priority.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEntitySystem"/> being added.</typeparam>
        /// <param name="system"><see cref="IEntitySystem"/> instance to add.</param>
        /// <param name="executionType">Type of execution.</param>
        /// <returns>The <see cref="IEntitySystem"/> instance passed, with the system bit flag set.</returns>
        public T SetSystem<T>(T system, SystemExecutionType executionType) where T : IEntitySystem
        {
            return SetSystem<T>(system, executionType, 0);
        }

        /// <summary>
        /// Adds an <see cref="IEntitySystem"/> to the manager with the specified <see cref="SystemExecutionType"/>and priority.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IEntitySystem"/> being added.</typeparam>
        /// <param name="system"><see cref="IEntitySystem"/> instance to add.</param>
        /// <param name="executionType">Type of execution.</param>
        /// <param name="priority">Execution priority. Systems with lower priority values are executed first.</param>
        /// <returns>The <see cref="IEntitySystem"/> instance passed, with the system bit flag set.</returns>
        public T SetSystem<T>(T system, SystemExecutionType executionType, int priority) where T : IEntitySystem
        {
            system.World = _world;

            _systems.Add(typeof(T), system);

            if (executionType == SystemExecutionType.Draw)
            {
                if (!_drawLayers.ContainsKey(priority))
                {
                    _drawLayers.Add(priority, new List<IEntitySystem>());
                }

                List<IEntitySystem> drawBag = _drawLayers[priority];
                
                if (drawBag == null)
                {
                    drawBag = new List<IEntitySystem>();
                    _drawLayers[priority] = drawBag;
                }

                if (!drawBag.Contains(system))
                {
                    drawBag.Add(system);
                }
            }
            else if (executionType == SystemExecutionType.Update)
            {
                if (!_updateLayers.ContainsKey(priority))
                {
                    _updateLayers.Add(priority, new List<IEntitySystem>());
                }

                List<IEntitySystem> updateBag = _updateLayers[priority];
                if (updateBag == null)
                {
                    updateBag = new List<IEntitySystem>();
                    _updateLayers[priority] = updateBag;
                }

                if (!updateBag.Contains(system))
                {
                    updateBag.Add(system);
                }
            }

            if (!_mergedBag.Contains(system))
            {
                _mergedBag.Add(system);
            }

            system.SystemBit = SystemBitManager.GetBitFor(system);

            return system;
        }

        /// <summary>
        /// Gets the specific <see cref="IEntitySystem"/> instance for the provided type.
        /// </summary>
        /// <typeparam name="T"><see cref="IEntitySystem"/> type to look up.</typeparam>
        /// <returns>The specific <see cref="IEntitySystem"/> instance for the provided type, or null if none is found.</returns>
        public T GetSystem<T>() where T : IEntitySystem
        {
            IEntitySystem system = null;
            _systems.TryGetValue(typeof(T), out system);

            return (T)system;
        }

        /// <summary>
        /// Gets a list of all registered <see cref="IEntitySystem"/> instances in the manager.
        /// </summary>
        /// <returns>A list of all registered <see cref="IEntitySystem"/> instances in the manager.</returns>
        /// <remarks>Slow. Should only be used for debugging purposes.</remarks>
        public List<IEntitySystem> GetSystems()
        {
            return _mergedBag;
        }

        /// <summary>
        /// Initializes all <see cref="IEntitySystem"/> instances in the manager.
        /// </summary>
        public void InitializeAll()
        {
            foreach (IEntitySystem system in _mergedBag)
            {
                system.Initialize();
            }
        }

        /// <summary>
        /// Synchronously updates all <see cref="IEntitySystem"/> instances of the specified execution type.
        /// </summary>
        /// <param name="executionType">Execution type of systems to update.</param>
        public void UpdateSynchronous(SystemExecutionType executionType)
        {
            if (executionType == SystemExecutionType.Draw)
            {
                foreach (List<IEntitySystem> bag in _drawLayers.Values)
                {
                    UpdateBagSync(bag);
                }
            }
            else if (executionType == SystemExecutionType.Update)
            {
                foreach (List<IEntitySystem> bag in _updateLayers.Values)
                {
                    UpdateBagSync(bag);
                }
            }
        }

        /// <summary>
        /// Asynchronously updates all <see cref="IEntitySystem"/> instances of the specified execution type.
        /// </summary>
        /// <param name="executionType">Execution type of systems to update.</param>
        public void UpdateAsynchronous(SystemExecutionType executionType)
        {
            if (executionType == SystemExecutionType.Draw)
            {
                foreach (List<IEntitySystem> bag in _drawLayers.Values)
                {
                    UpdateBagAsync(bag);
                }
            }
            else if (executionType == SystemExecutionType.Update)
            {
                foreach (List<IEntitySystem> bag in _updateLayers.Values)
                {
                    UpdateBagAsync(bag);
                }
            }
        }

        #endregion
    }
}
