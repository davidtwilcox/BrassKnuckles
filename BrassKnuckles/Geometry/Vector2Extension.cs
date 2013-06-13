using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using BrassKnuckles.Geometry;

namespace BrassKnuckles.Geometry
{
    /// <summary>
    /// Directions using a circle divided into quadrants (fourths), such that up is the quadrant numbered 1 spanning 7pi/4 radians (315 degrees) to pi/4 radians (45 degrees), and each quadrant number
    /// increases by 1 in a clockwise direction and spans pi/2 radians (90 degrees).
    /// </summary>
    public enum QuadrantDirection
    {
        #region Directions
        /// <summary>
        /// No direction.
        /// </summary>
        None = 0,
        /// <summary>
        /// Up
        /// </summary>
        Up = 1,
        /// <summary>
        /// Right
        /// </summary>
        Right = 2,
        /// <summary>
        /// Down
        /// </summary>
        Down = 3,
        /// <summary>
        /// Left
        /// </summary>
        Left = 4
        #endregion
    }

    /// <summary>
    /// Directions using a circle divided into octants (eighths), such that up is the octant numbered 1 spanning 15pi/8 radians (337.5 degrees) to pi/8 radians (22.5 degrees), and each octant number
    /// increases by 1 in a clockwise direction and spans pi/4 radians (45 degrees).
    /// </summary>
    public enum OctantDirection
    {
        #region Directions
        /// <summary>
        /// No direction
        /// </summary>
        None = 0,
        /// <summary>
        /// Up
        /// </summary>
        Up = 1,
        /// <summary>
        /// Up and right
        /// </summary>
        UpRight = 2,
        /// <summary>
        /// Right
        /// </summary>
        Right = 3,
        /// <summary>
        /// Down and right
        /// </summary>
        DownRight = 4,
        /// <summary>
        /// Down
        /// </summary>
        Down = 5,
        /// <summary>
        /// Down and left
        /// </summary>
        DownLeft = 6,
        /// <summary>
        /// Left
        /// </summary>
        Left = 7,
        /// <summary>
        /// Up and left
        /// </summary>
        UpLeft = 8
        #endregion
    }

    /// <summary>
    /// Provides helper methods for working with OctantDirection and QuadrantDirection values.
    /// </summary>
    public static class DirectionHelper
    {
        #region Constants

        private static readonly float _inverseSqrt2 = 1.0f / (float)Math.Sqrt(2.0d);

        #endregion

        #region Public methods

        /// <summary>
        /// Converts a QuadrantDirection value to a normalized Vector2.
        /// </summary>
        /// <param name="direction">QuadrantDirection value to convert.</param>
        /// <returns>A normalized Vector2 instance.</returns>
        public static Vector2 DirectionToVector(QuadrantDirection direction)
        {
            switch (direction)
            {
                case QuadrantDirection.Up:
                    return new Vector2(0.0f, -1.0f);
                case QuadrantDirection.Down:
                    return new Vector2(0.0f, 1.0f);
                case QuadrantDirection.Left:
                    return new Vector2(-1.0f, 0.0f);
                case QuadrantDirection.Right:
                    return new Vector2(1.0f, 0.0f);
                default:
                    return Vector2.Zero;
            }
        }

        /// <summary>
        /// Converts a OctantDirection value to a normalized Vector2.
        /// </summary>
        /// <param name="direction">OctantDirection value to convert.</param>
        /// <returns>A normalized Vector2 instance.</returns>
        public static Vector2 DirectionToVector(OctantDirection direction)
        {
            switch (direction)
            {
                case OctantDirection.Up:
                    return new Vector2(0.0f, -1.0f);
                case OctantDirection.UpLeft:
                    return new Vector2(-_inverseSqrt2, -_inverseSqrt2);
                case OctantDirection.UpRight:
                    return new Vector2(_inverseSqrt2, -_inverseSqrt2);
                case OctantDirection.Down:
                    return new Vector2(0.0f, 1.0f);
                case OctantDirection.DownLeft:
                    return new Vector2(-_inverseSqrt2, _inverseSqrt2);
                case OctantDirection.DownRight:
                    return new Vector2(_inverseSqrt2, _inverseSqrt2);
                case OctantDirection.Left:
                    return new Vector2(-1.0f, 0.0f);
                case OctantDirection.Right:
                    return new Vector2(1.0f, 0.0f);
                default:
                    return Vector2.Zero;
            }
        }

        #endregion
    }

    /// <summary>
    /// Vector2 extension class.
    /// </summary>
    /// <remarks>All angle-related methods use the following convention for angle directions: 0 radians (0 degrees) is up, with angles increasing in a clockwise direction, 
    /// so that pi/2 radians (90 degrees) is right, pi radians (180 degrees) is down, and 3pi/2 radians (270 degrees) is left.</remarks>
    public static class Vector2Extension
    {
        #region Public methods

        /// <summary>
        /// Converts an angle in radians to a Vector2.
        /// </summary>
        /// <param name="angle">Angle, in radians.</param>
        /// <returns>Vector2 instance based on the angle.</returns>
        public static Vector2 RadiansToVector(this float angle)
        {
            return new Vector2((float)Math.Sin(angle), -(float)Math.Cos(angle));
        }

        /// <summary>
        /// Converts an angle in degrees to a Vector2.
        /// </summary>
        /// <param name="angle">Angle, in degrees.</param>
        /// <returns>Vector2 instance based on the angle.</returns>
        public static Vector2 DegreesToVector(this float angle)
        {
            if (angle > 180.0f)
            {
                angle -= 360.0f;
            }
            float angleRadians = MathHelper.ToRadians(angle);

            return new Vector2((float)Math.Sin(angleRadians), -(float)Math.Cos(angleRadians));
        }

        /// <summary>
        /// Converts a Vector2 instance to an angle in radians.
        /// </summary>
        /// <param name="vector">Vector2 instance to convert.</param>
        /// <returns>Angle, in radians.</returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.X, -vector.Y);
        }

        /// <summary>
        /// Converts a Vector2 instance to an angle in degrees.
        /// </summary>
        /// <param name="vector">Vector2 instance to convert.</param>
        /// <returns>Angle, in degrees.</returns>
        public static float ToAngleDegrees(this Vector2 vector)
        {
            return MathHelper.ToDegrees((float)Math.Atan2(vector.X, -vector.Y));
        }

        /// <summary>
        /// Converts a Vector2 instance to an OctantDirection.
        /// </summary>
        /// <param name="vector">Vector2 instance to convert.</param>
        /// <returns>OctantDirection corresponding to vector direction.</returns>
        public static OctantDirection ToOctantDirection(this Vector2 vector)
        {
            if (vector == Vector2.Zero)
            {
                return OctantDirection.None;
            }

            float angle = vector.ToAngleDegrees();
            if (angle < 0.0f)
            {
                angle += 360.0f;
            }

            if ((angle >= 337.5f) || (angle < 22.5f))
            {
                return OctantDirection.Up;
            }
            else if (angle >= 292.5f)
            {
                return OctantDirection.UpLeft;
            }
            else if (angle >= 247.5f)
            {
                return OctantDirection.Left;
            }
            else if (angle >= 202.5f)
            {
                return OctantDirection.DownLeft;
            }
            else if (angle >= 157.5f)
            {
                return OctantDirection.Down;
            }
            else if (angle >= 112.5f)
            {
                return OctantDirection.DownRight;
            }
            else if (angle >= 67.5f)
            {
                return OctantDirection.Right;
            }
            else if (angle >= 22.5f)
            {
                return OctantDirection.UpRight;
            }

            return OctantDirection.None;
        }

        /// <summary>
        /// Converts a Vector2 instance to a QuadrantDirection.
        /// </summary>
        /// <param name="vector">Vector2 instance to convert.</param>
        /// <returns>QuadrantDirection corresponding to vector direction.</returns>
        public static QuadrantDirection ToQuadrantDirection(this Vector2 vector)
        {
            if (vector == Vector2.Zero)
            {
                return QuadrantDirection.None;
            }

            float angle = vector.ToAngleDegrees();
            if (angle < 0.0f)
            {
                angle += 360.0f;
            }

            if ((angle >= 315.0f) || (angle < 45.0f))
            {
                return QuadrantDirection.Up;
            }
            else if (angle >= 225.0f)
            {
                return QuadrantDirection.Left;
            }
            else if (angle >= 135.0f)
            {
                return QuadrantDirection.Down;
            }
            else if (angle >= 45.0f)
            {
                return QuadrantDirection.Right;
            }

            return QuadrantDirection.None;
        }

        /// <summary>
        /// Determines if a Vector2 position is inside or on the edge of a rectangle.
        /// </summary>
        /// <param name="vector">Vector2 instance to check.</param>
        /// <param name="rectangle">Rectangle instance to check.</param>
        /// <returns>True if the Vector2 position is completely inside or along the edge of the target rectangle, false if not.</returns>
        public static bool Intersects(this Vector2 vector, Rectangle rectangle)
        {
            return rectangle.Intersects(vector);
        }

        /// <summary>
        /// Determines if Vector2 position is inside or on the edge of a circle.
        /// </summary>
        /// <param name="vector">Vector2 instance to check.</param>
        /// <param name="circle">Circle instance to check.</param>
        /// <returns>True if the Vector2 position is completely inside or along the edge of the target circle, false if not.</returns>
        public static bool Intersects(this Vector2 vector, Circle circle)
        {
            return circle.Intersects(vector);
        }

        /// <summary>
        /// Calculates the Euclidean distance between two Vector2 positions.
        /// </summary>
        /// <param name="vector">First vector.</param>
        /// <param name="other">Second vector.</param>
        /// <returns>Euclidean distance between the two Vector2 positions.</returns>
        public static float Distance(this Vector2 vector, Vector2 other)
        {
            return (float)Math.Sqrt(((other.X - vector.X) * (other.X - vector.X)) + ((other.Y - vector.Y) * (other.Y - vector.Y)));
        }

        /// <summary>
        /// Calculates the Manhattan distance between two Vector2 positions.
        /// </summary>
        /// <param name="vector">First vector.</param>
        /// <param name="other">Second vector.</param>
        /// <returns>Manhattan distance between the two Vector2 positions.</returns>
        public static float ManhattanDistance(this Vector2 vector, Vector2 other)
        {
            return Math.Abs(vector.X - other.X) + Math.Abs(vector.Y - other.Y);
        }

        #endregion
    }
}
