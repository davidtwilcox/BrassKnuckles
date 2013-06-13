using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.Utility
{
    /// <summary>
    /// Defines the orientation of a gradient.
    /// </summary>
    public enum GradientOrientation
    {
        Vertical,
        Horizontal
    }

    /// <summary>
    /// Defines one-dimensional color gradients.
    /// </summary>
    public class Gradient
    {
        #region Properties

        public GradientStop[] Stops { get; protected set; }

        public GradientOrientation Orientation { get; set; }

        protected List<GradientStop> StopList { get; set; }

        #endregion

        #region Constructors

        public Gradient()
        {
            StopList = new List<GradientStop>();
        }

        public Gradient(IEnumerable<GradientStop> stops)
        {
            StopList = new List<GradientStop>(stops);
            Stops = StopList.ToArray();
        }

        #endregion

        #region Protected methods

        protected static GradientStop[] ColorToColor(Color start, Color end)
        {
            return new GradientStop[]
            {
                new GradientStop() { Color = start, Position = 0.0f },
                new GradientStop() { Color = end, Position = 1.0f }
            };
        }

        #endregion

        #region Public methods

        public void AddStop(GradientStop stop)
        {
            StopList.Add(stop);
            Stops = StopList.ToArray();
        }

        public void AddStops(IEnumerable<GradientStop> stops)
        {
            StopList.AddRange(stops);
            Stops = StopList.ToArray();
        }

        public void RemoveStop(int index)
        {
            StopList.RemoveAt(index);
            Stops = StopList.ToArray();
        }

        public void ClearStops()
        {
            StopList.Clear();
            Stops = StopList.ToArray();
        }

        /// <summary>
        /// Creates a linear gradient of the specified length.
        /// </summary>
        /// <param name="length">Length of the gradient.</param>
        /// <returns>An array of <see cref="Microsoft.Xna.Framework.Color"/> instances.</returns>
        public Color[] GetColors(int length)
        {
            if (length < 1)
            {
                throw new ArgumentOutOfRangeException("length", string.Format("Length must be greater than 0. Passed {0}", length));
            }

            List<GradientStop> sortedStops = (from gs in StopList
                                              orderby gs.Position ascending
                                              select gs).ToList();

            if (sortedStops[0].Position > 0.0f)
            {
                sortedStops.Insert(0, new GradientStop() { Color = sortedStops[0].Color, Position = 0.0f });
            }

            int lastIndex = sortedStops.Count - 1;
            if (sortedStops[lastIndex].Position < 1.0f)
            {
                sortedStops.Add(new GradientStop() { Color = sortedStops[lastIndex].Color, Position = 1.0f });
            }

            Func<float, float, int, float> GetDelta = (startValue, endValue, stops) =>
            {
                return (endValue - startValue) / (float)stops;
            };
            Color[] gradient = new Color[length];
            int totalItems = 0;
            for (int i = 0; i < sortedStops.Count - 1; ++i)
            {
                int items = (int)((sortedStops[i + 1].Position - sortedStops[i].Position) * (float)length);
                items = Math.Min(items, length - totalItems);

                Color startColor = sortedStops[i].Color;
                Color endColor = sortedStops[i + 1].Color;

                float factor = 1.0f / items;
                for (int j = 0; j < items; ++j)
                {
                    gradient[j + totalItems] = Color.Lerp(startColor, endColor, factor * j);
                }

                totalItems += items;
            }

            return gradient;
        }

        public static Gradient CreateBlackToWhite()
        {
            return CreateColorToColor(Color.Black, Color.White);
        }

        public static Gradient CreateWhiteToBlack()
        {
            return CreateColorToColor(Color.White, Color.Black);
        }

        public static Gradient CreateColorToColor(Color start, Color end)
        {
            return new Gradient(ColorToColor(start, end));
        }

        #endregion
    }
}
