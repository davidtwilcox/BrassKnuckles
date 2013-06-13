using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.EntityFramework.Systems
{
    /// <summary>
    /// System for processing entities after a delay.
    /// </summary>
    public abstract class DelayedEntitySystem : EntitySystem
    {
        #region Private fields

        private float _elapsedTime;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the delay in seconds before processing entities. 
        /// </summary>
        public float Delay { get; protected set; }

        /// <summary>
        /// Gets the remaining time in seconds before entities are processed.
        /// </summary>
        public float RemainingTimeUntilProcessing
        {
            get
            {
                if (IsRunning)
                {
                    return (Delay - _elapsedTime);
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a flag indicating if the system is running or not.
        /// </summary>
        public bool IsRunning { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new DelayedEntitySystem instance associated with the provided types.
        /// </summary>
        /// <param name="types">Types to associate with.</param>
        public DelayedEntitySystem(params Type[] types)
            : base(types)
        {
        }

        /// <summary>
        /// Creates a new DelayedEntitySystem instance associated with the merged list of
        /// required and other types.
        /// </summary>
        /// <param name="requiredType">Type required by system.</param>
        /// <param name="otherTypes">Other types associated with system.</param>
        public DelayedEntitySystem(Type requiredType, params Type[] otherTypes)
            : this(GetMergedTypes(requiredType, otherTypes))
        {
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Processes all <see cref="Entity"/> instances passed.
        /// </summary>
        /// <param name="entities">Dictionary containing all <see cref="Entity"/> instances to be processed. The dictionary
        /// key is the entity ID.</param>
        protected override void ProcessEntities(Dictionary<int, Entity> entities)
        {
            ProcessEntities(entities, _elapsedTime);
            Stop();
        }

        /// <summary>
        /// Checks whether or not the system should process the entities.
        /// </summary>
        /// <returns>True if the system is ready to process entities, or false if not.</returns>
        protected override bool CheckProcessing()
        {
            if (IsRunning)
            {
                _elapsedTime += World.ElapsedTime;
                if (_elapsedTime >= Delay)
                {
                    return IsEnabled;
                }
            }

            return false;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Processes an <see cref="Entity"/> using the provided elapsed time.
        /// </summary>
        /// <param name="entity">The <see cref="Entity"/> instance to process.</param>
        /// <param name="elapsedTime">Elapsed time in seconds since last processed.</param>
        public abstract void Process(Entity entity, float elapsedTime);

        /// <summary>
        /// Processes all <see cref="Entity"/> instances passed, passing the provided elapsed time value.
        /// </summary>
        /// <param name="entities">Dictionary containing all <see cref="Entity"/> instances to be processed. The dictionary
        /// key is the entity ID.</param>
        /// <param name="elapsedTime">Elapsed time in seconds since last time entities were processed.</param>
        public virtual void ProcessEntities(Dictionary<int, Entity> entities, float elapsedTime)
        {
            foreach (Entity entity in entities.Values)
            {
                Process(entity, elapsedTime);
            }
        }

        /// <summary>
        /// Starts the countdown to delayed processing of entities.
        /// </summary>
        /// <param name="delay">Time in seconds to wait before processing entities.</param>
        public void StartDelayedRun(float delay)
        {
            Delay = delay;
            _elapsedTime = 0;
            IsRunning = true;
        }

        /// <summary>
        /// Stops the countdown to delayed processing of entities.
        /// </summary>
        public void Stop()
        {
            IsRunning = false;
            _elapsedTime = 0;
        }

        #endregion
    }
}
