using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Contains a set of child tasks, of which one will be randomly selected and
    /// executed when the instance is executed.
    /// </summary>
    public class RandomTask : Task
    {
        #region Constructors

        /// <summary>
        /// Creates a new RandomTask instance.
        /// </summary>
        public RandomTask()
            : base()
        {
        }

        /// <summary>
        /// Creates a new RandomTask instance with the provided collection of <see cref="ITask"/>
        /// instances as child tasks.
        /// </summary>
        /// <param name="childTasks">Collection of <see cref="ITask"/> instances to use as
        /// child tasks. The ordering of child tasks is not significant.</param>
        public RandomTask(IEnumerable<ITask> childTasks)
            : base(childTasks)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Selects a single child task randomly and executes it, returning the
        /// result of that child task.
        /// </summary>
        /// <returns>The result code of the randomly selected child task that
        /// was executed.</returns>
        public override TaskResultCode Execute()
        {
            return ChildTasks.RandomElement().Execute();
        }

        #endregion
    }
}
