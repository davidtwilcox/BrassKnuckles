using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using Nuclex.UserInterface.Controls;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.UI
{
    /// <summary>
    /// Auxiliary component used to attach a Nuclex UI control to an entity.
    /// </summary>
    public class ControlComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the <see cref="Nuclex.UserInterface.Controls.Control"/> instance attached to the entity.
        /// </summary>
        public Control Control { get; set; }

        /// <summary>
        /// Gets or sets the offset of the control's position from the entity's position.
        /// </summary>
        public Vector2 Offset { get; set; }

        #endregion
    }
}
