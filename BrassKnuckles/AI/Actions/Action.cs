using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.Actions
{
    /// <summary>
    /// Default comparer for Action instances. Sorts by priority from high to low,
    /// with equal priority sorted by expiration from low to high.
    /// </summary>
    public sealed class ActionComparer : Comparer<Action>
    {
        #region Public methods

        /// <summary>
        /// Compares the provided <see cref="Action"/> instances to one another
        /// and returns a value indicating if the x parameter is less than, equal to
        /// or greater than the y parameter.
        /// </summary>
        /// <param name="x">First instance to compare.</param>
        /// <param name="y">Second instance to compare.</param>
        /// <returns>A value indicating if the x parameter is less than, equal to
        /// or greater than the y parameter.</returns>
        public override int Compare(Action x, Action y)
        {
            // If equal priority, the action that expires sooner
            // should be placed first when sorting.
            if (x.Priority == y.Priority)
            {
                return x.Expiration.CompareTo(y.Expiration);
            }
            else
            {
                // Higher values take precedence over lower,
                // so compare result is negated.
                return -x.Priority.CompareTo(y.Priority);
            }
        }

        #endregion
    }

    /// <summary>
    /// Base class for discrete actions that can be performed by an AI instance.
    /// </summary>
    public abstract class Action
    {
        #region Properties

        /// <summary>
        /// Gets or sets the expiration time for the action, specified in milliseconds
        /// from the current time.
        /// </summary>
        public float Expiration { get; set; }

        /// <summary>
        /// Gets or sets the priority for the action, with higher values taking precedence
        /// over lower values.
        /// </summary>
        public int Priority { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Action with the specified expiration and priority.
        /// </summary>
        /// <param name="expiration">The expiration time for the action, specified in milliseconds
        /// from the current time.</param>
        /// <param name="priority">The priority for the action, with higher values taking precedence
        /// over lower values.</param>
        protected Action(float expiration, int priority)
        {
            Expiration = expiration;
            Priority = priority;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if this instance can interrupt other actions.
        /// </summary>
        /// <returns>True if this instance can interrupt other actions,
        /// false if not.</returns>
        public abstract bool CanInterrupt();

        /// <summary>
        /// Determines if this instance can be executed at the same time
        /// as the other Action instance provided.
        /// </summary>
        /// <param name="otherAction">Other Action instance to check.</param>
        /// <returns>True if the current instance can be combined with and
        /// executed simultaneously with the other action provided.</returns>
        public abstract bool CanDoBoth(Action otherAction);

        /// <summary>
        /// Determines if this instance has finished executing.
        /// </summary>
        /// <returns>True if the instance has completed execution, 
        /// false if not.</returns>
        public abstract bool IsComplete();

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public abstract void Execute();

        #endregion
    }
}
