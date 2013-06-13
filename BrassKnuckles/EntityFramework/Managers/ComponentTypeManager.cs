using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Manages <see cref="ComponentType"/> mappings to <see cref="IComponent"/> implementations.
    /// </summary>
    public static class ComponentTypeManager
    {
        #region Private fields

        private static Dictionary<Type, ComponentType> _componentTypes = new Dictionary<Type, ComponentType>();

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the <see cref="ComponentType"/> mapped to the <see cref="IComponent"/> type T.
        /// </summary>
        /// <typeparam name="T"><see cref="IComponent"/> type.</typeparam>
        /// <returns>The <see cref="ComponentType"/> mapped to the requested <see cref="IComponent"/> type T.</returns>
        public static ComponentType GetTypeFor<T>() where T : IComponent
        {
            ComponentType componentType = null;
            Type receivedType = typeof(T);

            if (!_componentTypes.TryGetValue(receivedType, out componentType))
            {
                componentType = new ComponentType();
                _componentTypes.Add(receivedType, componentType);
            }

            return componentType;
        }

        /// <summary>
        ///  Gets the <see cref="ComponentType"/> mapped to the type provided.
        /// </summary>
        /// <param name="component"><see cref="IComponent"/> type to look up.</param>
        /// <returns>The <see cref="ComponentType"/> mapped to the type provided.</returns>
        public static ComponentType GetTypeFor(Type component)
        {
            ComponentType componentType = null;
            if (!_componentTypes.TryGetValue(component, out componentType))
            {
                componentType = new ComponentType();
                _componentTypes.Add(component, componentType);
            }

            return componentType;
        }

        /// <summary>
        /// Gets the type ID for <see cref="IComponent"/> type T.
        /// </summary>
        /// <typeparam name="T"><see cref="IComponent"/> type.</typeparam>
        /// <returns>Type ID.</returns>
        public static int GetId<T>() where T : IComponent
        {
            return GetTypeFor<T>().Id;
        }

        /// <summary>
        /// Gets the type bit flag for <see cref="IComponent"/> type T.
        /// </summary>
        /// <typeparam name="T"><see cref="IComponent"/> type.</typeparam>
        /// <returns>Type bit flag.</returns>
        public static long GetBit<T>() where T : IComponent
        {
            return GetTypeFor<T>().Bit;
        }

        #endregion
    }
}
