using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.EntityFramework.Systems;

namespace BrassKnuckles.EntityFramework.Managers
{
    /// <summary>
    /// Manages the allocation of system bit flags to systems.
    /// </summary>
    public static class SystemBitManager
    {
        #region Private fields

        private static int _position = 0;
        private static Dictionary<IEntitySystem, long> _systemBits = new Dictionary<IEntitySystem, long>();

        #endregion

        #region Public methods

        /// <summary>
        /// Gets a system bit for the specified <see cref="IEntitySystem"/>.
        /// </summary>
        /// <param name="entitySystem">The <see cref="IEntitySystem"/> to use.</param>
        /// <returns>Either the system bit flag assigned to the provided <see cref="IEntitySystem"/>, or a new
        /// system bit flag.</returns>
        public static long GetBitFor(IEntitySystem entitySystem)
        {
            long bit;
            if (!_systemBits.TryGetValue(entitySystem, out bit))
            {
                bit = 1L << _position;
                ++_position;
                _systemBits.Add(entitySystem, bit);
            }

            return bit;
        }

        #endregion
    }
}
