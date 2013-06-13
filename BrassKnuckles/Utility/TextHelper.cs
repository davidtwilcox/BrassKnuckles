using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BrassKnuckles.Utility
{
    /// <summary>
    /// Provides helper methods for working with text.
    /// </summary>
    public static class TextHelper
    {
        #region Public methods

        /// <summary>
        /// Wraps text using the provided font to fit the width specified.
        /// </summary>
        /// <param name="text">Text to wrap.</param>
        /// <param name="size">Width in pixels of the area in which the text will fit.</param>
        /// <param name="font">Font to use for measuring the text.</param>
        /// <returns>An array of strings, with each element representing a line of text.</returns>
        public static string[] WrapText(string text, int width, SpriteFont font)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return new string[] { string.Empty };
            }

            List<string> output = new List<string>();
            string line = string.Empty;
            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                string appended = line + word;
                if (font.MeasureString(appended).X > width)
                {
                    output.Add(line);
                    line = string.Empty;
                }

                line += word + ' ';
            }
            output.Add(line);

            return output.ToArray<string>();
        }

        #endregion
    }
}
