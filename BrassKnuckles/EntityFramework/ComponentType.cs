using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework
{
    /// <summary>
    /// Stores type data for IComponent implementations.
    /// </summary>
    public sealed class ComponentType
    {
        #region Private fields

        private static int _nextId;
        private static long _nextBit = 1;

        #endregion

        #region Properties

        /// <summary>
        /// IComponent type ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// IComponent type bit flag.
        /// </summary>
        public long Bit { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ComponentType instance.
        /// </summary>
        public ComponentType()
        {
            Id = _nextId++;
            Bit = _nextBit;
            _nextBit = _nextBit << 1;
        }

        #endregion
    }
}
