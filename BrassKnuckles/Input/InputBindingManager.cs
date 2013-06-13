using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using System.Threading;
using Nuclex.Input;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Base interface for classes that manage collections of IInputBinding instances.
    /// </summary>
    public interface IInputBindingManager : IGameComponent
    {
        #region Properties

        /// <summary>
        /// Number of InputBinding instances in the cache.
        /// </summary>
        int Count { get; }

        #endregion

        #region Methods

        /// <summary>
        /// Creates a new InputBinding with the specified name, action and list of IInputMatcher instances, and adds it to the cache.
        /// </summary>
        /// <param name="name">Name of the InputBinding.</param>
        /// <param name="action">Delegate to execute if any of the bound IInputMatcher instances match.</param>
        /// <param name="inputMatchers">List of IInputMatcher instances to check for a match.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the name parameter is null, empty or all whitespace, or if the action parameter is null.</exception>
        void AddInputBinding(string name, InputBindingAction action, IEnumerable<IInputMatcher> inputMatchers);

        /// <summary>
        /// Adds the specified InputBinding instance to the cache.
        /// </summary>
        /// <param name="inputBinding">InputBinding instance to add.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the inputBinding parameter is null.</exception>
        void AddInputBinding(InputBinding inputBinding);

        /// <summary>
        /// Removes the InputBinding instance with the specified name from the cache.
        /// </summary>
        /// <param name="name">Name of the InputBinding instance to remove.</param>
        /// <returns>True if found and removed, false if not.</returns>
        bool RemoveInputBinding(string name);

        /// <summary>
        /// Removes all InputBinding instances from the cache.
        /// </summary>
        void ClearInputBindings();

        /// <summary>
        /// Checks the InputBinding instance with the specified name to see if it is matched and runs its action if so.
        /// </summary>
        /// <param name="name">Name of the InputBinding instance to check.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>True if the InputBinding instance was found and matched, false if not.</returns>
        bool CheckAndRunInputBinding(string name, GameTime gameTime);

        #endregion
    }

    /// <summary>
    /// Manages a collection of IInputBinding instances.
    /// </summary>
    public class InputBindingManager : GameComponent, IInputBindingManager
    {
        #region Properties

        /// <summary>
        /// IInputBinding cache.
        /// </summary>
        protected Dictionary<string, InputBinding> InputBindings { get; private set; }

        /// <summary>
        /// Number of InputBinding instances in the cache.
        /// </summary>
        public int Count
        {
            get { return InputBindings.Count; }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new InputBindingManager instance.
        /// </summary>
        public InputBindingManager(Game game)
            : base(game)
        {
            InputBindings = new Dictionary<string, InputBinding>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates a new InputBinding with the specified name, action and list of IInputMatcher instances, and adds it to the cache.
        /// </summary>
        /// <param name="name">Name of the InputBinding.</param>
        /// <param name="action">Delegate to execute if any of the bound IInputMatcher instances match.</param>
        /// <param name="inputMatchers">List of IInputMatcher instances to check for a match.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the name parameter is null, empty or all whitespace, or if the action parameter is null.</exception>
        public void AddInputBinding(string name, InputBindingAction action, IEnumerable<IInputMatcher> inputMatchers)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException("name");
            }

            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            AddInputBinding(new InputBinding(name, action, inputMatchers));
        }

        /// <summary>
        /// Adds the specified InputBinding instance to the cache.
        /// </summary>
        /// <param name="inputBinding">InputBinding instance to add.</param>
        /// <exception cref="System.ArgumentNullException">Thrown if the inputBinding parameter is null.</exception>
        public void AddInputBinding(InputBinding inputBinding)
        {
            if (inputBinding == null)
            {
                throw new ArgumentNullException("inputBinding");
            }

            string name = inputBinding.Name;
            if (InputBindings.ContainsKey(name))
            {
                InputBindings[name] = inputBinding;
            }
            else
            {
                InputBindings.Add(name, inputBinding);
            }
        }

        /// <summary>
        /// Removes the InputBinding instance with the specified name from the cache.
        /// </summary>
        /// <param name="name">Name of the InputBinding instance to remove.</param>
        /// <returns>True if found and removed, false if not.</returns>
        public bool RemoveInputBinding(string name)
        {
            return InputBindings.Remove(name);
        }

        /// <summary>
        /// Removes all InputBinding instances from the cache.
        /// </summary>
        public void ClearInputBindings()
        {
            InputBindings.Clear();
        }

        /// <summary>
        /// Checks the InputBinding instance with the specified name to see if it is matched and runs its action if so.
        /// </summary>
        /// <param name="name">Name of the InputBinding instance to check.</param>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// <returns>True if the InputBinding instance was found and matched, false if not.</returns>
        public bool CheckAndRunInputBinding(string name, GameTime gameTime)
        {
            if (!InputBindings.ContainsKey(name))
            {
                return false;
            }

            InputBinding inputBinding = InputBindings[name];
            foreach (IInputMatcher matcher in inputBinding.InputMatchers)
            {
                if (matcher.Matched())
                {
                    inputBinding.Action.DynamicInvoke(gameTime);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks all IInputBinding instances in the cache and invokes their Action delegate if any of their IInputMatcher instances has a match.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            foreach (InputBinding inputBinding in InputBindings.Values)
            {
                if (inputBinding.Enabled)
                {
                    bool matched = false;
                    if (inputBinding.MatchAll)
                    {
                        matched = inputBinding.InputMatchers.All(m => m.Matched());
                    }
                    else
                    {
                        matched = inputBinding.InputMatchers.Any(m => m.Matched());
                    }

                    if (matched)
                    {
                        inputBinding.Action(gameTime);
                    }
                }
            }
        }

        #endregion
    }
}
