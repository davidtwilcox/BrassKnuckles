using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls;
using BrassKnuckles.EntityFramework;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.UI
{
    /// <summary>
    /// Extension methods for <see cref="Nuclex.UserInterface.Controls.Control"/> instances.
    /// </summary>
    public static class ControlExtension
    {
        #region Public methods

        /// <summary>
        /// Attaches a control to an <see cref="Entity"/> instance, using the specified offset.
        /// </summary>
        /// <param name="control">Control instance being attached to the entity.</param>
        /// <param name="entity">Entity instance to which the control is being attached.</param>
        /// <param name="offset">Offset from the entity's position to display the control.</param>
        public static void AttachToEntity(this Control control, Entity entity, Vector2 offset)
        {
            ControlComponent controlComponent = new ControlComponent()
            {
                Control = control,
                Offset = offset
            };

            entity.AddComponent<ControlComponent>(controlComponent);
            entity.Refresh();
        }

        #endregion
    }
}
