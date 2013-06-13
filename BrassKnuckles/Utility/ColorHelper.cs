using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.Utility
{
    public struct GradientStop
    {
        #region Properties

        public Color Color { get; set; }

        private float _position;
        public float Position
        {
            get { return _position; }
            set { _position = MathHelper.Clamp(value, 0.0f, 1.0f); }
        }

        #endregion
    }

    /// <summary>
    /// Provides helper methods for creating and modifying color values.
    /// </summary>
    public static class ColorHelper
    {
        #region Public methods

        /// <summary>
        /// Creates a new <see cref="Microsoft.Xna.Framework.Color"/> instance from the provided string.
        /// </summary>
        /// <param name="text">String describing color. Should be a comma-separated list of color values in RGB or RGBA format.
        /// For example "255,255,0" for yellow or "128,128,128,128" for 50% transparent gray.</param>
        /// <returns>A <see cref="Microsoft.Xna.Framework.Color"/> instance of the specified color.</returns>
        public static Color FromText(string text)
        {
            string[] colorTextComponents = text.Split(',');
            int componentCount = colorTextComponents.Length;
            if ((componentCount < 3) || (componentCount > 4))
            {
                throw new ArgumentException(string.Format("Invalid color string: {0}", text));
            }

            int red = int.Parse(colorTextComponents[0]);
            int green = int.Parse(colorTextComponents[1]);
            int blue = int.Parse(colorTextComponents[2]);

            int alpha = 255;
            if (componentCount == 4)
            {
                alpha = int.Parse(colorTextComponents[3]);
            }

            return new Color(red, green, blue, alpha);
        }

        #endregion
    }
}
