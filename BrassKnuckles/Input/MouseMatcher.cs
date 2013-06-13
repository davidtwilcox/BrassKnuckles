using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Mouse button options
    /// </summary>
    public enum MouseButton
    {
        #region Buttons
        /// <summary>
        /// Left mouse button.
        /// </summary>
        Left,
        /// <summary>
        /// Right mouse button.
        /// </summary>
        Right,
        /// <summary>
        /// Middle mouse button.
        /// </summary>
        Middle,
        /// <summary>
        /// XButton1 (typically forward button).
        /// </summary>
        XButton1,
        /// <summary>
        /// XButton2 (typically back button).
        /// </summary>
        XButton2
        #endregion
    }

    /// <summary>
    /// Base class for binding to mouse buttons.
    /// </summary>
    public abstract class MouseButtonMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Mouse button to bind.
        /// </summary>
        public MouseButton Button { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MouseButtonBinding instance, bound to the specified button.
        /// </summary>
        /// <param name="button">Mouse button to bind.</param>
        protected MouseButtonMatcher(MouseButton button)
        {
            Button = button;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the mouse button state matches the binding.
        /// </summary>
        /// <returns>True if the mouse button is in the necessary state, false if not.</returns>
        public abstract bool Matched();

        #endregion
    }

    /// <summary>
    /// Matches a specified mouse button if it is pressed.
    /// </summary>
    public class MouseButtonPressedMatcher : MouseButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new MouseButtonPressedMatcher instance for the specified button.
        /// </summary>
        /// <param name="button">Button to bind.</param>
        public MouseButtonPressedMatcher(MouseButton button)
            : base(button)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound button is pressed.
        /// </summary>
        /// <returns>True if the button is pressed, false if not.</returns>
        public override bool Matched()
        {
            switch (Button)
            {
                case MouseButton.Left:
                    return Director.SharedDirector.InputManager.IsMouseLeftButtonPressed();
                case MouseButton.Right:
                    return Director.SharedDirector.InputManager.IsMouseRightButtonPressed();
                case MouseButton.Middle:
                    return Director.SharedDirector.InputManager.IsMouseMiddleButtonPressed();
                case MouseButton.XButton1:
                    return Director.SharedDirector.InputManager.IsMouseXButton1Pressed();
                case MouseButton.XButton2:
                    return Director.SharedDirector.InputManager.IsMouseXButton2Pressed();
                default:
                    return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Matches a specified mouse button if it is held.
    /// </summary>
    public class MouseButtonHeldMatcher : MouseButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new MouseButtonHeldMatcher instance for the specified button.
        /// </summary>
        /// <param name="button">Button to bind.</param>
        public MouseButtonHeldMatcher(MouseButton button)
            : base(button)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound button is held.
        /// </summary>
        /// <returns>True if the button is held, false if not.</returns>
        public override bool Matched()
        {
            switch (Button)
            {
                case MouseButton.Left:
                    return Director.SharedDirector.InputManager.IsMouseLeftButtonHeld();
                case MouseButton.Right:
                    return Director.SharedDirector.InputManager.IsMouseRightButtonHeld();
                case MouseButton.Middle:
                    return Director.SharedDirector.InputManager.IsMouseMiddleButtonHeld();
                case MouseButton.XButton1:
                    return Director.SharedDirector.InputManager.IsMouseXButton1Held();
                case MouseButton.XButton2:
                    return Director.SharedDirector.InputManager.IsMouseXButton2Held();
                default:
                    return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Matches a specified mouse button if it is pressed or held.
    /// </summary>
    public class MouseButtonPressedOrHeldMatcher : MouseButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new MouseButtonPressedOrHeldMatcher instance for the specified button.
        /// </summary>
        /// <param name="button">Button to bind.</param>
        public MouseButtonPressedOrHeldMatcher(MouseButton button)
            : base(button)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound button is pressed or held.
        /// </summary>
        /// <returns>True if the button is pressed or held, false if not.</returns>
        public override bool Matched()
        {
            switch (Button)
            {
                case MouseButton.Left:
                    return Director.SharedDirector.InputManager.IsMouseLeftButtonPressed() || Director.SharedDirector.InputManager.IsMouseLeftButtonHeld();
                case MouseButton.Right:
                    return Director.SharedDirector.InputManager.IsMouseRightButtonPressed() || Director.SharedDirector.InputManager.IsMouseRightButtonHeld();
                case MouseButton.Middle:
                    return Director.SharedDirector.InputManager.IsMouseMiddleButtonPressed() || Director.SharedDirector.InputManager.IsMouseMiddleButtonHeld();
                case MouseButton.XButton1:
                    return Director.SharedDirector.InputManager.IsMouseXButton1Pressed() || Director.SharedDirector.InputManager.IsMouseXButton1Held();
                case MouseButton.XButton2:
                    return Director.SharedDirector.InputManager.IsMouseXButton2Pressed() || Director.SharedDirector.InputManager.IsMouseXButton2Held();
                default:
                    return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Matches a specified mouse button if it is released.
    /// </summary>
    public class MouseButtonReleasedMatcher : MouseButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new MouseButtonReleasedMatcher instance for the specified button.
        /// </summary>
        /// <param name="button">Button to bind.</param>
        public MouseButtonReleasedMatcher(MouseButton button)
            : base(button)
        {
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Checks if the bound button is released.
        /// </summary>
        /// <returns>True if the button is released, false if not.</returns>
        public override bool Matched()
        {
            switch (Button)
            {
                case MouseButton.Left:
                    return Director.SharedDirector.InputManager.IsMouseLeftButtonReleased();
                case MouseButton.Right:
                    return Director.SharedDirector.InputManager.IsMouseRightButtonReleased();
                case MouseButton.Middle:
                    return Director.SharedDirector.InputManager.IsMouseMiddleButtonReleased();
                case MouseButton.XButton1:
                    return Director.SharedDirector.InputManager.IsMouseXButton1Released();
                case MouseButton.XButton2:
                    return Director.SharedDirector.InputManager.IsMouseXButton2Released();
                default:
                    return false;
            }
        }

        #endregion
    }

    /// <summary>
    /// Input binding for multiple mouse button inputs.
    /// </summary>
    /// <remarks>All MouseButtonMatcher instances must match for this to be matched.</remarks>
    public class MultipleMouseButtonMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Array of MouseButtonMatcher instances to match.
        /// </summary>
        protected MouseButtonMatcher[] MouseButtonMatchers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MultipleMouseButtonMatcher instance using the list of MouseButtonMatcher instances provided. 
        /// </summary>
        /// <param name="mouseButtonMatchers">MouseButtonMatcher instances to check.</param>
        public MultipleMouseButtonMatcher(IEnumerable<MouseButtonMatcher> mouseButtonMatchers)
        {
            if (mouseButtonMatchers == null)
            {
                throw new ArgumentNullException("mouseButtonMatchers");
            }

            MouseButtonMatchers = mouseButtonMatchers.ToArray<MouseButtonMatcher>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if all MouseButtonMatcher instances match.
        /// </summary>
        /// <returns>True if all MouseButtonMatcher instances match, false if not.</returns>
        public bool Matched()
        {
            return MouseButtonMatchers.All<MouseButtonMatcher>(mb => mb.Matched());
        }

        #endregion
    }
}
