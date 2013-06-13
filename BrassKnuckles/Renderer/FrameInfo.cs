using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrassKnuckles.Renderer
{
    /// <summary>
    /// Describes a single frame of animation.
    /// </summary>
    public class FrameInfo
    {
        #region Properties

        /// <summary>
        /// Gets or sets the area of the texture to draw.
        /// </summary>
        public Rectangle Source { get; set; }

        /// <summary>
        /// Gets or sets the scale factor used when drawing the image.
        /// </summary>
        public Vector2 Scale { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the sprite in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the color used to tint the image. The default value of
        /// <see cref="Color.White"/> draws the image without any color change.
        /// </summary>
        public Color Tint { get; set; }

        /// <summary>
        /// Gets or sets additional effects to apply to the image when drawing.
        /// </summary>
        public SpriteEffects Effects { get; set; }

        /// <summary>
        /// Gets or sets a value in seconds that determines how long the frame is displayed.
        /// </summary>
        public float FrameLength { get; set; }

        #endregion

        #region Constructors

        public FrameInfo()
        {
            Scale = Vector2.One;
            Effects = SpriteEffects.None;
            Tint = Color.White;
            Rotation = 0.0f;
        }

        #endregion
    }
}
