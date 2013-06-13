using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities in parallel at a constant interval.
    /// </summary>
    public abstract class ParallelIntervalEntitySystem : IntervalEntitySystem
    {
        #region Constructors

        /// <summary>
        /// Creates a new ParallelIntervalEntitySystem set to process entities at the specified interval and associated
        /// with the types provided.
        /// </summary>
        /// <param name="interval">Interval in seconds at which entities will be processed.</param>
        /// <param name="types">Types to associate with.</param>
        public ParallelIntervalEntitySystem(float interval, params Type[] types)
            : base(interval, types)
        {
        }

        /// <summary>
        /// Creates a new IntervalEntitySystem set to process entities at the specified interval and associated
        /// with the merged list of required and other types provided.
        /// </summary>
        /// <param name="interval">Interval in seconds at which entities will be processed.</param>
        /// <param name="requiredType">Required type to associate with.</param>
        /// <param name="otherTypes">Other types to associate with.</param>
        public ParallelIntervalEntitySystem(float interval, Type requiredType, params Type[] otherTypes)
            : base(interval, requiredType, otherTypes)
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
            Parallel.ForEach<Entity>(entities.Values, Process);
        }

        #endregion
    }
}
