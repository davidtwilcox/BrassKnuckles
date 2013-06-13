using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Gamepad thumbstick options.
    /// </summary>
    public enum GamePadThumbStick
    {
        #region Thumbsticks
        /// <summary>
        /// Left thumbstick.
        /// </summary>
        Left,
        /// <summary>
        /// Right thumbstick.
        /// </summary>
        Right
        #endregion
    }

    /// <summary>
    /// Gamepad trigger options.
    /// </summary>
    public enum GamePadTrigger
    {
        #region Triggers
        /// <summary>
        /// Left trigger.
        /// </summary>
        Left,
        /// <summary>
        /// Right trigger.
        /// </summary>
        Right
        #endregion
    }

    /// <summary>
    /// Base class for an input binding that works with gamepad states.
    /// </summary>
    public abstract class GamePadMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Player whose gamepad will be bound.
        /// </summary>
        protected PlayerIndex Player { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GamePadBinding instance bound to the specified player.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        protected GamePadMatcher(PlayerIndex player)
        {
            Player = player;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the player's gamepad state matches the binding.
        /// </summary>
        /// <returns>True if the gamepad state matches the binding, false if not.</returns>
        public abstract bool Matched();

        #endregion
    }

    /// <summary>
    /// Base class for binding to gamepad buttons, including the DPad.
    /// </summary>
    public abstract class GamePadButtonMatcher : GamePadMatcher
    {
        #region Properties

        /// <summary>
        /// Bound button.
        /// </summary>
        public Buttons Button { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GamePadButtonBinding instance bound to the specified player's gamepad, button and button state.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="button">Button to bind.</param>
        protected GamePadButtonMatcher(PlayerIndex player, Buttons button)
            : base(player)
        {
            Button = button;
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific gamepad button for a particular player if it is pressed.
    /// </summary>
    public class GamePadButtonPressedMatcher : GamePadButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new GamePadButtonPressedMatcher instance for the specified player's gamepad bound to the specified button.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="button">Button to bind.</param>
        public GamePadButtonPressedMatcher(PlayerIndex player, Buttons button)
            : base(player, button)
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
            return Director.SharedDirector.InputManager.IsGamePadButtonPressed(Player, Button);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific gamepad button for a particular player if it is held.
    /// </summary>
    public class GamePadButtonHeldMatcher : GamePadButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new GamePadButtonHeldMatcher instance for the specified player's gamepad bound to the specified button.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="button">Button to bind.</param>
        public GamePadButtonHeldMatcher(PlayerIndex player, Buttons button)
            : base(player, button)
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
            return Director.SharedDirector.InputManager.IsGamePadButtonHeld(Player, Button);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific gamepad button for a particular player if it is pressed or held.
    /// </summary>
    public class GamePadButtonPressedOrHeldMatcher : GamePadButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new GamePadButtonPressedOrHeldMatcher instance for the specified player's gamepad bound to the specified button.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="button">Button to bind.</param>
        public GamePadButtonPressedOrHeldMatcher(PlayerIndex player, Buttons button)
            : base(player, button)
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
            return Director.SharedDirector.InputManager.IsGamePadButtonPressed(Player, Button) || Director.SharedDirector.InputManager.IsGamePadButtonHeld(Player, Button);
        }

        #endregion
    }

    /// <summary>
    /// Matches a specific gamepad button for a particular player if it is released.
    /// </summary>
    public class GamePadButtonReleasedMatcher : GamePadButtonMatcher
    {
        #region Constructors

        /// <summary>
        /// Creates a new GamePadButtonReleasedMatcher instance for the specified player's gamepad bound to the specified button.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="button">Button to bind.</param>
        public GamePadButtonReleasedMatcher(PlayerIndex player, Buttons button)
            : base(player, button)
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
            return Director.SharedDirector.InputManager.IsGamePadButtonReleased(Player, Button);
        }

        #endregion
    }

    /// <summary>
    /// Binds to gamepad thumbsticks.
    /// </summary>
    public class GamePadThumbStickMatcher : GamePadMatcher
    {
        #region Properties

        /// <summary>
        /// Thumbstick to bind.
        /// </summary>
        public GamePadThumbStick ThumbStick { get; protected set; }

        private float _xMinimumThreshold;
        /// <summary>
        /// Minimum X-axis value that will match.
        /// </summary>
        /// <remarks>Must be from -1.0 and 1.0, and less than or equal to the XMaximumThreshold value.</remarks>
        public float XMinimumThreshold
        {
            get { return _xMinimumThreshold; }
            protected set
            {
                _xMinimumThreshold = MathHelper.Clamp(value, -1.0f, 1.0f);
            }
        }

        private float _xMaximumThreshold;
        /// <summary>
        /// Maximum X-axis value that will match.
        /// </summary>
        /// <remarks>Must be from -1.0 and 1.0, and greater than or equal to the XMinimumThreshold value.</remarks>
        public float XMaximumThreshold
        {
            get { return _xMaximumThreshold; }
            protected set
            {
                _xMaximumThreshold = MathHelper.Clamp(value, -1.0f, 1.0f);
            }
        }

        private float _yMinimumThreshold;
        /// <summary>
        /// Minimum Y-axis value that will match.
        /// </summary>
        /// <remarks>Must be from -1.0 and 1.0, and less than or equal to the YMaximumThreshold value.</remarks>
        public float YMinimumThreshold
        {
            get { return _yMinimumThreshold; }
            protected set
            {
                _yMinimumThreshold = MathHelper.Clamp(value, -1.0f, 1.0f);
            }
        }

        private float _yMaximumThreshold;
        /// <summary>
        /// Maximum Y-axis value that will match.
        /// </summary>
        /// <remarks>Must be from -1.0 and 1.0, and greater than or equal to the YMinimumThreshold value.</remarks>
        public float YMaximumThreshold
        {
            get { return _yMaximumThreshold; }
            protected set
            {
                _yMaximumThreshold = MathHelper.Clamp(value, -1.0f, 1.0f);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GamePadThumbStickBinding instance for the specified player's gamepad, bound to the specified thumbstick, and with the provided X- and Y-axis threshold values.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="thumbstick">Thumbstick to bind.</param>
        /// <param name="xMinimumThreshold">Minimum X-axis value that will match.</param>
        /// <param name="xMaximumThreshold">Maximum X-axis value that will match.</param>
        /// <param name="yMinimumThreshold">Minimum Y-axis value that will match.</param>
        /// <param name="yMaximumThreshold">Maximum Y-axis value that will match.</param>
        /// <remarks>X-axis values must be from -1.0 .. 1.0, inclusive, where negative values are to the left and positive values to the right. Y-axis values must also be from -1.0 .. 1.0, 
        /// where negative values are down and positive values up.</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if xMinimumThreshold is greater than xMaximumThreshold, or if yMinimumThreshold is greater than yMaximumThreshold.</exception>
        public GamePadThumbStickMatcher(PlayerIndex player, GamePadThumbStick thumbstick, float xMinimumThreshold, float xMaximumThreshold, float yMinimumThreshold, float yMaximumThreshold)
            : base(player)
        {
            if (xMinimumThreshold > xMaximumThreshold)
            {
                throw new ArgumentOutOfRangeException("xMinimumThreshold");
            }

            if (yMinimumThreshold > yMaximumThreshold)
            {
                throw new ArgumentOutOfRangeException("yMinimumThreshold");
            }

            ThumbStick = thumbstick;
            XMinimumThreshold = xMinimumThreshold;
            XMaximumThreshold = xMaximumThreshold;
            YMinimumThreshold = yMinimumThreshold;
            YMaximumThreshold = yMaximumThreshold;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the bound thumbstick position is within the X- and Y-axis threshold values.
        /// </summary>
        /// <returns>True if the position is within the threshold values, false if not.</returns>
        public override bool Matched()
        {
            Vector2 position = Vector2.Zero;
            if (ThumbStick == GamePadThumbStick.Left)
            {
                position = Director.SharedDirector.InputManager.GetCurrentGamePadState(Player).ThumbSticks.Left;
            }
            if (ThumbStick == GamePadThumbStick.Right)
            {
                position = Director.SharedDirector.InputManager.GetCurrentGamePadState(Player).ThumbSticks.Right;
            }

            if (!((position.X >= XMinimumThreshold) && (position.X <= XMaximumThreshold) &&
                  (position.Y >= YMinimumThreshold) && (position.Y <= YMaximumThreshold)))
            {
                return false;
            }

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Binds to gamepad triggers.
    /// </summary>
    public class GamePadTriggerMatcher : GamePadMatcher
    {
        #region Properties

        /// <summary>
        /// Trigger to bind.
        /// </summary>
        public GamePadTrigger Trigger { get; protected set; }

        private float _minimumThreshold;
        /// <summary>
        /// Minimum trigger value to match.
        /// </summary>
        /// <remarks>Must be from 0.0 to 1.0, and less than or equal to the MaximumThreshold value.</remarks>
        public float MinimumThreshold
        {
            get { return _minimumThreshold; }
            protected set
            {
                _minimumThreshold = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }

        private float _maximumThreshold;
        /// <summary>
        /// Maximum trigger value to match.
        /// </summary>
        /// <remarks>Must be from 0.0 to 1.0, and greater than or equal to the MinimumThreshold value.</remarks>
        public float MaximumThreshold
        {
            get { return _maximumThreshold; }
            protected set
            {
                _maximumThreshold = MathHelper.Clamp(value, 0.0f, 1.0f);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new GamePadTriggerBinding instance for the specified player's gamepad, bound to the specified trigger and with the provided minimum and maximum threshold values.
        /// </summary>
        /// <param name="player">Player whose gamepad will be bound.</param>
        /// <param name="trigger">Trigger to bind.</param>
        /// <param name="minimumThreshold">Minimum trigger value to match.</param>
        /// <param name="maximumThreshold">Maximum trigger value to match.</param>
        /// <remarks>Minimum and maximum threshold values must be in the range 0.0 .. 1.0, where increasing values indicate increasing pull on the trigger.</remarks>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown if minimumThreshold is greater than maximumThreshold.</exception>
        public GamePadTriggerMatcher(PlayerIndex player, GamePadTrigger trigger, float minimumThreshold, float maximumThreshold)
            : base(player)
        {
            if (minimumThreshold > maximumThreshold)
            {
                throw new ArgumentOutOfRangeException("minimumThreshold");
            }

            Player = player;
            Trigger = trigger;
            MinimumThreshold = minimumThreshold;
            MaximumThreshold = maximumThreshold;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if the bound trigger's value is within the threshold range.
        /// </summary>
        /// <returns>True if the bound trigger's value is within the threshold range, or false if not.</returns>
        public override bool Matched()
        {
            float value = 0.0f;
            if (Trigger == GamePadTrigger.Left)
            {
                value = Director.SharedDirector.InputManager.GetCurrentGamePadState(Player).Triggers.Left;
            }
            if (Trigger == GamePadTrigger.Right)
            {
                value = Director.SharedDirector.InputManager.GetCurrentGamePadState(Player).Triggers.Right;
            }

            if (!((value >= MinimumThreshold) && (value <= MaximumThreshold)))
            {
                return false;
            }

            return true;
        }

        #endregion
    }

    /// <summary>
    /// Input binding for multiple gamepad inputs.
    /// </summary>
    /// <remarks>All matchers must match for this to be considered matched.</remarks>
    public class MultipleGamePadMatcher : IInputMatcher
    {
        #region Properties

        /// <summary>
        /// Array of GamePadMatcher instances to match.
        /// </summary>
        protected GamePadMatcher[] GamePadMatchers { get; set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new MultipleGamePadMatcher instance using the list of GamePadMatcher instances provided.
        /// </summary>
        /// <param name="gamePadMatchers">GamePadMatcher instances to check.</param>
        public MultipleGamePadMatcher(IEnumerable<GamePadMatcher> gamePadMatchers)
        {
            if (gamePadMatchers == null)
            {
                throw new ArgumentNullException("gamePadMatchers");
            }

            GamePadMatchers = gamePadMatchers.ToArray<GamePadMatcher>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Determines if all GamePadMatcher instances bound match.
        /// </summary>
        /// <returns>True if all GamePadMatcher instances match, false if not.</returns>
        public bool Matched()
        {
            return GamePadMatchers.All<GamePadMatcher>(gpm => gpm.Matched());
        }

        #endregion
    }
}
