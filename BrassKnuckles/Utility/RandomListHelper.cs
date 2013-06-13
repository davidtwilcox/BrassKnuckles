using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.Utility
{
    /// <summary>
    /// Provides methods to randomize lists and access random elements from a list.
    /// </summary>
    public static class RandomListHelper
    {
        #region Public methods

        /// <summary>
        /// Returns a random element from an IList instance.
        /// </summary>
        /// <typeparam name="T">Type of elements contained within the list.</typeparam>
        /// <param name="list">IList instance to use.</param>
        /// <returns>A random element from the list, or the default value of T if the list null or empty.</returns>
        public static T RandomElement<T>(this IList<T> list)
        {
            if ((list == null) || (list.Count == 0))
            {
                return default(T);
            }

            Random random = new Random();

            return list[random.Next(0, list.Count)];
        }

        /// <summary>
        /// Randomly shuffles the elements of an IList instance.
        /// </summary>
        /// <typeparam name="T">Type of elements contained within the list.</typeparam>
        /// <param name="list">IList instance to use.</param>
        public static void Shuffle<T>(this IList<T> list)
        {
            if ((list == null) || (list.Count < 2))
            {
                return;
            }

            Random random = new Random();

            int n = list.Count - 1;
            while (n > 1)
            {
                int index = random.Next(0, n);
                T temp = list[n];
                list[n] = list[index];
                list[index] = temp;
                --n;
            }
        }

        #endregion
    }
}
