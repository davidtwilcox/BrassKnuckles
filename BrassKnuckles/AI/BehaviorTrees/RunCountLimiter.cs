using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that wraps another task and prevents it executing more
    /// than a set number of times.
    /// </summary>
    public class RunCountLimiter : Decorator
    {
        #region Private fields

        private int _runCount;

        #endregion

        #region Properties

        private int _maxRunCount;
        /// <summary>
        /// Gets or sets the maximum number of times the wrapped
        /// task can be executed.
        /// </summary>
        public int MaxRunCount
        {
            get { return _maxRunCount; }
            set { _maxRunCount = Math.Max(0, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new RunCountLimiter instance wrapping the provided <see cref="ITask"/>
        /// instance and with the specified maximum run count.
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        /// <param name="maxRunCount">Maximum number of times the wrapped task
        /// can be executed.</param>
        public RunCountLimiter(ITask childTask, int maxRunCount)
            : base(childTask)
        {
            MaxRunCount = maxRunCount;
            _runCount = 0;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// If the maximum run count has not been reached, this executes the wrapped task and 
        /// returns a code indicating the result of execution. Otherwise, the execution
        /// is deferred.
        /// </summary>
        /// <returns>Code indicating the result of executing the task or a deferred code if the task
        /// was not executed.</returns>
        public override TaskResultCode Execute()
        {
            ++_runCount;
            if (_runCount <= MaxRunCount)
            {
                return ChildTask.Execute();
            }

            return TaskResultCode.Deferred;
        }

        /// <summary>
        /// Resets the current run count to 0 to allow execution of the wrapped
        /// task again.
        /// </summary>
        public void Reset()
        {
            _runCount = 0;
        }

        #endregion
    }
}
