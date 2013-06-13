using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Contains a set of child tasks that will be executed until
    /// one succeeds or all fail.
    /// </summary>
    public class Selector : Task
    {
        #region Constructors

        /// <summary>
        /// Creates a new Selector instance.
        /// </summary>
        public Selector()
            : base()
        {
        }

        /// <summary>
        /// Creates a new Selector instance with the provided collection of <see cref="ITask"/>
        /// instances as child tasks.
        /// </summary>
        /// <param name="childTasks">Collection of <see cref="ITask"/> instances to use as
        /// child tasks. The tasks will be executed in the order provided, so the ordering
        /// is significant.</param>
        public Selector(IEnumerable<ITask> childTasks)
            : base(childTasks)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Executes each child task in order until one succeeds or all fail.
        /// </summary>
        /// <returns>Success on the first successful execution of a child task, or
        /// failure if all fail.</returns>
        public override TaskResultCode Execute()
        {
            foreach (ITask task in ChildTasks)
            {
                if (task.Execute() == TaskResultCode.Success)
                {
                    return TaskResultCode.Success;
                }
            }

            return TaskResultCode.Failure;
        }

        #endregion
    }
}
