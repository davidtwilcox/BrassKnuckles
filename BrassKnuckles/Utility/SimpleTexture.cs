using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.Utility
{
    /// <summary>
    /// Helper for creating textures at runtime.
    /// </summary>
    public static class SimpleTexture
    {
        #region Public methods

        /// <summary>
        /// Creates a 1x1 pixel texture of the specified color.
        /// </summary>
        /// <param name="device"><see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> instance to use.</param>
        /// <param name="color">Fill color for the texture.</param>
        /// <returns>A 1x1 pixel texture.</returns>
        public static Texture2D CreatePixelTexture(GraphicsDevice device, Color color)
        {
            Texture2D pixel = new Texture2D(device, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData<Color>(new Color[] { color });

            return pixel;
        }

        /// <summary>
        /// Creates a 1 pixel wide vertical gradient texture using the provided <see cref="GradientStop"/> array.
        /// </summary>
        /// <param name="device"><see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> instance to use.</param>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/> instances to use when generating the gradient.</param>
        /// <param name="height">Height of the generated texture in pixels.</param>
        /// <returns>A texture 1 pixel wide and the height specified.</returns>
        public static Texture2D CreateVerticalGradient(GraphicsDevice device, Gradient gradient, int height)
        {
            Texture2D texture = new Texture2D(device, 1, height, false, SurfaceFormat.Color);
            Color[] colors = gradient.GetColors(height);
            texture.SetData<Color>(colors);

            return texture;
        }

        /// <summary>
        /// Creates a 1 pixel high horizontal gradient texture using the provided <see cref="GradientStop"/> array.
        /// </summary>
        /// <param name="device"><see cref="Microsoft.Xna.Framework.Graphics.GraphicsDevice"/> instance to use.</param>
        /// <param name="gradientStops">Array of <see cref="GradientStop"/> instances to use when generating the gradient.</param>
        /// <param name="width">Width of the generated texture in pixels.</param>
        /// <returns>A texture with the width specified and 1 pixel high.</returns>
        public static Texture2D CreateHorizontalGradient(GraphicsDevice device, Gradient gradient, int width)
        {
            Texture2D texture = new Texture2D(device, width, 1, false, SurfaceFormat.Color);
            Color[] colors = gradient.GetColors(width);
            texture.SetData<Color>(colors);

            return texture;
        }

        #endregion
    }
}
