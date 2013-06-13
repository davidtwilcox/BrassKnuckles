using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using Microsoft.Xna.Framework;

namespace BrassKnuckles
{
    /// <summary>
    /// Used to set the world position of an <see cref="Entity"/>.
    /// </summary>
    public class PositionComponent : IComponent
    {
        #region Fields
        private Vector2 _position;
        private float _rotation;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the world coordinates for an entity.
        /// </summary>
        public Vector2 Position 
        {
            get { return _position; }
            set
            {
                if (value != Position)
                {
                    PreviousPosition = _position;
                    _position = value;
                    Updated = true;
                }
            }
        }

        /// <summary>
        /// Read only property which holds the entity's previous position.
        /// </summary>
        public Vector2 PreviousPosition { get; private set; }

        /// <summary>
        /// Flag indicating whether or not this position component has been updated this tick.
        /// </summary>
        public bool Updated { get; set; }

        /// <summary>
        /// Gets or sets the rotation of an entity in radians.
        /// </summary>
        public float Rotation
        {
            get { return _rotation; }
            set
            {
                _rotation = MathHelper.WrapAngle(value);
            }
        }

        #endregion
    }
}
