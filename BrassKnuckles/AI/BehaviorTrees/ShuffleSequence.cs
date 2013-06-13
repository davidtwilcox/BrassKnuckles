using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BrassKnuckles.Utility;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Similar to the <see cref="Sequence"/> task, except the order of the
    /// child tasks is shuffled prior to each execution of the instance.
    /// </summary>
    public class ShuffleSequence : Sequence
    {
        #region Public methods

        /// <summary>
        /// Shuffles the order of child tasks, then executes each child task in order 
        /// until all succeed or one fails.
        /// </summary>
        /// <returns>Success if all child task succeeds, or the result code of the
        /// first child task that is not successful.</returns>
        public override TaskResultCode Execute()
        {
            ChildTasks.Shuffle();

            return base.Execute();
        }

        #endregion
    }
}
