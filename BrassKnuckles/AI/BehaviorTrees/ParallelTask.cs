using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Task container for running a set of child tasks in parallel.
    /// </summary>
    public class ParallelTask : Task
    {
        #region Nested types

        /// <summary>
        /// Wraps an <see cref="ITask"/> Execute() method to be run
        /// asynchronously.
        /// </summary>
        /// <returns>The TaskResultCode returned by the wrapped method.</returns>
        private delegate TaskResultCode AsyncTaskExecute();

        /// <summary>
        /// Contains data needed to manage an asynchronously 
        /// executing task.
        /// </summary>
        private class TaskResult
        {
            #region Properties

            /// <summary>
            /// Gets or sets the <see cref="ITask"/> instance.
            /// </summary>
            public ITask Task { get; set; }

            /// <summary>
            /// Gets or sets the AsyncTaskExecute being run.
            /// </summary>
            public AsyncTaskExecute Execute { get; set; }

            /// <summary>
            /// Gets or sets the IAsyncResult containing execution information.
            /// </summary>
            public IAsyncResult Result { get; set; }

            #endregion
        }

        #endregion

        #region Private fields

        private IList<TaskResult> _taskResults;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ParallelTask instance.
        /// </summary>
        public ParallelTask()
            : base()
        {
        }

        /// <summary>
        /// Creates a new ParallelTask instance with the provided collection
        /// of <see cref="ITask"/> instances as child tasks.
        /// </summary>
        /// <param name="childTasks">Tasks to use as child tasks.</param>
        public ParallelTask(IEnumerable<ITask> childTasks)
            : base(childTasks)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Terminates the task if executing.
        /// </summary>
        public override void Terminate()
        {
            foreach (TaskResult taskResult in _taskResults)
            {
                taskResult.Task.Terminate();
            }

            base.Terminate();
        }

        /// <summary>
        /// Executes all child tasks in parallel. If any child task fails, all child tasks
        /// that are still running will be terminated prematurely.
        /// </summary>
        /// <returns>Success if all child tasks run successfully, the result code of the
        /// first child task to fail if any fail, or interrupted if execution is terminated
        /// prematurely.</returns>
        public override TaskResultCode Execute()
        {
            int taskCount = ChildTasks.Count;
            _taskResults = new List<TaskResult>(taskCount);
            foreach (ITask task in ChildTasks)
            {
                AsyncTaskExecute execute = new AsyncTaskExecute(task.Execute);
                IAsyncResult result = execute.BeginInvoke(null, null);
                _taskResults.Add(new TaskResult() 
                { 
                    Task = task,
                    Execute = execute, 
                    Result = result
                });
            }

            TaskResultCode resultCode = TaskResultCode.Success;
            bool isDone = false;
            while (!ShouldTerminate && !isDone)
            {
                int completedCount = 0;
                foreach (TaskResult taskResult in _taskResults)
                {
                    if (taskResult.Result.IsCompleted == true)
                    {
                        ++completedCount;
                        resultCode = taskResult.Execute.EndInvoke(taskResult.Result);
                    }

                    if (resultCode != TaskResultCode.Success)
                    {
                        Terminate();
                        break;
                    }
                }

                if (completedCount == taskCount)
                {
                    isDone = true;
                }
            }

            if (ShouldTerminate)
            {
                return TaskResultCode.Interrupted;
            }
            else
            {
                return resultCode;
            }
        }

        #endregion
    }
}
