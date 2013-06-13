using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities in a group at a set interval.
    /// </summary>
    public abstract class IntervalGroupSystem : GroupSystem
    {
        #region Private fields

        private float _elapsedTime;
        private float _interval;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new IntervalGroupSystem instance to process entities in the group specified
        /// at a set interval.
        /// </summary>
        /// <param name="interval">Interval in seconds at which entities will be processed.</param>
        /// <param name="group">Entity group to be processed.</param>
        public IntervalGroupSystem(float interval, string group)
            : base(group)
        {
            _interval = interval;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Checks if the system is ready to process entities.
        /// </summary>
        /// <returns>True if the system is ready to process entities, false if not.</returns>
        protected override bool CheckProcessing()
        {
            _elapsedTime += World.ElapsedTime;
            if (_elapsedTime >= _interval)
            {
                _elapsedTime -= _interval;

                return IsEnabled;
            }

            return false;
        }

        #endregion
    }
}
