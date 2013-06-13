using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.BehaviorTrees
{
    /// <summary>
    /// Base interface for task nodes in a behavior tree.
    /// </summary>
    public interface ITask
    {
        #region Properties

        /// <summary>
        /// Gets the number of child tasks contained by this task.
        /// </summary>
        int ChildCount { get; }

        /// <summary>
        /// Gets or sets the <see cref="Blackboard"/> instance used to
        /// store task data and parameters.
        /// </summary>
        Blackboard Blackboard { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Terminates the task if executing.
        /// </summary>
        void Terminate();

        /// <summary>
        /// Executes the task and returns a task indicating the result of execution.
        /// </summary>
        /// <returns>Code indicating the result of executing the task.</returns>
        TaskResultCode Execute();

        /// <summary>
        /// Adds the provided task as a child task.
        /// </summary>
        /// <param name="childTask"><see cref="ITask"/> to add as a child task.</param>
        void AddChild(ITask childTask);

        /// <summary>
        /// Removes the child task at the specified position in the child task list.
        /// </summary>
        /// <param name="index">Index of child <see cref="ITask"/> instance to remove.</param>
        void RemoveChild(int index);

        /// <summary>
        /// Removes all child tasks.
        /// </summary>
        void ClearChildren();

        #endregion
    }

    /// <summary>
    /// Base implementation of task nodes in a behavior tree.
    /// </summary>
    public abstract class Task : ITask
    {
        #region Properties

        /// <summary>
        /// Gets child tasks.
        /// </summary>
        protected List<ITask> ChildTasks { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating execution should terminate.
        /// </summary>
        protected bool ShouldTerminate { get; set; }

        /// <summary>
        /// Gets the number of child tasks contained by this task.
        /// </summary>
        public int ChildCount
        {
            get { return ChildTasks.Count; }
        }

        /// <summary>
        /// Gets or sets the <see cref="Blackboard"/> instance used to
        /// store task data and parameters.
        /// </summary>
        public Blackboard Blackboard { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Task instance.
        /// </summary>
        protected Task()
        {
            ChildTasks = new List<ITask>();
        }

        /// <summary>
        /// Creates a new Task instance with the provided child tasks.
        /// </summary>
        /// <param name="childTasks">Collection of <see cref="ITask"/> instances to use 
        /// as child tasks.</param>
        protected Task(IEnumerable<ITask> childTasks)
        {
            ChildTasks = new List<ITask>(childTasks);
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
        /// Executes the task and returns a code indicating the result of execution.
        /// </summary>
        /// <returns>Code indicating the result of executing the task.</returns>
        public abstract TaskResultCode Execute();

        /// <summary>
        /// Adds the provided task as a child task.
        /// </summary>
        /// <param name="childTask"><see cref="ITask"/> to add as a child task.</param>
        public virtual void AddChild(ITask childTask)
        {
            if (childTask == null)
            {
                throw new ArgumentNullException("childTask");
            }

            ChildTasks.Add(childTask);
        }

        /// <summary>
        /// Removes the child task at the specified position in the child task list.
        /// </summary>
        /// <param name="index">Index of child <see cref="ITask"/> instance to remove.</param>
        public virtual void RemoveChild(int index)
        {
            if (index >= ChildTasks.Count)
            {
                return;
            }

            ChildTasks.RemoveAt(index);
        }

        /// <summary>
        /// Removes all child tasks.
        /// </summary>
        public virtual void ClearChildren()
        {
            ChildTasks.Clear();
        }

        #endregion
    }
}
