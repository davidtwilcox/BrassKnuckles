using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities in parallel.
    /// </summary>
    public abstract class ParallelEntitySystem : EntitySystem
    {
        #region Private fields

        private TaskFactory _taskFactory;
        float _simultaneous;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ParallelEntityProcessingSystem instance using the merged list of
        /// required and other types.
        /// </summary>
        /// <param name="requiredType">Required type.</param>
        /// <param name="otherTypes">Other types to associate with.</param>
        public ParallelEntitySystem(Type requiredType, params Type[] otherTypes)
            : base(GetMergedTypes(requiredType, otherTypes))
        {
            _taskFactory = new TaskFactory(TaskScheduler.Default);
            _simultaneous = Environment.ProcessorCount * 2.0f;
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all entities provided in parallel.
        /// </summary>
        /// <param name="entities">Collection of entities to process.</param>
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            Parallel.ForEach<Entity>(entities.Values, Process);
        }

        #endregion
    }
}
