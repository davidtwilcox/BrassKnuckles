using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Base class for input bindings for a single key
    /// </summary>
    public abstract class KeyMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Gets the bound key.
        /// </summary>
        public Keys Key { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new KeyBinding instance bound to the specified key.
        /// </summary>
        /// <param name="key">Key to bind.</param>
        protected KeyMatcher(Keys key)
        {
            Key = key;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the bound key is in the required state.
        /// </summary>
        /// <returns>True if the bound key is in the required state, false if not.</returns>
        public abstract bool Matched();

        #endregion
    }

    /// <summary>
    /// Matches a specific key if it is pressed.
    /// </summary>
    public class KeyPressedMatcher : KeyMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeyPressedMatcher instance for the specified key.
        /// </summary>
        /// <param name="key">Key to bind.</param>
        /// <param name="inputManager"><see cref="IDirector.SharedDirector.InputManager"/> to use for retrieving input state.</param>
        public KeyPressedMatcher(Keys key)
            : base(key)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound key is pressed.
        /// </summary>
        /// <returns>True if the bound key is pressed, false if not.</returns>
        public override bool Matched()
        {
            return Director.SharedDirector.InputManager.IsKeyPressed(Key);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific key if it is held.
    /// </summary>
    public class KeyHeldMatcher : KeyMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeyHeldMatcher instance for the specified key.
        /// </summary>
        /// <param name="key">Key to bind.</param>
        /// <param name="inputManager"><see cref="IDirector.SharedDirector.InputManager"/> to use for retrieving input state.</param>
        public KeyHeldMatcher(Keys key)
            : base(key)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound key is held.
        /// </summary>
        /// <returns>True if the bound key is held, false if not.</returns>
        public override bool Matched()
        {
            return Director.SharedDirector.InputManager.IsKeyHeld(Key);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific key if it is pressed or held.
    /// </summary>
    public class KeyPressedOrHeldMatcher : KeyMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeyPressedOrHeldMatcher instance for the specified key.
        /// </summary>
        /// <param name="key">Key to bind.</param>
        /// <param name="inputManager"><see cref="IDirector.SharedDirector.InputManager"/> to use for retrieving input state.</param>
        public KeyPressedOrHeldMatcher(Keys key)
            : base(key)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound key is pressed or held.
        /// </summary>
        /// <returns>True if the bound key is pressed or held, false if not.</returns>
        public override bool Matched()
        {
            return Director.SharedDirector.InputManager.IsKeyPressed(Key) || Director.SharedDirector.InputManager.IsKeyHeld(Key);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific key if it is released.
    /// </summary>
    public class KeyReleasedMatcher : KeyMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeyReleasedMatcher instance for the specified key.
        /// </summary>
        /// <param name="key">Key to bind.</param>
        /// <param name="inputManager"><see cref="IDirector.SharedDirector.InputManager"/> to use for retrieving input state.</param>
        public KeyReleasedMatcher(Keys key)
            : base(key)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound key is released.
        /// </summary>
        /// <returns>True if the bound key is held, false if not.</returns>
        public override bool Matched()
        {
            return Director.SharedDirector.InputManager.IsKeyReleased(Key);
        }

        #endregion
    }

    /// <summary>
    /// Input binding for multiple keys.
    /// </summary>
    /// <remarks>All matchers must match for this to be considered matched.</remarks> 
    public class MultipleKeyMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Array of all keys bound and their required states.
        /// </summary>
        protected KeyMatcher[] KeyMatchers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MultipleKeyMatcher instance from the provided sequence of KeyBinding instances.
        /// </summary>
        /// <param name="keyMatchers">KeyMatcher instances which will be bound.</param>
        public MultipleKeyMatcher(IEnumerable<KeyMatcher> keyMatchers)
        {
            if (keyMatchers == null)
            {
                throw new ArgumentNullException("boundKeys");
            }

            KeyMatchers = keyMatchers.ToArray<KeyMatcher>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if all bound KeyBinding instances have been in the current input state.
        /// </summary>
        /// <returns>True if all key bindings match, false if not.</returns>
        public bool Matched()
        {
            return KeyMatchers.All(kb => kb.Matched());
        }

        #endregion
    }
}
