using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Base class for tasks that wrap the behavior of a single child task to provide additional features.
    /// </summary>
    /// <remarks>Useful for providing filtering, limiting, or inversion of behavior of other tasks.</remarks>
    public abstract class Decorator : ITask
    {
        #region Properties

        /// <summary>
        /// Gets the number of child tasks contained by this task.
        /// </summary>
        /// <remarks>Will always return 1.</remarks>
        public int ChildCount
        {
            get { return 1; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Blackboard"/> instance used to
        /// store task data and parameters.
        /// </summary>
        public Blackboard Blackboard { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating execution should terminate.
        /// </summary>
        protected bool ShouldTerminate { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ITask"/> instance wrapped by the Decorator.
        /// </summary>
        protected ITask ChildTask { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Decorator instance wrapping the provided 
        /// <see cref="ITask"/> instance.
        /// </summary>
        /// <param name="childTask">Task to wrap.</param>
        protected Decorator(ITask childTask)
        {
            ChildTask = childTask;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Terminates the task if executing.
        /// </summary>
        public virtual void Terminate()
        {
            ShouldTerminate = true;
        }

        /// <summary>
        /// Executes the task and returns a task indicating the result of execution.
        /// </summary>
        /// <returns>Code indicating the result of executing the task.</returns>
        public abstract TaskResultCode Execute();

        /// <summary>
        /// Adds the provided task as a child task.
        /// </summary>
        /// <param name="childTask"><see cref="ITask"/> to add as a child task.</param>
        /// <remarks>Decorator instances are restricted to a single child task, as they
        /// are intended to wrap one and only one other task.</remarks>
        public virtual void AddChild(ITask childTask)
        {
            ChildTask = childTask;
        }

        /// <summary>
        /// Removes the child task at the specified position in the child task list.
        /// </summary>
        /// <param name="index">Index of child <see cref="ITask"/> instance to remove.</param>
        public virtual void RemoveChild(int index) { }

        /// <summary>
        /// Removes all child tasks.
        /// </summary>
        public virtual void ClearChildren() { }

        #endregion
    }
}
