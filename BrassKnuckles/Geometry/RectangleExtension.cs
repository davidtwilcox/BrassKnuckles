using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.Geometry;
using BrassKnuckles.Utility;

namespace BrassKnuckles.Geometry
{
    /// <summary>
    /// Provides additional methods for Rectangle instances.
    /// </summary>
    public static class RectangleExtension
    {
        #region Public methods

        /// <summary>
        /// Determines if a point is inside a rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <param name="point">Point to check.</param>
        /// <returns>True if the point is inside the rectangle, false if not.</returns>
        public static bool Intersects(this Rectangle rectangle, Point point)
        {
            return Intersects(rectangle, new Vector2(point.X, point.Y));
        }

        /// <summary>
        /// Determines if a vector is inside a rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <param name="vector">Vector to check.</param>
        /// <returns>True if the vector is inside the rectangle, false if not.</returns>
        public static bool Intersects(this Rectangle rectangle, Vector2 vector)
        {
            return (vector.X >= rectangle.Left) && (vector.X <= rectangle.Right) &&
                   (vector.Y >= rectangle.Top) && (vector.Y <= rectangle.Bottom);
        }

        /// <summary>
        /// Determines if a circle intersects a rectangle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <param name="circle">Circle to check.</param>
        /// <returns>True if the circle intersects the rectangle, false if not.</returns>
        public static bool Intersects(this Rectangle rectangle, Circle circle)
        {
            return MathExtension.CircleRectangleIntersection(circle, rectangle);
        }

        #endregion
    }
}
