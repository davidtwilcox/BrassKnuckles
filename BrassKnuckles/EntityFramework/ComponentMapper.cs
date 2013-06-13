using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework.Managers;

namespace BrassKnuckles.EntityFramework
{
    /// <summary>
    /// Maps <see cref="Entity"/> instances to the specific <see cref="IComponent"/> instances of component type T they use.
    /// </summary>
    /// <typeparam name="T">Type of IComponent being mapped.</typeparam>
    public sealed class ComponentMapper<T> where T : IComponent
    {
        #region Private fields

        private IEntityWorld _world;
        private ComponentType _componentType;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ComponentMapper instance associated with the specified entity world.
        /// </summary>
        /// <param name="world">Entity world instance to associate with.</param>
        public ComponentMapper(IEntityWorld world)
        {
            _world = world;
            _componentType = ComponentTypeManager.GetTypeFor<T>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the <see cref="IComponent"/> instance of type T associated with the specified <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="entity"><see cref="Entity"/> instance to look up.</param>
        /// <returns>An <see cref="IComponent"/> instance of type T associated with an entity, or null if none exists.</returns>
        public T Get(Entity entity)
        {
            return (T)_world.EntityManager.GetComponent(entity, _componentType);
        }

        #endregion
    }
}
