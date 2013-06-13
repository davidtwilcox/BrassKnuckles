using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;
using BrassKnuckles.EntityFramework.Managers;

namespace BrassKnuckles.EntityFramework
{
    /// <summary>
    /// Simple unique identifier for an item in the framework. 
    /// </summary>
    [Serializable]
    public sealed class Entity
    {
        #region Private fields

        [NonSerialized]
        private EntityWorld _world;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the entity ID. 
        /// </summary>
        /// <remarks>Can be recycled as entities are removed and reused.</remarks>
        public int Id { get; private set; }

        /// <summary>
        /// Gets or sets the unique ID for the entity.
        /// </summary>
        /// <remarks>Always unique and never recycled.</remarks>
        public long UniqueId { get; set; }

        /// <summary>
        /// Gets or sets the bit flags indicating the component types associated with the entity.
        /// </summary>
        public long TypeBits { get; set; }

        /// <summary>
        /// Gets or sets the bit flags indicating the system types associated with the entity.
        /// </summary>
        public long SystemBits { get; set; }

        /// <summary>
        /// Gets or sets the entity tag.
        /// </summary>
        public string Tag
        {
            get { return _world.TagManager.GetTagOfEntity(this); }
            set { _world.TagManager.AssignTag(value, this); }
        }

        /// <summary>
        /// Gets or sets the entity group name.
        /// </summary>
        public string Group
        {
            get { return _world.GroupManager.GetGroupFor(this); }
            set { _world.GroupManager.AddToGroup(value, this); }
        }

        /// <summary>
        /// Determines if the entity is active in the system. True if the entity is still active, 
        /// false if the entity has been deleted.
        /// </summary>
        public bool IsActive
        {
            get { return _world.EntityManager.IsActive(Id); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Entity instance assigned to the specified world and with the ID provided.
        /// </summary>
        /// <param name="world"><see cref="EntityWorld"/> instance to which the Entity instance is assigned.</param>
        /// <param name="id">Value to assign to the Entity <see cref="Id"/> property.</param>
        public Entity(EntityWorld world, int id)
        {
            _world = world;
            Id = id;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the component type bit to the <see cref="TypeBits"/> property.
        /// </summary>
        /// <param name="bit">Component type bit flag to add.</param>
        public void AddTypeBit(long bit)
        {
            TypeBits |= bit;
        }

        /// <summary>
        /// Removes the component type bit from the <see cref="TypeBits"/> property.
        /// </summary>
        /// <param name="bit">Component type bit flag to remove.</param>
        public void RemoveTypeBit(long bit)
        {
            TypeBits &= ~bit;
        }

        /// <summary>
        /// Adds the system type bit to the <see cref="SystemBits"/> property.
        /// </summary>
        /// <param name="bit">System type bit flag to add.</param>
        public void AddSystemBit(long bit)
        {
            SystemBits |= bit;
        }

        /// <summary>
        /// Removes the system type bit from the <see cref="SystemBits"/> property.
        /// </summary>
        /// <param name="bit">System type bit flag to remove.</param>
        public void RemoveSystemBit(long bit)
        {
            SystemBits &= ~bit;
        }

        /// <summary>
        /// Adds a component type to the Entity.
        /// </summary>
        /// <param name="component"><see cref="IComponent"/> instance to add to the Entity.</param>
        public void AddComponent(IComponent component)
        {
            _world.EntityManager.AddComponent(this, component);
        }

        /// <summary>
        /// Adds a component of type T to the Entity.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IComponent"/> to add.</typeparam>
        /// <param name="component"><see cref="IComponent"/> instance to add to the Entity.</param>
        public void AddComponent<T>(T component) where T : IComponent
        {
            _world.EntityManager.AddComponent<T>(this, component);
        }

        /// <summary>
        /// Removes a component type from the Entity.
        /// </summary>
        /// <param name="type"><see cref="IComponent"/> instance to remove from the Entity.</param>
        public void RemoveComponent(ComponentType type)
        {
            _world.EntityManager.RemoveComponent(this, type);
        }

        /// <summary>
        /// Remove a component of type T from the Entity.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="IComponent"/> to remove.</typeparam>
        /// <param name="component"><see cref="IComponent"/> instance to remove from the Entity.</param>
        public void RemoveComponent<T>(T component) where T : IComponent
        {
            _world.EntityManager.RemoveComponent<T>(this, component);
        }

        /// <summary>
        /// Gets a component of the specified type associated with the Entity.
        /// </summary>
        /// <param name="type"><see cref="ComponentType"/> indicating type of <see cref="IComponent"/> to get for the Entity.</param>
        /// <returns>An <see cref="IComponent"/> instance of the specified type if one is associated with the Entity, or null if not.</returns>
        /// <remarks>This method has higher performance than <see cref="GetComponent{T}"/> and is the preferred method.</remarks>
        public IComponent GetComponent(ComponentType type)
        {
            return _world.EntityManager.GetComponent(this, type);
        }

        /// <summary>
        /// Gets a component of type T associated with the Entity.
        /// </summary>
        /// <typeparam name="T"><see cref="IComponent"/> type to get for the Entity.</typeparam>
        /// <returns>>An <see cref="IComponent"/> instance of the specified type if one is associated with the Entity, or null if not.</returns>
        public T GetComponent<T>() where T : IComponent
        {
            return (T)GetComponent(ComponentTypeManager.GetTypeFor<T>());
        }

        /// <summary>
        /// Gets all components associated with the Entity.
        /// </summary>
        /// <returns>A <see cref="Bag"/> containing all <see cref="IComponent"/> instances associated with the Entity.</returns>
        /// <remarks>Extremely slow. Should only be used for debugging purposes.</remarks>
        public List<IComponent> GetComponents()
        {
            return _world.EntityManager.GetComponents(this);
        }

        /// <summary>
        /// Refreshes all component changes for the Entity.
        /// </summary>
        /// <remarks>Should always be called after all calls to add or remove components from the entity have been completed so that
        /// the associated systems can perform any necessary updates.</remarks>
        public void Refresh()
        {
            _world.RefreshEntity(this);
        }

        /// <summary>
        /// Deletes the Entity from its associated <see cref="EntityWorld"/> instance.
        /// </summary>
        public void Delete()
        {
            _world.DeleteEntity(this);
        }

        /// <summary>
        /// Resets all type and system bit flags for the Entity.
        /// </summary>
        public void Reset()
        {
            TypeBits = 0;
            SystemBits = 0;
        }

        /// <summary>
        /// Converts the Entity instance to its equivalent string representation.
        /// </summary>
        /// <returns>The string representation of the Entity instance.</returns>
        public override string ToString()
        {
            return string.Format("Entity[{0}]", Id);
        }

        #endregion
    }
}
