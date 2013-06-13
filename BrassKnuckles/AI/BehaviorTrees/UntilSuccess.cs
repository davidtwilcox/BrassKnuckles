using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that repeatedly runs the wrapped task until it succeeds.
    /// </summary>
    public class UntilSuccess : Decorator
    {
        #region Constructors

        /// <summary>
        /// Creates a new UntilSuccess instance wrapping the provided 
        /// <see cref="ITask"/> instance. 
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        public UntilSuccess(ITask childTask)
            : base(childTask)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the wrapped task repeatedly as long as it returns
        /// a non-success result code.
        /// </summary>
        /// <returns>Always returns success.</returns>
        public override TaskResultCode Execute()
        {
            TaskResultCode result;
            do
            {
                result = ChildTask.Execute();
            }
            while (result != TaskResultCode.Success);

            return TaskResultCode.Success;
        }

        #endregion
    }
}
