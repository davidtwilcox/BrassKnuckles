using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Decorator that inverts the result code of the wrapped task, changing success to failure
    /// and any non-success code to success.
    /// </summary>
    public class Invert : Decorator
    {
        #region Constructors

        /// <summary>
        /// Creates a new Invert instance wrapping the provided <see cref="ITask"/> instance.
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        public Invert(ITask childTask)
            : base(childTask)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes the wrapped task, returning the inverse of the tasks result code.
        /// </summary>
        /// <returns>Success if the wrapped task returned a non-successful result code,
        /// or failure if it returned success.</returns>
        public override TaskResultCode Execute()
        {
            TaskResultCode result = ChildTask.Execute();

            return (result == TaskResultCode.Success) ? TaskResultCode.Failure : TaskResultCode.Success;
        }

        #endregion
    }
}
