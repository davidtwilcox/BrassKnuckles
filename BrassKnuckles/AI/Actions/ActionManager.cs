using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.Support.Collections;

namespace BrassKnuckles.AI.Actions
{
    /// <summary>
    /// Base interface for classes that manage <see cref="Action"/> instances for an AI instance.
    /// </summary>
    public interface IActionManager
    {
        #region Methods

        /// <summary>
        /// Adds the provided <see cref="Action"/> instances to the scheduled action queue.
        /// </summary>
        /// <param name="action">Action instance to be scheduled.</param>
        void ScheduleAction(Action action);

        /// <summary>
        /// Updates the instance.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        void Update(GameTime gameTime);

        #endregion
    }

    /// <summary>
    /// Manages a queue of <see cref="Action"/> instances for a single AI instance.
    /// </summary>
    /// <remarks>There will be one ActionManager instance per AI instance, such as a character or a general
    /// managing a group of characters. Unlike other "manager" classes, this is not a global instance.</remarks>
    public class ActionManager : IActionManager
    {
        #region Private fields

        ActionComparer _comparer;
        List<Action> _actions;
        List<Action> _active;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new ActionManager instance.
        /// </summary>
        public ActionManager()
        {
            _comparer = new ActionComparer();
            _actions = new List<Action>();
            _active = new List<Action>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds the provided <see cref="Action"/> instances to the scheduled action queue.
        /// </summary>
        /// <param name="action">Action instance to be scheduled.</param>
        public void ScheduleAction(Action action)
        {
            _actions.Add(action);
        }

        /// <summary>
        /// Updates the instance.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public void Update(GameTime gameTime)
        {
            // Sort main action list by priority, then expiration
            _actions.Sort(_comparer);

            // Look for any actions in main list that can interrupt
            // the currently active actions
            if (_active.Count > 0)
            {
                _active.Sort(_comparer);
                int highestPriority = _active[0].Priority;
                foreach (Action action in _actions)
                {
                    // Low priority actions can't interrupt higher priority actions, so quit looking
                    if (action.Priority <= highestPriority)
                    {
                        break;
                    }

                    if (action.CanInterrupt())
                    {
                        _active.Clear();
                        _active.Add(action);
                    }
                }
            }

            float elapsedTime = (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            List<int> indexesToRemove = new List<int>();

            // Decrement expiration time for all actions and
            // look for actions that can be combined with currently
            // active actions
            for (int i = 0; i < _actions.Count; ++i)
            {
                Action action = _actions[i];

                bool removing = false;

                // Remove expired actions from list even if they haven't
                // been executed yet. Will still check in the next step
                // to see if they can be executed, but that will be the
                // last chance
                action.Expiration -= elapsedTime;
                if (action.Expiration <= 0.0f)
                {
                    indexesToRemove.Add(i);
                    removing = true;
                }

                // Look for actions that can be combined with
                // currently active actions
                bool canCombine = true;
                foreach (Action activeAction in _active)
                {
                    if (!action.CanDoBoth(activeAction))
                    {
                        canCombine = false;
                        break;
                    }
                }
                // If action can't be combined with active actions,
                // move on to the next action in the list
                if (!canCombine)
                {
                    continue;
                }

                // If not already being removed due to expiration,
                // then remove since the action will be added to
                // the active list
                if (!removing)
                {
                    indexesToRemove.Add(i);
                }

                _active.Add(action);
            }

            // Remove expired and activated actions from the main list
            for (int i = indexesToRemove.Count - 1; i >= 0; --i)
            {
                _actions.RemoveAt(i);
            }
            indexesToRemove.Clear();

            // Look for active actions that have completed and set
            // them up to be removed from the active list
            for (int i = 0; i < _active.Count; ++i)
            {
                if (_active[i].IsComplete())
                {
                    indexesToRemove.Add(i);
                }
            }

            // Remove completed actions from the active list
            for (int i = indexesToRemove.Count - 1; i >= 0; --i)
            {
                _active.RemoveAt(i);
            }

            // Execute all actions in the active list
            foreach (Action activeAction in _active)
            {
                activeAction.Execute();
            }
        }

        #endregion
    }
}
