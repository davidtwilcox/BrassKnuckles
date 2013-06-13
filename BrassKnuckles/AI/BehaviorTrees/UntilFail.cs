using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that repeatedly runs the wrapped task until it fails.
    /// </summary>
    public class UntilFail : Decorator
    {
        #region Constructors

        /// <summary>
        /// Creates a new UntilFail instance wrapping the provided 
        /// <see cref="ITask"/> instance. 
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        public UntilFail(ITask childTask)
            : base(childTask)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the wrapped task repeatedly as long as it returns
        /// a success result code.
        /// </summary>
        /// <returns>Always returns success.</returns>
        public override TaskResultCode Execute()
        {
            TaskResultCode result;
            do
            {
                result = ChildTask.Execute();
            }
            while (result == TaskResultCode.Success);

            return TaskResultCode.Success;
        }

        #endregion
    }
}
