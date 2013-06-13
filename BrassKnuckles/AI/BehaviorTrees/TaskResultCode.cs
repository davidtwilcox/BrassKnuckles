using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Result of a behavior tree task's execution.
    /// </summary>
    public enum TaskResultCode
    {
        /// <summary>
        /// Task completed successfully.
        /// </summary>
        Success = 0,
        /// <summary>
        /// Task failed.
        /// </summary>
        Failure,
        /// <summary>
        /// Task execution was deferred until later.
        /// </summary>
        Deferred,
        /// <summary>
        /// Task execution was interrupted by another task.
        /// </summary>
        Interrupted,
        /// <summary>
        /// Task execution was canceled.
        /// </summary>
        Canceled
    }
}
