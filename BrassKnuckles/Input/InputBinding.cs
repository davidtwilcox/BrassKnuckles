using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Contains a method that will be called if an IInputBinding instance has a match.
    /// </summary>
    /// <param name="gameTime">Provides a snapshot of timing values.</param>
    public delegate void InputBindingAction(GameTime gameTime);

    /// <summary>
    /// Binds a list of IInputMatcher instances to a method that will be executed if any of them match.
    /// </summary>
    public class InputBinding
    {
        #region Properties

        private string _name;
        /// <summary>
        /// Name of the InputBinding.
        /// </summary>
        public string Name
        {
            get { return _name; }
            protected set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentNullException("Name");
                }

                _name = value;
            }
        }

        /// <summary>
        /// Whether or not the InputBinding should be checked for matches.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// If true, all <see cref="IInputMatcher"/> instances in the <see cref="InputMatchers"/> property must match
        /// to trigger the action. Otherwise, any match will trigger the action.
        /// </summary>
        /// <remarks>Default value is false (i.e., any match will trigger).</remarks>
        public bool MatchAll { get; set; }

        /// <summary>
        /// Delegate to execute if any of the bound IInputMatcher instances match.
        /// </summary>
        public InputBindingAction Action { get; protected set; }

        /// <summary>
        /// List of IInputMatcher instances to check for a match.
        /// </summary>
        public IList<IInputMatcher> InputMatchers { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new InputBinding instance with the specified name, action and list of IInputMatcher instances.
        /// </summary>
        /// <param name="name">Name of the InputBinding.</param>
        /// <param name="action">Delegate to execute if any of the bound IInputMatcher instances match.</param>
        /// <param name="inputMatchers">List of IInputMatcher instances to check for a match.</param>
        public InputBinding(string name, InputBindingAction action, IEnumerable<IInputMatcher> inputMatchers)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (inputMatchers == null)
            {
                throw new ArgumentNullException("inputMatchers");
            }

            Name = name;
            Action = action;
            Enabled = true;
            MatchAll = false;
            InputMatchers = new List<IInputMatcher>(inputMatchers);
        }

        #endregion
    }
}
