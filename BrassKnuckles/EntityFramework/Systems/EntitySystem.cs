using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework.Managers;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// Base interface for systems that process <see cref="Entity"/> instances.
    /// </summary>
    public interface IEntitySystem
    {
        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="EntityWorld"/> instance associated with the system.
        /// </summary>
        EntityWorld World { get; set; }

        /// <summary>
        /// Gets or sets the system bit flag assigned to the system.
        /// </summary>
        long SystemBit { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Performs any necessary system initialization.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> is added to the system.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was added.</param>
        void Added(Entity entity);

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> is removed from the system.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was removed.</param>
        void Removed(Entity entity);

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> has changed.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was changed.</param>
        void Change(Entity entity);

        /// <summary>
        /// Processes all <see cref="Entity"/> instances associated with the system.
        /// </summary>
        void Process();

        /// <summary>
        /// Enables the system.
        /// </summary>
        void Enable();

        /// <summary>
        /// Disables the system.
        /// </summary>
        void Disable();

        /// <summary>
        /// Toggles the <see cref="IsEnabled"/> property.
        /// </summary>
        void ToggleEnabled();

        /// <summary>
        /// Processes the entity provided.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to process.</param>
        void Process(Entity entity);

        #endregion
    }

    /// <summary>
    /// Base class for all systems that process <see cref="Entity"/> instances.
    /// </summary>
    public abstract class EntitySystem : IEntitySystem
    {
        #region Private fields

        private long _typeFlags;
        private Dictionary<int, Entity> _activeEntities;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="EntityWorld"/> instance associated with the system.
        /// </summary>
        public EntityWorld World { get; set; }

        /// <summary>
        /// Gets or sets the system bit flag assigned to the system.
        /// </summary>
        public long SystemBit { get; set; }

        /// <summary>
        /// Gets or sets whether or not the system is enabled.
        /// </summary>
        protected bool IsEnabled { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new EntitySystem instance.
        /// </summary>
        public EntitySystem()
        {
            _activeEntities = new Dictionary<int, Entity>();
            IsEnabled = true;
        }

        /// <summary>
        /// Creates a new EntitySystem instance associated with the provided types.
        /// </summary>
        /// <param name="types">Types to associate with the EntitySystem.</param>
        public EntitySystem(params Type[] types)
            : this()
        {
            int typeCount = types.Length;
            for (int i = 0; i < typeCount; ++i)
            {
                Type type = types[i];
                ComponentType componentType = ComponentTypeManager.GetTypeFor(type);
                _typeFlags |= componentType.Bit;
            }
        }

        /// <summary>
        /// Creates a new EntitySystem instance associated with the merged list of required
        /// and other types.
        /// </summary>
        /// <param name="requiredType">Required type.</param>
        /// <param name="otherTypes">Other types to associate with.</param>
        public EntitySystem(Type requiredType, params Type[] otherTypes)
            : this(GetMergedTypes(requiredType, otherTypes))
        {
        }

        #endregion

        #region Private methods

        private void Remove(Entity entity)
        {
            _activeEntities.Remove(entity.Id);
            entity.RemoveSystemBit(SystemBit);
            Removed(entity);
        }

        #endregion

        #region Protected methods

        protected virtual void Begin()
        {
        }

        protected virtual void End()
        {
        }

        protected virtual void ProcessEntities(Dictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                Process(entity);
            }
        }

        protected virtual bool CheckProcessing()
        {
            return IsEnabled;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Performs any necessary system initialization.
        /// </summary>
        public virtual void Initialize()
        {
        }

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> is added to the system.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was added.</param>
        public virtual void Added(Entity entity)
        {
        }

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> is removed from the system.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was removed.</param>
        public virtual void Removed(Entity entity)
        {
        }

        /// <summary>
        /// Handles any actions required when an <see cref="Entity"/> has changed.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> that was changed.</param>
        public void Change(Entity entity)
        {
            bool contains = (SystemBit & entity.SystemBits) == SystemBit;
            bool interest = (_typeFlags & entity.TypeBits) == _typeFlags;

            if (interest && !contains && (_typeFlags > 0))
            {
                _activeEntities.Add(entity.Id, entity);
                entity.AddSystemBit(SystemBit);
                Added(entity);
            }
            else if (!interest && contains && (_typeFlags > 0))
            {
                Remove(entity);
            }
        }

        /// <summary>
        /// Processes all <see cref="Entity"/> instances associated with the system.
        /// </summary>
        public virtual void Process()
        {
            if (CheckProcessing())
            {
                Begin();
                ProcessEntities(_activeEntities);
                End();
            }
        }

        /// <summary>
        /// Enables the system.
        /// </summary>
        public void Enable()
        {
            IsEnabled = true;
        }

        /// <summary>
        /// Disables the system.
        /// </summary>
        public void Disable()
        {
            IsEnabled = false;
        }

        /// <summary>
        /// Toggles the <see cref="IsEnabled"/> property.
        /// </summary>
        public void ToggleEnabled()
        {
            IsEnabled = !IsEnabled;
        }

        /// <summary>
        /// Processes the entity provided.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to process.</param>
        public abstract void Process(Entity entity);

        /// <summary>
        /// Merges the required type instance and optional array of other type instances into a single array.
        /// </summary>
        /// <param name="requiredType">Required type.</param>
        /// <param name="otherTypes">Optional array of additional types.</param>
        /// <returns>Merged array with the required type at index 0.</returns>
        public static Type[] GetMergedTypes(Type requiredType, params Type[] otherTypes)
        {
            Type[] types = new Type[otherTypes.Length + 1];
            types[0] = requiredType;
            int typeCount = otherTypes.Length;
            for (int i = 0; i < typeCount; ++i)
            {
                types[i + 1] = otherTypes[i];
            }

            return types;
        }

        #endregion
    }
}
