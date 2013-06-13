using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using BrassKnuckles.Geometry;

namespace BrassKnuckles.Utility
{
    /// <summary>
    /// Additional math functions beyond those found in Math and MathHelper.
    /// </summary>
    public static class MathExtension
    {
        #region Methods

        /// <summary>
        /// Divides two integers, rounding up the result to the next highest integer.
        /// </summary>
        /// <param name="x">Numerator.</param>
        /// <param name="y">Denominator.</param>
        /// <returns>x divided by y, rounded up.</returns>
        /// <example>
        /// int result1 = 100 / 33;
        /// // result1 = 3
        /// int result2 = MathEx.DivideUp(100, 33);
        /// // result2 = 4
        /// </example>
        public static int DivideUp(int x, int y)
        {
            return (x + y - 1) / y;
        }

        /// <summary>
        /// Determines if a circle and rectangle intersect one another.
        /// </summary>
        /// <param name="circle">Circle to check.</param>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <returns>True if the circle and rectangle intersect, false if not.</returns>
        public static bool CircleRectangleIntersection(Circle circle, Rectangle rectangle)
        {
            float halfWidth = rectangle.Width / 2.0f;
            float halfHeight = rectangle.Height / 2.0f;

            float distanceX = Math.Abs(circle.Center.X - rectangle.X - halfWidth);
            float distanceY = Math.Abs(circle.Center.Y - rectangle.Y - halfHeight);

            if ((distanceX > (halfWidth + circle.Radius)) || (distanceY > (halfHeight + circle.Radius)))
            {
                return false;
            }

            if ((distanceX <= halfWidth) || (distanceY <= halfHeight))
            {
                return true;
            }

            float cornerDistance = ((distanceX - halfWidth) * (distanceX - halfWidth)) + ((distanceY - halfHeight) * (distanceY - halfHeight));

            return cornerDistance <= (circle.Radius * circle.Radius);
        }

        /// <summary>
        /// Determines all of the integer points on a line between two vectors.
        /// </summary>
        /// <param name="start">Starting point of line.</param>
        /// <param name="end">Ending point of line.</param>
        /// <returns>An array of Point instances containing the consecutive integer points along the line.</returns>
        public static Point[] PointsOnLine(Vector2 start, Vector2 end)
        {
            List<Point> points = new List<Point>();

            int x1 = (int)Math.Floor(start.X);
            int y1 = (int)Math.Floor(start.Y);
            int x2 = (int)Math.Floor(end.X);
            int y2 = (int)Math.Floor(end.Y);

            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int err = dx - dy;
            int err2 = 2 * err;

            int sx = -1;
            if (x1 < x2)
            {
                sx = 1;
            }
            int sy = -1;
            if (y1 < y2)
            {
                sy = 1;
            }

            int x = x1;
            int y = y1;
            while ((x != x2) || (y != y2))
            {
                points.Add(new Point(x, y));
                err2 = 2 * err;
                if (err2 > -dy)
                {
                    err -= dy;
                    x += sx;
                }
                if (err2 < dx)
                {
                    err += dx;
                    y += sy;
                }
            }

            return points.ToArray<Point>();
        }

        #endregion
    }
}
