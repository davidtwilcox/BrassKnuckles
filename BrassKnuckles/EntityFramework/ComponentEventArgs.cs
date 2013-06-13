using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework
{
    /// <summary>
    /// Contains <see cref="IComponent"/> related event arguments.
    /// </summary>
    public class ComponentEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the <see cref="Entity"/> for the event.
        /// </summary>
        public Entity Entity { get; protected set; }

        /// <summary>
        /// Gets the <see cref="IComponent"/> for the event.
        /// </summary>
        public IComponent Component { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ComponentEventArgs instance for the specified <see cref="Entity"/> and <see cref="IComponent"/>.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> for the event.</param>
        /// <param name="component">The <see cref="IComponent"/> for the event.</param>
        public ComponentEventArgs(Entity entity, IComponent component)
        {
            Entity = entity;
            Component = component;
        }

        #endregion
    }
}
