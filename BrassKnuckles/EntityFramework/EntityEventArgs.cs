using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework
{
    /// <summary>
    /// Contains <see cref="Entity"/> related event arguments.
    /// </summary>
    public class EntityEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// Gets the source <see cref="Entity"/> for the event.
        /// </summary>
        public Entity SourceEntity { get; protected set; }

        /// <summary>
        /// Gets the target <see cref="Entity"/> for the event.
        /// </summary>
        public Entity TargetEntity { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new EntityEventArgs instance using the provided source <see cref="Entity"/> instance.
        /// </summary>
        /// <param name="sourceEntity">Source <see cref="Entity"/> for the event.</param>
        public EntityEventArgs(Entity sourceEntity)
            : this(sourceEntity, null)
        {
        }

        /// <summary>
        /// Creates a new EntityEventArgs instance using the provided
        /// source and target <see cref="Entity"/> instances.
        /// </summary>
        /// <param name="sourceEntity">Source <see cref="Entity"/> for the event.</param>
        /// <param name="targetEntity">Target <see cref="Entity"/> for the event.</param>
        public EntityEventArgs(Entity sourceEntity, Entity targetEntity)
        {
            SourceEntity = sourceEntity;
            TargetEntity = targetEntity;
        }

        #endregion
    }
}
