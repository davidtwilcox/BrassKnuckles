using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.Utility;

namespace BrassKnuckles.Geometry
{
    /// <summary>
    /// Basic structure for circles.
    /// </summary>
    [Serializable]
    public struct Circle
    {
        #region Properties

        private Vector2 _center;
        /// <summary>
        /// Coordinates of circle center point.
        /// </summary>
        public Vector2 Center
        {
            get { return _center; }
            set { _center = value; }
        }

        private float _radius;
        /// <summary>
        /// Radius of circle.
        /// </summary>
        /// <remarks>Must be 0.0 or greater.</remarks>
        public float Radius
        {
            get { return _radius; }
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentOutOfRangeException("Radius");
                }

                _radius = value;
            }
        }

        /// <summary>
        /// X coordinate of circle center.
        /// </summary>
        public float X
        {
            get { return Center.X; }
        }

        /// <summary>
        /// Y coordinate of circle center.
        /// </summary>
        public float Y
        {
            get { return Center.Y; }
        }

        /// <summary>
        /// Coordinates of top of circle.
        /// </summary>
        public Vector2 Top
        {
            get { return new Vector2(Center.X, Center.Y - Radius); }
        }

        /// <summary>
        /// Coordinates of bottom of circle.
        /// </summary>
        public Vector2 Bottom
        {
            get { return new Vector2(Center.X, Center.Y + Radius); }
        }

        /// <summary>
        /// Coordinates of left side of circle.
        /// </summary>
        public Vector2 Left
        {
            get { return new Vector2(Center.X - Radius, Center.Y); }
        }

        /// <summary>
        /// Coordinates of right side of circle.
        /// </summary>
        public Vector2 Right
        {
            get { return new Vector2(Center.X + Radius, Center.Y); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Circle instance with the specified center and radius.
        /// </summary>
        /// <param name="center">Coordinates of center of circle.</param>
        /// <param name="radius">Radius of circle. Must be 0.0 or greater.</param>
        public Circle(Vector2 center, float radius)
        {
            _center = center;

            if (radius < 0.0f)
            {
                throw new ArgumentOutOfRangeException("radius");
            }
            _radius = radius;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if a point is inside the circle.
        /// </summary>
        /// <param name="point">Point to check.</param>
        /// <returns>True if the point is inside the circle, false if not.</returns>
        public bool Intersects(Point point)
        {
            return Intersects(new Vector2(point.X, point.Y));
        }

        /// <summary>
        /// Determines if a vector is inside the circle.
        /// </summary>
        /// <param name="vector">Vector to check.</param>
        /// <returns>True if the vector is inside the circle, false if not.</returns>
        public bool Intersects(Vector2 vector)
        {
            float distance = Center.Distance(vector);

            return distance <= Radius;
        }

        /// <summary>
        /// Determines if another circle intersects this circle.
        /// </summary>
        /// <param name="circle">Circle to check.</param>
        /// <returns>True if the other circle intersects this circle, false if not.</returns>
        public bool Intersects(Circle circle)
        {
            float distance = Center.Distance(circle.Center);

            return distance <= (Radius + circle.Radius);
        }

        /// <summary>
        /// Determines if a rectangle intersects this circle.
        /// </summary>
        /// <param name="rectangle">Rectangle to check.</param>
        /// <returns>True if the rectangle intersects the circle, false if not.</returns>
        public bool Intersects(Rectangle rectangle)
        {
            return MathExtension.CircleRectangleIntersection(this, rectangle);
        }

        #endregion
    }
}
