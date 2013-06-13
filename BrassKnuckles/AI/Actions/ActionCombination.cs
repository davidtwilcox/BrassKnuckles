using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BrassKnuckles.AI.Actions
{
    /// <summary>
    /// Contains a set of <see cref="Action"/> instances that will be combined
    /// and managed as one instance.
    /// </summary>
    public class ActionCombination : Action
    {
        #region Private fields

        private List<Action> _actions;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ActionCombination instance with the specified expiration, priority,
        /// and collection of <see cref="Action"/> instances to combine.
        /// </summary>
        /// <param name="expiration">The expiration time for the action, specified in milliseconds
        /// from the current time.</param>
        /// <param name="priority">The priority for the action, with higher values taking precedence
        /// over lower values.</param>
        /// <param name="actions">Collection of <see cref="Action"/> instances to combine.</param>
        public ActionCombination(float expiration, int priority, IEnumerable<Action> actions)
            : base(expiration, priority)
        {
            _actions = new List<Action>(actions);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if any contained Action can interrupt other actions.
        /// </summary>
        /// <returns>True if any single contained Action can interrupt
        /// other actions.</returns>
        public override bool CanInterrupt()
        {
            return _actions.Any(a => a.CanInterrupt());
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
        /// Determines if all contained Action instances are complete.
        /// </summary>
        /// <returns>True if all contained Action instances are complete,
        /// false if not.</returns>
        public override bool IsComplete()
        {
            return _actions.All(a => a.IsComplete());
        }

        /// <summary>
        /// Runs all contained Action instances.
        /// </summary>
        public override void Execute()
        {
            foreach (Action action in _actions)
            {
                action.Execute();
            }
        }

        #endregion
    }
}
