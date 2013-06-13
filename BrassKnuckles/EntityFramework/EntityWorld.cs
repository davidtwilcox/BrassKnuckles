using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;
using BrassKnuckles.EntityFramework.Managers;
using Microsoft.Xna.Framework;
using BrassKnuckles.Input;

namespace BrassKnuckles.EntityFramework
{
    public interface IEntityWorld : IGameComponent
    {
        #region Properties

        /// <summary>
        /// Gets the shared SystemManager instance.
        /// </summary>
        SystemManager SystemManager { get; }
        
        /// <summary>
        /// Gets the shared EntityManager instance.
        /// </summary>
        EntityManager EntityManager { get; }

        /// <summary>
        /// Gets the shared TagManager instance.
        /// </summary>
        TagManager TagManager { get; }

        /// <summary>
        /// Gets the shared GroupManager instance.
        /// </summary>
        GroupManager GroupManager { get; }

        /// <summary>
        /// Gets or sets the elapsed time in milliseconds since the last game tick.
        /// </summary>
        float ElapsedTime { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an <see cref="IManager"/> instance to the world.
        /// </summary>
        /// <param name="manager"><see cref="IManager"/> instance to add.</param>
        void AddManager(IManager manager);

        /// <summary>
        /// Gets an <see cref="IManager"/> instance of type T.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IManager"/> to retrieve.</typeparam>
        /// <returns>An <see cref="IManager"/> instance of type T if found, or null if not.</returns>
        T GetManager<T>() where T : IManager;

        /// <summary>
        /// Gets the <see cref="Entity"/> instance with the specified ID.
        /// </summary>
        /// <param name="entityId">ID of the <see cref="Entity"/> instance to retrieve.</param>
        /// <returns>The <see cref="Entity"/> with the specified ID, or null if not found.</returns>
        Entity GetEntity(int entityId);

        /// <summary>
        /// Deletes the provided <see cref="Entity"/> from the world.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to delete.</param>
        void DeleteEntity(Entity entity);

        /// <summary>
        /// Refreshes the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to refresh.</param>
        void RefreshEntity(Entity entity);

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Entity"/> instance.</returns>
        Entity CreateEntity();

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance with the provided tag.
        /// </summary>
        /// <param name="tag">Tag to assign to the new <see cref="Entity"/> instance.</param>
        /// <returns>A new <see cref="Entity"/> instance with the provided tag.</returns>
        Entity CreateEntity(string tag);

        /// <summary>
        /// Gets all active <see cref="Entity"/> instances in the world and a <see cref="Bag"/> containing all associated <see cref="IComponent"/> instances for the entity.
        /// </summary>
        /// <returns>Dictionary with <see cref="Entity"/> instances as keys and their associated <see cref="IComponent"/> instances as values.</returns>
        Dictionary<Entity, List<IComponent>> GetCurrentState();

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance in the world and assigns the provided tag, group and <see cref="IComponent"/> instances to it.
        /// </summary>
        /// <param name="tag">Tag for the new entity.</param>
        /// <param name="group">Group for the new entity.</param>
        /// <param name="components"><see cref="IEnumerable"/> containing all <see cref="IComponent"/> instances to associate with new entity.</param>
        void LoadEntityState(string tag, string group, IEnumerable<IComponent> components);

        #endregion
    }

    /// <summary>
    /// Central access point for all managers and shared data required for an instance of an entity system. 
    /// </summary>
    public sealed class EntityWorld : GameComponent, IEntityWorld
    {
        #region Private fields

        private List<Entity> _refreshedEntities;
        private List<Entity> _deletedEntities;
        private Dictionary<Type, IManager> _managers;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the shared SystemManager instance.
        /// </summary>
        public SystemManager SystemManager { get; private set; }
        
        /// <summary>
        /// Gets the shared EntityManager instance.
        /// </summary>
        public EntityManager EntityManager { get; private set; }

        /// <summary>
        /// Gets the shared TagManager instance.
        /// </summary>
        public TagManager TagManager { get; private set; }

        /// <summary>
        /// Gets the shared GroupManager instance.
        /// </summary>
        public GroupManager GroupManager { get; private set; }

        /// <summary>
        /// Gets or sets the elapsed time in milliseconds since the last game tick.
        /// </summary>
        public float ElapsedTime { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new EntityWorld instance.
        /// </summary>
        /// <param name="game"><see cref="Game"/> instance to use.</param>
        public EntityWorld(Game game) 
            : base(game)
        {
            SystemManager = new SystemManager(this);
            EntityManager = new EntityManager(this);
            TagManager = new TagManager(this);
            GroupManager = new GroupManager(this);
            _refreshedEntities = new List<Entity>();
            _deletedEntities = new List<Entity>();
            _managers = new Dictionary<Type, IManager>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds an <see cref="IManager"/> instance to the world.
        /// </summary>
        /// <param name="manager"><see cref="IManager"/> instance to add.</param>
        public void AddManager(IManager manager)
        {
            Type type = manager.GetType();
            if (_managers.ContainsKey(type))
            {
                _managers[type] = manager;
            }
            else
            {
                _managers.Add(type, manager);
            }
        }

        /// <summary>
        /// Gets an <see cref="IManager"/> instance of type T.
        /// </summary>
        /// <typeparam name="T">Type of <see cref="IManager"/> to retrieve.</typeparam>
        /// <returns>An <see cref="IManager"/> instance of type T if found, or null if not.</returns>
        public T GetManager<T>() where T : IManager
        {
            IManager manager;
            _managers.TryGetValue(typeof(T), out manager);

            return (T)manager;
        }

        /// <summary>
        /// Gets the <see cref="Entity"/> instance with the specified ID.
        /// </summary>
        /// <param name="entityId">ID of the <see cref="Entity"/> instance to retrieve.</param>
        /// <returns>The <see cref="Entity"/> with the specified ID, or null if not found.</returns>
        public Entity GetEntity(int entityId)
        {
            return EntityManager.GetEntity(entityId);
        }

        /// <summary>
        /// Deletes the provided <see cref="Entity"/> from the world.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to delete.</param>
        public void DeleteEntity(Entity entity)
        {
            GroupManager.RemoveFromGroup(entity);
            if (!_deletedEntities.Contains(entity))
            {
                _deletedEntities.Add(entity);
            }
        }

        /// <summary>
        /// Refreshes the provided <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to refresh.</param>
        public void RefreshEntity(Entity entity)
        {
            _refreshedEntities.Add(entity);
        }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance.
        /// </summary>
        /// <returns>A new <see cref="Entity"/> instance.</returns>
        public Entity CreateEntity()
        {
            return EntityManager.Create();
        }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance with the provided tag.
        /// </summary>
        /// <param name="tag">Tag to assign to the new <see cref="Entity"/> instance.</param>
        /// <returns>A new <see cref="Entity"/> instance with the provided tag.</returns>
        public Entity CreateEntity(string tag)
        {
            Entity entity = EntityManager.Create();
            TagManager.AssignTag(tag, entity);

            return entity;
        }

        /// <summary>
        /// Starts the world loop. Should be called each game tick.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            ElapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_refreshedEntities.Count > 0)
            {
                int refreshedCount = _refreshedEntities.Count;
                for (int i = 0; i < refreshedCount; ++i)
                {
                    EntityManager.Refresh(_refreshedEntities[i]);
                }

                _refreshedEntities.Clear();
            }

            if (_deletedEntities.Count > 0)
            {
                int deletedCount = _deletedEntities.Count;
                for (int i = 0; i < deletedCount; ++i)
                {
                    Entity entity = _deletedEntities[i];
                    EntityManager.Remove(entity);
                }

                _deletedEntities.Clear();
            }
        }

        /// <summary>
        /// Gets all active <see cref="Entity"/> instances in the world and a <see cref="Bag"/> containing all associated <see cref="IComponent"/> instances for the entity.
        /// </summary>
        /// <returns>Dictionary with <see cref="Entity"/> instances as keys and their associated <see cref="IComponent"/> instances as values.</returns>
        public Dictionary<Entity, List<IComponent>> GetCurrentState()
        {
            List<Entity> entities = EntityManager.ActiveEntities;
            Dictionary<Entity, List<IComponent>> currentState = new Dictionary<Entity, List<IComponent>>();

            int entityCount = entities.Count;
            for (int i = 0; i < entityCount; ++i)
            {
                Entity entity = entities[i];
                List<IComponent> components = entity.GetComponents();
                currentState.Add(entity, components);
            }

            return currentState;
        }

        /// <summary>
        /// Creates a new <see cref="Entity"/> instance in the world and assigns the provided tag, group and <see cref="IComponent"/> instances to it.
        /// </summary>
        /// <param name="tag">Tag for the new entity.</param>
        /// <param name="group">Group for the new entity.</param>
        /// <param name="components"><see cref="IEnumerable"/> containing all <see cref="IComponent"/> instances to associate with new entity.</param>
        public void LoadEntityState(string tag, string group, IEnumerable<IComponent> components)
        {
            Entity entity;

            if (!string.IsNullOrEmpty(tag))
            {
                entity = CreateEntity(tag);
            }
            else
            {
                entity = CreateEntity();
            }

            if (!string.IsNullOrEmpty(group))
            {
                GroupManager.AddToGroup(group, entity);
            }

            foreach (IComponent component in components)
            {
                entity.AddComponent(component);
            }

            entity.Refresh();
        }

        #endregion
    }
}
