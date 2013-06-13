using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrassKnuckles.Renderer
{
    /// <summary>
    /// Determines how an <see cref="ImageComponent"/> Position property is interpreted.
    /// </summary>
    public enum PositionMode
    {
        /// <summary>
        /// <see cref="ImageComponent"/> position is based solely on its Position property.
        /// </summary>
        Independent,
        /// <summary>
        /// <see cref="ImageComponent"/> position is the sum of the position specified
        /// by the <see cref="PositionComponent"/> of the entity and the Position property.
        /// </summary>
        External
    }

    /// <summary>
    /// Determines how an <see cref="ImageComponent"/> Rotation property is interpreted.
    /// </summary>
    public enum RotationMode
    {
        /// <summary>
        /// <see cref="ImageComponent"/> rotation is based solely on its Rotation property.
        /// </summary>
        Independent,
        /// <summary>
        /// <see cref="ImageComponent"/> rotation is the sum of the rotation specified
        /// by the <see cref="FacingComponent"/> of the entity and the Rotation property.
        /// </summary>
        External
    }

    /// <summary>
    /// Used by the <see cref="RenderSystem"/> to draw a texture.
    /// </summary>
    public class ImageComponent : IComponent
    {
        #region Properties

        /// <summary>
        /// Gets or sets the texture to draw.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Gets or sets the area of the texture to draw.
        /// </summary>
        public Rectangle Source { get; set; }

        /// <summary>
        /// Gets or sets the image position.
        /// </summary>
        /// <remarks>How this value is interpreted is determined by the
        /// <see cref="PositionMode"/> property.</remarks>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Gets or sets the mode that determines how the <see cref="Position"/> property
        /// is interpreted.
        /// </summary>
        public PositionMode PositionMode { get; set; }

        /// <summary>
        /// Gets or sets the image origin.
        /// </summary>
        public Vector2 Origin { get; set; }

        private Vector2 _scale = Vector2.One;
        /// <summary>
        /// Gets or sets the scale factor used when drawing the image.
        /// </summary>
        public Vector2 Scale
        {
            get { return _scale; }
            set
            {
                _scale.X = Math.Max(value.X, 0.0f);
                _scale.Y = Math.Max(value.Y, 0.0f);
            }
        }

        /// <summary>
        /// Gets or sets the rotation of the sprite in radians.
        /// </summary>
        public float Rotation { get; set; }

        /// <summary>
        /// Gets or sets the mode used for setting the sprite rotation.
        /// </summary>
        public RotationMode RotationMode { get; set; }

        /// <summary>
        /// Gets or sets the color used to tint the image. The default value of
        /// <see cref="Color.White"/> draws the image without any color change.
        /// </summary>
        public Color Tint { get; set; }

        /// <summary>
        /// Gets or sets additional effects to apply to the image when drawing.
        /// </summary>
        public SpriteEffects Effects { get; set; }

        private float _layer = 0.0f;
        /// <summary>
        /// Gets or sets the drawing layer at which the image will be drawn, with
        /// 0.0 at the front and 1.0 at the back.
        /// </summary>
        public float Layer
        {
            get { return _layer; }
            set { _layer = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        /// <summary>
        /// Gets or sets a flag indicating whether or not the image is visible.
        /// </summary>
        public bool IsVisible { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ImageComponent instance with default values.
        /// </summary>
        public ImageComponent()
        {
            PositionMode = PositionMode.Independent;
            Origin = Vector2.Zero;
            Tint = Color.White;
            IsVisible = true;
        }

        #endregion
    }
}
