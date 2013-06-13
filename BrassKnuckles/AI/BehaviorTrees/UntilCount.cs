using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that runs the wrapped task a set number of times or until failure,
    /// whichever occurs first.
    /// </summary>
    public class UntilCount : Decorator
    {
        #region Properties

        private int _runCount;
        /// <summary>
        /// Gets or sets the number of times the wrapped
        /// task will be run.
        /// </summary>
        /// <remarks>Minimum value is 1.</remarks>
        public int RunCount
        {
            get { return _runCount; }
            set { _runCount = Math.Max(1, value); }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new UntilCount instance wrapping the provided <see cref="ITask"/>
        /// instance and set to run the specified number of times.
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        /// <param name="runCount">Number of times the wrapped task will be run on
        /// each execution of this instance.</param>
        public UntilCount(ITask childTask, int runCount)
            : base(childTask)
        {
            RunCount = runCount;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the wrapped task <see cref="RunCount"/> number of times or until
        /// failure, whichever occurs first.
        /// </summary>
        /// <returns>Success if the wrapped task was run the set number of times without failure,
        /// or the result code of the last iteration if not successful.</returns>
        public override TaskResultCode Execute()
        {
            int count = 0;
            TaskResultCode result = TaskResultCode.Success;
            while ((count < RunCount) && (result == TaskResultCode.Success))
            {
                result = ChildTask.Execute();
                ++count;
            }

            return result;
        }

        #endregion
    }
}
