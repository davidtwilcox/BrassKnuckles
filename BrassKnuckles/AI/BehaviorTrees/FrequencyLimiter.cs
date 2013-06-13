using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that wraps another task and prevents it executing more often 
    /// than a set frequency.
    /// </summary>
    public class FrequencyLimiter : Decorator
    {
        #region Private fields

        private float _timeSinceLastExecute;

        #endregion

        #region Properties

        private float _frequency;
        /// <summary>
        /// Gets or sets the maximum frequency at which the wrapped <see cref="ITask"/>
        /// instance can be executed. Specified in milliseconds.
        /// </summary>
        public float Frequency
        {
            get { return _frequency; }
            set { _frequency = Math.Max(0.0f, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new FrequencyLimited instance wrapping the provided 
        /// <see cref="ITask"/> instance and with the specified frequency
        /// limit.
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        /// <param name="frequency">Maximum frequency at which the wrapped
        /// task can be executed. Specified in milliseconds.</param>
        public FrequencyLimiter(ITask childTask, float frequency)
            : base(childTask)
        {
            Frequency = frequency;
            _timeSinceLastExecute = 0.0f;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// If at least <see cref="Frequency"/> milliseconds has passed since the last execution, 
        /// this executes the wrapped task and returns a code indicating the result of execution. Otherwise,
        /// the execution is deferred.
        /// </summary>
        /// <returns>Code indicating the result of executing the task or a deferred code if the task
        /// was not executed.</returns>
        public override TaskResultCode Execute()
        {
            _timeSinceLastExecute += (float)Director.SharedDirector.GameTime.ElapsedGameTime.TotalMilliseconds;
            if (_timeSinceLastExecute >= Frequency)
            {
                _timeSinceLastExecute = 0.0f;

                return ChildTask.Execute();
            }

            return TaskResultCode.Deferred;
        }

        #endregion
    }
}
