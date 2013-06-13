using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Contains a set of child tasks that will all be executed in sequence
    /// or until one fails.
    /// </summary>
    public class Sequence : Task
    {
        #region Constructors

        /// <summary>
        /// Creates a new Sequence instance.
        /// </summary>
        public Sequence()
            : base()
        {
        }

        /// <summary>
        /// Creates a new Sequence instance with the provided collection of <see cref="ITask"/>
        /// instances as child tasks.
        /// </summary>
        /// <param name="childTasks">Collection of <see cref="ITask"/> instances to use as
        /// child tasks. The tasks will be executed in the order provided, so the ordering
        /// is significant.</param>
        public Sequence(IEnumerable<ITask> childTasks)
            : base(childTasks)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes each child task in order until all succeed or one fails.
        /// </summary>
        /// <returns>Success if all child task succeeds, or the result code of the
        /// first child task that is not successful.</returns>
        public override TaskResultCode Execute()
        {
            foreach (ITask task in ChildTasks)
            {
                TaskResultCode resultCode = task.Execute();
                if (resultCode != TaskResultCode.Success)
                {
                    return resultCode;
                }
            }

            return TaskResultCode.Success;
        }

        #endregion
    }
}
