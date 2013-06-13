using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;
using BrassKnuckles.EntityFramework.Systems;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Manages <see cref="Entity"/> instances.
    /// </summary>
    public sealed class EntityManager : IManager
    {
        #region Private fields

        private EntityWorld _world;
        private Queue<Entity> _removedAndAvailable;
        private int _nextAvailableId;
        private long _uniqueEntityId;
        private Dictionary<int, Dictionary<int, IComponent>> _componentsByType;

        #endregion

        #region Properties

        private Dictionary<int, Entity> _activeEntityTable;
        /// <summary>
        /// Gets a <see cref="List"/> containing all active <see cref="Entity"/> instances.
        /// </summary>
        public List<Entity> ActiveEntities
        {
            get { return _activeEntityTable.Values.ToList(); }
        }

        /// <summary>
        /// Gets the number of <see cref="Entity"/> instances being managed.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the total number of <see cref="Entity"/> instances created over the lifetime of the manager.
        /// </summary>
        public long TotalCreated { get; private set; }

        /// <summary>
        /// Gets the total number of <see cref="Entity"/> instances removed over the lifetime of the manager.
        /// </summary>
        public long TotalRemoved { get; private set; }

        #endregion

        #region Events

        /// <summary>
        /// Raised when an <see cref="Entity"/> is added to the manager.
        /// </summary>
        public event EventHandler<EntityEventArgs> EntityAdded;

        /// <summary>
        /// Raised when an <see cref="Entity"/> is removed from the manager.
        /// </summary>
        public event EventHandler<EntityEventArgs> EntityRemoved;

        /// <summary>
        /// Raised when an <see cref="IComponent"/> is added to an <see cref="Entity"/> in the manager.
        /// </summary>
        public event EventHandler<ComponentEventArgs> ComponentAdded;

        /// <summary>
        /// Raised when an <see cref="IComponent"/> is removed from an <see cref="Entity"/> in the manager.
        /// </summary>
        public event EventHandler<ComponentEventArgs> ComponentRemoved;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new EntityManager instance in the provided <see cref="EntityWorld"/>.
        /// </summary>
        /// <param name="world"><see cref="EntityWorld"/> instance in which the manager will be created.</param>
        public EntityManager(EntityWorld world)
        {
            if (world == null)
            {
                throw new ArgumentNullException("world");
            }

            _world = world;
            _removedAndAvailable = new Queue<Entity>();
            _componentsByType = new Dictionary<int, Dictionary<int, IComponent>>();
            _activeEntityTable = new Dictionary<int, Entity>();
        }

        #endregion

        #region Private methods

        private void OnEntityAdded(Entity entity)
        {
            if (EntityAdded != null)
            {
                EntityAdded(this, new EntityEventArgs(entity));
            }
        }

        private void OnEntityRemoved(Entity entity)
        {
            if (EntityRemoved != null)
            {
                EntityRemoved(this, new EntityEventArgs(entity));
            }
        }

        private void OnComponentAdded(Entity entity, IComponent component)
        {
            if (ComponentAdded != null)
            {
                ComponentAdded(this, new ComponentEventArgs(entity, component));
            }
        }

        private void OnComponentRemoved(Entity entity, IComponent component)
        {
            if (ComponentRemoved != null)
            {
                ComponentRemoved(this, new ComponentEventArgs(entity, component));
            }
        }

        private void RemoveComponentsOfEntity(Entity entity)
        {
            int entityId = entity.Id;
            foreach (Dictionary<int, IComponent> componentTable in _componentsByType.Values)
            {
                IComponent component;
                if (componentTable.TryGetValue(entityId, out component))
                {
                    componentTable.Remove(entityId);
                    OnComponentRemoved(entity, component);
                }
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Entity"/> instance.</returns>
        public Entity Create()
        {
            Entity entity = null;
            if (_removedAndAvailable.Count > 0)
            {
                entity = _removedAndAvailable.Dequeue();
                entity.Reset();
            }
            else
            {
                entity = new Entity(_world, _nextAvailableId++);
            }

            entity.UniqueId = _uniqueEntityId++;

            _activeEntityTable.Add(entity.Id, entity);
            
            ++Count;
            ++TotalCreated;

            OnEntityAdded(entity);

            return entity;
        }

        /// <summary>
        /// Removes the provided <see cref="Entity"/> from the manager.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to remove.</param>
        public void Remove(Entity entity)
        {
            _activeEntityTable.Remove(entity.Id);

            entity.TypeBits = 0;

            Refresh(entity);

            RemoveComponentsOfEntity(entity);

            --Count;
            ++TotalRemoved;

            _removedAndAvailable.Enqueue(entity);

            OnEntityRemoved(entity);
        }

        /// <summary>
        /// Determines if an <see cref="Entity"/> with the specified ID is active in the manager.
        /// </summary>
        /// <param name="entityId">ID of the <see cref="Entity"/> to check.</param>
        /// <returns>True if an <see cref="Entity"/> with the specified ID is active in the manager,
        /// or false if not.</returns>
        public bool IsActive(int entityId)
        {
            return _activeEntityTable.ContainsKey(entityId);
        }

        /// <summary>
        /// Gets the <see cref="Entity"/> instance with the specified ID.
        /// </summary>
        /// <param name="entityId">ID of the <see cref="Entity"/> to retrieve.</param>
        /// <returns>The <see cref="Entity"/> instance with the specified ID if it exists,
        /// or false if not.</returns>
        public Entity GetEntity(int entityId)
        {
            Entity entity;
            if (_activeEntityTable.TryGetValue(entityId, out entity))
            {
                return entity;
            }

            return null;
        }

        /// <summary>
        /// Gets the associated <see cref="IComponent"/> instance of the specified <see cref="ComponentType"/>
        /// for the <see cref="Entity"/> provided.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to look up.</param>
        /// <param name="type"><see cref="ComponentType"/> to look up.</param>
        /// <returns>The associated <see cref="IComponent"/> instance of the specified type for the entity provided,
        /// or null if none exists.</returns>
        public IComponent GetComponent(Entity entity, ComponentType type)
        {
            int entityId = entity.Id;
            int typeId = type.Id;
            if (_componentsByType.ContainsKey(typeId) && _componentsByType[typeId].ContainsKey(entityId))
            {
                return _componentsByType[typeId][entityId];
            }

            return null;
        }

        /// <summary>
        /// Gets all <see cref="IComponent"/> instances associated with the provided <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to look up.</param>
        /// <returns>A <see cref="Bag"/> containing all <see cref="IComponent"/> instances associated 
        /// with the provided <see cref="Entity"/>.</returns>
        /// <remarks>Slow - should only be used for debugging purposes.</remarks>
        public List<IComponent> GetComponents(Entity entity)
        {
            int entityId = entity.Id;
            List<IComponent> components = new List<IComponent>();
            foreach (Dictionary<int, IComponent> componentTable in _componentsByType.Values)
            {
                IComponent component;
                if (componentTable.TryGetValue(entityId, out component))
                {
                    components.Add(component);
                }
            }

            return components;
        }

        /// <summary>
        /// Adds an <see cref="IComponent"/> instance to the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to which the component will be added.</param>
        /// <param name="component">The <see cref="IComponent"/> instance to be added.</param>
        public void AddComponent(Entity entity, IComponent component)
        {
            ComponentType type = ComponentTypeManager.GetTypeFor(component.GetType());
            int typeId = type.Id;
            if (!_componentsByType.ContainsKey(typeId))
            {
                _componentsByType.Add(typeId, new Dictionary<int, IComponent>());
            }

            int entityId = entity.Id;
            if (!_componentsByType[typeId].ContainsKey(entityId))
            {
                _componentsByType[typeId].Add(entityId, component);
                entity.AddTypeBit(type.Bit);

                OnComponentAdded(entity, component);
            }
        }

        /// <summary>
        /// Adds an <see cref="IComponent"/> instance of type T to the specified <see cref="Entity"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IComponent"/> being added.</typeparam>
        /// <param name="entity">The <see cref="Entity"/> instance to which the component will be added.</param>
        /// <param name="component">The <see cref="IComponent"/> instance to be added.</param>
        public void AddComponent<T>(Entity entity, T component) where T : IComponent
        {
            ComponentType type = ComponentTypeManager.GetTypeFor<T>();
            int typeId = type.Id;
            if (!_componentsByType.ContainsKey(typeId))
            {
                _componentsByType.Add(typeId, new Dictionary<int, IComponent>());
            }

            int entityId = entity.Id;
            if (!_componentsByType[typeId].ContainsKey(entityId))
            {
                _componentsByType[typeId].Add(entityId, component);
                entity.AddTypeBit(type.Bit);

                OnComponentAdded(entity, component);
            }
        }

        /// <summary>
        /// Removes an <see cref="IComponent"/> instance from the specified <see cref="Entity"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance from which the component will be removed.</param>
        /// <param name="type">The <see cref="ComponentType"/> instance describing the type of component to removed.</param>
        public void RemoveComponent(Entity entity, ComponentType type)
        {
            int entityId = entity.Id;
            int typeId = type.Id;

            if (_componentsByType.ContainsKey(typeId) && _componentsByType[typeId].ContainsKey(entityId))
            {
                IComponent component = _componentsByType[typeId][entityId];
                _componentsByType[typeId].Remove(entityId);
                entity.RemoveTypeBit(type.Bit);

                OnComponentRemoved(entity, component);
            }
        }

        /// <summary>
        /// Removes an <see cref="IComponent"/> instance of type T from the specified <see cref="Entity"/>.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IComponent"/> to be removed.</typeparam>
        /// <param name="entity">The <see cref="Entity"/> instance from which the component will be removed.</param>
        /// <param name="component">The <see cref="ComponentType"/> instance describing the type of component to removed.</param>
        public void RemoveComponent<T>(Entity entity, T component) where T : IComponent
        {
            ComponentType type = ComponentTypeManager.GetTypeFor<T>();
            RemoveComponent(entity, type);
        }

        /// <summary>
        /// Refreshes the provided <see cref="Entity"/> state within all systems to which it belongs.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> to refresh.</param>
        public void Refresh(Entity entity)
        {
            List<IEntitySystem> systems = _world.SystemManager.GetSystems();
            foreach (IEntitySystem system in systems)
            {
                system.Change(entity);
            }
        }

        #endregion
    }
}
