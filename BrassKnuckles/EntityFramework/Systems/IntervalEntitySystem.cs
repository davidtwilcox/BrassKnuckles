using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities at a constant interval.
    /// </summary>
    public abstract class IntervalEntitySystem : EntitySystem
    {
        #region Private fields

        private float _elapsedTime;
        private float _interval;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the elapsed time in seconds since the system last processed entities.
        /// </summary>
        /// <remarks>Needed when processing entities so that they are processed based on the actual time
        /// elapsed since they were last processed, rather than the time since the last game tick as
        /// provided by the <see cref="EntityWorld"/> ElapsedTime property.</remarks>
        protected float ElapsedTimeSinceLastProcess { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new IntervalEntitySystem set to process entities at the specified interval and associated
        /// with the types provided.
        /// </summary>
        /// <param name="interval">Interval in seconds at which entities will be processed.</param>
        /// <param name="types">Types to associate with.</param>
        public IntervalEntitySystem(float interval, params Type[] types)
            : base(types)
        {
            _interval = interval;
        }

        /// <summary>
        /// Creates a new IntervalEntitySystem set to process entities at the specified interval and associated
        /// with the merged list of required and other types provided.
        /// </summary>
        /// <param name="interval">Interval in seconds at which entities will be processed.</param>
        /// <param name="requiredType">Required type to associate with.</param>
        /// <param name="otherTypes">Other types to associate with.</param>
        public IntervalEntitySystem(float interval, Type requiredType, params Type[] otherTypes)
            : this(interval, GetMergedTypes(requiredType, otherTypes))
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all entities passed.
        /// </summary>
        /// <param name="entities">The <see cref="Entity"/> instances to process.</param>
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            foreach (Entity entity in entities.Values)
            {
                Process(entity);
            }
        }

        /// <summary>
        /// Checks if the system should process entities.
        /// </summary>
        /// <returns>True if the system should process entities, false if not.</returns>
        protected override bool CheckProcessing()
        {
            _elapsedTime += World.ElapsedTime;
            if (_elapsedTime >= _interval)
            {
                ElapsedTimeSinceLastProcess = _elapsedTime;
                _elapsedTime -= _interval;

                return IsEnabled;
            }

            return false;
        }

        #endregion
    }
}
