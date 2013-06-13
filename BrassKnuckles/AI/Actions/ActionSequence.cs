using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.Actions
{
    /// <summary>
    /// Contains a set of <see cref="Action"/> instances that will be combined
    /// and managed in a set sequence.
    /// </summary>
    public class ActionSequence : Action
    {
        #region Private fields

        private List<Action> _actions;
        private int _activeIndex = 0;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ActionSequence instance with the specified expiration, priority
        /// and collection of <see cref="Action"/> instances to combine.
        /// </summary>
        /// <param name="expiration">The expiration time for the action, specified in milliseconds
        /// from the current time.</param>
        /// <param name="priority">The priority for the action, with higher values taking precedence
        /// over lower values.</param>
        /// <param name="actions">Collection of <see cref="Action"/> instances to combine. These must
        /// be provided in the order in which they should be executed.</param>
        public ActionSequence(float expiration, int priority, IEnumerable<Action> actions)
            : base(expiration, priority)
        {
            _actions = new List<Action>(actions);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the currently active Action instance in the sequence
        /// can interrupt other actions.
        /// </summary>
        /// <returns>True if the currently active Action instance can interrupt
        /// other actions, false if not.</returns>
        public override bool CanInterrupt()
        {
            return _actions[_activeIndex].CanInterrupt();
        }

        /// <summary>
        /// Determines if all contained Action instances can be combined
        /// with the other Action instance provided.
        /// </summary>
        /// <param name="otherAction">Other Action instance to check.</param>
        /// <returns>True if all contained Action instances can be combined
        /// with the other Action instance provided, false if not.</returns>
        public override bool CanDoBoth(Action otherAction)
        {
            return _actions.All(a => a.CanDoBoth(otherAction));
        }

        /// <summary>
        /// Determines if all Action instances in the sequence have been executed
        /// and are complete.
        /// </summary>
        /// <returns>True if all Action instances in the sequence have been executed
        /// and are complete, false if not.</returns>
        public override bool IsComplete()
        {
            return _activeIndex >= _actions.Count;
        }

        /// <summary>
        /// Runs the currently active Action instance in the sequence.
        /// </summary>
        public override void Execute()
        {
            _actions[_activeIndex].Execute();

            if (_actions[_activeIndex].IsComplete())
            {
                ++_activeIndex;
            }
        }

        #endregion
    }
}
