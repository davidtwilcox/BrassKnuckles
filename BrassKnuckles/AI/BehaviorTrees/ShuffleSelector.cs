using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Similar to the <see cref="Selector"/> task, except the order of the
    /// child tasks is shuffled prior to each execution of the instance.
    /// </summary>
    public class ShuffleSelector : Selector
    {
        #region Public methods

        /// <summary>
        /// Shuffles the order of child tasks, then executes each child task
        /// in order until one succeeds or all fail.
        /// </summary>
        /// <returns>Success on the first successful execution of a child task, or
        /// failure if all fail.</returns>
        public override TaskResultCode Execute()
        {
            ChildTasks.Shuffle();

            return base.Execute();
        }

        #endregion
    }
}
