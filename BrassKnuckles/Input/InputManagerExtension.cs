using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using BrassKnuckles.Utility;
using BrassKnuckles.Geometry;
using Nuclex.Input;

namespace BrassKnuckles.Input
{
    /// <summary>
    /// Extension methods for <see cref="Nuclex.Input.InputManager"/> class.
    /// </summary>
    public static class InputManagerExtension
    {
        #region Private fields

        private static float _elapsedTime;
        private static float _mouseVelocity;
        private static float _mouseXVelocity;
        private static float _mouseYVelocity;
        private static bool _mouseVelocityCalculated;
        private static bool _mouseXVelocityCalculated;
        private static bool _mouseYVelocityCalculated;
        private static KeyboardState _previousKeyboardState;
        private static KeyboardState _currentKeyboardState;
        private static MouseState _previousMouseState;
        private static MouseState _currentMouseState;
        private static Dictionary<PlayerIndex, GamePadState> _previousGamePadStates;
        private static Dictionary<PlayerIndex, GamePadState> _currentGamePadStates;

        #endregion

        #region Constructors

        static InputManagerExtension()
        {
            _previousKeyboardState = Keyboard.GetState();
            _currentKeyboardState = _previousKeyboardState;

            _previousMouseState = Mouse.GetState();
            _currentMouseState = _previousMouseState;

            _previousGamePadStates = new Dictionary<PlayerIndex, GamePadState>(4);
            _previousGamePadStates.Add(PlayerIndex.One, GamePad.GetState(PlayerIndex.One));
            _previousGamePadStates.Add(PlayerIndex.Two, GamePad.GetState(PlayerIndex.Two));
            _previousGamePadStates.Add(PlayerIndex.Three, GamePad.GetState(PlayerIndex.Three));
            _previousGamePadStates.Add(PlayerIndex.Four, GamePad.GetState(PlayerIndex.Four));

            _currentGamePadStates = new Dictionary<PlayerIndex, GamePadState>(4);
            _currentGamePadStates.Add(PlayerIndex.One, _previousGamePadStates[PlayerIndex.One]);
            _currentGamePadStates.Add(PlayerIndex.Two, _previousGamePadStates[PlayerIndex.Two]);
            _currentGamePadStates.Add(PlayerIndex.Three, _previousGamePadStates[PlayerIndex.Three]);
            _currentGamePadStates.Add(PlayerIndex.Four, _previousGamePadStates[PlayerIndex.Four]);
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Gets the previous gamepad state for the specified player.
        /// </summary>
        /// <param name="inputManager">Input manager instance.</param>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>State of the gamepad for the specified player in the previous frame.</returns>
        public static GamePadState GetPreviousGamePadState(this InputManager inputManager, PlayerIndex player)
        {
            return _previousGamePadStates[player];
        }

        /// <summary>
        /// Gets the current gamepad state for the specified player.
        /// </summary>
        /// <param name="inputManager">Input manager instance.</param>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>State of the gamepad for the specified player in the current frame.</returns>
        public static GamePadState GetCurrentGamePadState(this InputManager inputManager, PlayerIndex player)
        {
            return _currentGamePadStates[player];
        }

        /// <summary>
        /// Determines if a specific key is pressed.
        /// </summary>
        /// <param name="key">Key to check if pressed.</param>
        /// <returns>True if the key is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsKeyPressed(this InputManager inputManager, Keys key)
        {
            return _previousKeyboardState.IsKeyUp(key) && _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines if a specific key is being held down.
        /// </summary>
        /// <param name="key">Key to check if held.</param>
        /// <returns>True if the key is down this frame and the previous frame, false otherwise.</returns>
        public static bool IsKeyHeld(this InputManager inputManager, Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Determines if a specific key is pressed or held.
        /// </summary>
        /// <param name="key">Key to check if pressed or held.</param>
        /// <returns>True if the key is pressed or held, false otherwise.</returns>
        public static bool IsKeyPressedOrHeld(this InputManager inputManager, Keys key)
        {
            return inputManager.IsKeyPressed(key) || inputManager.IsKeyHeld(key);
        }

        /// <summary>
        /// Determines if a specific key was just released.
        /// </summary>
        /// <param name="key">Key to check if released.</param>
        /// <returns>True if the key is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsKeyReleased(this InputManager inputManager, Keys key)
        {
            return _previousKeyboardState.IsKeyDown(key) && _currentKeyboardState.IsKeyUp(key);
        }

        /// <summary>
        /// Determines if the left mouse button is pressed.
        /// </summary>
        /// <returns>True if the left mouse button is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsMouseLeftButtonPressed(this InputManager inputManager)
        {
            return (_previousMouseState.LeftButton == ButtonState.Released) && (_currentMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the left mouse button is being held down.
        /// </summary>
        /// <returns>True if the left mouse button is pressed this frame and the previous frame, false otherwise.</returns>
        public static bool IsMouseLeftButtonHeld(this InputManager inputManager)
        {
            return (_previousMouseState.LeftButton == ButtonState.Pressed) && (_currentMouseState.LeftButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the left mouse button is pressed or held down.
        /// </summary>
        /// <returns>True if the left mouse button is pressed or held.</returns>
        public static bool IsMouseLeftButtonPressedOrHeld(this InputManager inputManager)
        {
            return inputManager.IsMouseLeftButtonPressed() || inputManager.IsMouseLeftButtonHeld();
        }

        /// <summary>
        /// Determines if the left mouse button was just released.
        /// </summary>
        /// <returns>True if the left mouse button is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsMouseLeftButtonReleased(this InputManager inputManager)
        {
            return (_previousMouseState.LeftButton == ButtonState.Pressed) && (_currentMouseState.LeftButton == ButtonState.Released);
        }

        /// <summary>
        /// Determines if the right mouse button is pressed.
        /// </summary>
        /// <returns>True if the right mouse button is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsMouseRightButtonPressed(this InputManager inputManager)
        {
            return (_previousMouseState.RightButton == ButtonState.Released) && (_currentMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the right mouse button is being held down.
        /// </summary>
        /// <returns>True if the right mouse button is pressed this frame and the previous frame, false otherwise.</returns>
        public static bool IsMouseRightButtonHeld(this InputManager inputManager)
        {
            return (_previousMouseState.RightButton == ButtonState.Pressed) && (_currentMouseState.RightButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the right mouse button is pressed or held down.
        /// </summary>
        /// <returns>True if the right mouse button is pressed or held.</returns>
        public static bool IsMouseRightButtonPressedOrHeld(this InputManager inputManager)
        {
            return inputManager.IsMouseRightButtonPressed() || inputManager.IsMouseRightButtonHeld();
        }

        /// <summary>
        /// Determines if the right mouse button was just released.
        /// </summary>
        /// <returns>True if the right mouse button is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsMouseRightButtonReleased(this InputManager inputManager)
        {
            return (_previousMouseState.RightButton == ButtonState.Pressed) && (_currentMouseState.RightButton == ButtonState.Released);
        }

        /// <summary>
        /// Determines if the middle mouse button is pressed.
        /// </summary>
        /// <returns>True if the middle mouse button is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsMouseMiddleButtonPressed(this InputManager inputManager)
        {
            return (_previousMouseState.MiddleButton == ButtonState.Released) && (_currentMouseState.MiddleButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the middle mouse button is being held down.
        /// </summary>
        /// <returns>True if the middle mouse button is pressed this frame and the previous frame, false otherwise.</returns>
        public static bool IsMouseMiddleButtonHeld(this InputManager inputManager)
        {
            return (_previousMouseState.MiddleButton == ButtonState.Pressed) && (_currentMouseState.MiddleButton == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the middle mouse button is pressed or held down.
        /// </summary>
        /// <returns>True if the middle mouse button is pressed or held.</returns>
        public static bool IsMouseMiddleButtonPressedOrHeld(this InputManager inputManager)
        {
            return inputManager.IsMouseMiddleButtonPressed() || inputManager.IsMouseMiddleButtonHeld();
        }

        /// <summary>
        /// Determines if the middle mouse button was just released.
        /// </summary>
        /// <returns>True if the middle mouse button is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsMouseMiddleButtonReleased(this InputManager inputManager)
        {
            return (_previousMouseState.MiddleButton == ButtonState.Pressed) && (_currentMouseState.MiddleButton == ButtonState.Released);
        }

        /// <summary>
        /// Determines if the mouse XButton1 is pressed.
        /// </summary>
        /// <returns>True if the mouse XButton1 is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton1Pressed(this InputManager inputManager)
        {
            return (_previousMouseState.XButton1 == ButtonState.Released) && (_currentMouseState.XButton1 == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the mouse XButton1 is held.
        /// </summary>
        /// <returns>True if the mouse XButton1 is pressed this frame and the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton1Held(this InputManager inputManager)
        {
            return (_previousMouseState.XButton1 == ButtonState.Pressed) && (_currentMouseState.XButton1 == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the mouse XButton1 is pressed or held down.
        /// </summary>
        /// <returns>True if the mouse XButton1 is pressed or held.</returns>
        public static bool IsMouseXButton1ButtonPressedOrHeld(this InputManager inputManager)
        {
            return inputManager.IsMouseXButton1Pressed() || inputManager.IsMouseXButton1Held();
        }

        /// <summary>
        /// Determines if the mouse XButton1 was just released.
        /// </summary>
        /// <returns>True if the mouse XButton1 is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton1Released(this InputManager inputManager)
        {
            return (_previousMouseState.XButton1 == ButtonState.Pressed) && (_currentMouseState.XButton1 == ButtonState.Released);
        }

        /// <summary>
        /// Determines if the mouse XButton2 is pressed.
        /// </summary>
        /// <returns>True if the mouse XButton2 is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton2Pressed(this InputManager inputManager)
        {
            return (_previousMouseState.XButton2 == ButtonState.Released) && (_currentMouseState.XButton2 == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the mouse XButton2 is pressed or held down.
        /// </summary>
        /// <returns>True if the mouse XButton2 is pressed or held.</returns>
        public static bool IsMouseXButton2ButtonPressedOrHeld(this InputManager inputManager)
        {
            return inputManager.IsMouseXButton2Pressed() || inputManager.IsMouseXButton2Held();
        }

        /// <summary>
        /// Determines if the mouse XButton2 is held.
        /// </summary>
        /// <returns>True if the mouse XButton2 is pressed this frame and the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton2Held(this InputManager inputManager)
        {
            return (_previousMouseState.XButton2 == ButtonState.Pressed) && (_currentMouseState.XButton2 == ButtonState.Pressed);
        }

        /// <summary>
        /// Determines if the mouse XButton2 was just released.
        /// </summary>
        /// <returns>True if the mouse XButton2 is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsMouseXButton2Released(this InputManager inputManager)
        {
            return (_previousMouseState.XButton2 == ButtonState.Pressed) && (_currentMouseState.XButton2 == ButtonState.Released);
        }

        /// <summary>
        /// Gets the X and Y distance the mouse travelled from the previous frame to the current frame.
        /// </summary>
        /// <returns>Vector2 instance representing the mouse movement distance and direction.</returns>
        public static Vector2 GetMouseChange(this InputManager inputManager)
        {
            return new Vector2(_currentMouseState.X - _previousMouseState.X, _currentMouseState.Y - _previousMouseState.Y);
        }

        /// <summary>
        /// Gets the overall mouse velocity from the previous frame to the current frame, measured in pixels / second.
        /// </summary>
        /// <returns>Float value representing the overall mouse velocity in pixels / second.</returns>
        public static float GetMouseVelocity(this InputManager inputManager)
        {
            if (!_mouseVelocityCalculated)
            {
                Vector2 change = inputManager.GetMouseChange();
                _mouseVelocity = (float)Math.Sqrt((change.X * change.X) + (change.Y * change.Y)) / _elapsedTime;
                _mouseVelocityCalculated = true;
            }

            return _mouseVelocity;
        }

        /// <summary>
        /// Gets the mouse X-axis velocity from the previous frame to the current frame, measured in pixels / second.
        /// </summary>
        /// <returns>Float value representing the mouse X-axis velocity in pixels / second.</returns>
        public static float GetMouseXVelocity(this InputManager inputManager)
        {
            if (!_mouseXVelocityCalculated)
            {
                _mouseXVelocity = (_currentMouseState.X - _previousMouseState.X) / _elapsedTime;
                _mouseXVelocityCalculated = true;
            }

            return _mouseXVelocity;
        }

        /// <summary>
        /// Gets the mouse Y-axis velocity from the previous frame to the current frame, measured in pixels / second.
        /// </summary>
        /// <returns>Float value representing the mouse Y-axis velocity in pixels / second.</returns>
        public static float GetMouseYVelocity(this InputManager inputManager)
        {
            if (!_mouseYVelocityCalculated)
            {
                _mouseYVelocity = (_currentMouseState.Y - _previousMouseState.Y) / _elapsedTime;
                _mouseYVelocityCalculated = true;
            }

            return _mouseYVelocity;
        }

        /// <summary>
        /// Gets the amount the mouse scroll wheel moved from the previous frame to the current frame.
        /// </summary>
        /// <returns>Total scroll wheel movement over last frame.</returns>
        public static int GetMouseScroll(this InputManager inputManager)
        {
            return _currentMouseState.ScrollWheelValue - _previousMouseState.ScrollWheelValue;
        }

        /// <summary>
        /// Gets the mouse OctantDirection based on most recent mouse movement direction.
        /// </summary>
        /// <returns>OctantDirection indicating the direction the mouse moved the last frame.</returns>
        public static OctantDirection GetMouseMoveOctantDirection(this InputManager inputManager)
        {
            return inputManager.GetMouseChange().ToOctantDirection();
        }

        /// <summary>
        /// Gets the mouse QuadrantDirection based on most recent mouse movement direction.
        /// </summary>
        /// <returns>QuadrantDirection indicating the direction the mouse moved the last frame.</returns>
        public static QuadrantDirection GetMouseMoveQuadrantDirection(this InputManager inputManager)
        {
            return inputManager.GetMouseChange().ToQuadrantDirection();
        }

        /// <summary>
        /// Determines if a specific button on a particular player's gamepad is pressed.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <param name="button">Button to check if pressed.</param>
        /// <returns>True if the player's gamepad is connected and the button is down this frame but not the previous frame, false otherwise.</returns>
        public static bool IsGamePadButtonPressed(this InputManager inputManager, PlayerIndex player, Buttons button)
        {
            return _currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsButtonUp(button) && _currentGamePadStates[player].IsButtonDown(button);
        }

        /// <summary>
        /// Determines if a specific button on a particular player's gamepad is being held down.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <param name="button">Button to check if held.</param>
        /// <returns>True if the player's gamepad is connected and the button is down this frame and the previous frame, false otherwise.</returns>
        public static bool IsGamePadButtonHeld(this InputManager inputManager, PlayerIndex player, Buttons button)
        {
            return _currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsButtonDown(button) && _currentGamePadStates[player].IsButtonDown(button);
        }

        /// <summary>
        /// Determines if a specific button on a particular player's gamepad was just released.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <param name="button">Button to check if held.</param>
        /// <returns>True if the player's gamepad is connected and the button is up this frame but down the previous frame, false otherwise.</returns>
        public static bool IsGamePadButtonReleased(this InputManager inputManager, PlayerIndex player, Buttons button)
        {
            return _currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsButtonDown(button) && _currentGamePadStates[player].IsButtonUp(button);
        }

        /// <summary>
        /// Position of the left thumbstick on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Current position of the left thumbstick, or Vector2.Zero if the player's gamepad is disconnected.</returns>
        public static Vector2 GetGamePadLeftThumbStick(this InputManager inputManager, PlayerIndex player)
        {
            Vector2 value = Vector2.Zero;
            if (_currentGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].ThumbSticks.Left;
            }

            return value;
        }

        /// <summary>
        /// Change in the position of the left thumbstick on a particular player's gamepad from the previous frame to the current frame.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Difference between the position of the left thumbstick in the previous frame compared to the current frame, or Vector2.Zero if the player's gamepad 
        /// was disconnected in the previous or current frame.</returns>
        public static Vector2 GetGamePadLeftThumbStickChange(this InputManager inputManager, PlayerIndex player)
        {
            Vector2 value = Vector2.Zero;
            if (_currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].ThumbSticks.Left - _previousGamePadStates[player].ThumbSticks.Left;
            }

            return value;
        }

        /// <summary>
        /// Direction of left thumbstick position on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>OctantDirection indicating left thumbstick direction.</returns>
        public static OctantDirection GetGamePadLeftThumbStickOctantDirection(this InputManager inputManager, PlayerIndex player)
        {
            OctantDirection direction = OctantDirection.None;
            if (_currentGamePadStates[player].IsConnected)
            {
                direction = _currentGamePadStates[player].ThumbSticks.Left.ToOctantDirection();
            }

            return direction;
        }

        /// <summary>
        /// Direction of left thumbstick position on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>QuadrantDirection indicating left thumbstick direction.</returns>
        public static QuadrantDirection GetGamePadLeftThumbStickQuadrantDirection(this InputManager inputManager, PlayerIndex player)
        {
            QuadrantDirection direction = QuadrantDirection.None;
            if (_currentGamePadStates[player].IsConnected)
            {
                direction = _currentGamePadStates[player].ThumbSticks.Left.ToQuadrantDirection();
            }

            return direction;
        }

        /// <summary>
        /// Position of the right thumbstick on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Current position of the right thumbstick, or Vector2.Zero if the player's gamepad is disconnected.</returns>
        public static Vector2 GetGamePadRightThumbStick(this InputManager inputManager, PlayerIndex player)
        {
            Vector2 value = Vector2.Zero;
            if (_currentGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].ThumbSticks.Right;
            }

            return value;
        }

        /// <summary>
        /// Change in the position of the right thumbstick on a particular player's gamepad from the previous frame to the current frame.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Difference between the position of the right thumbstick in the previous frame compared to the current frame, or Vector2.Zero if the player's gamepad 
        /// was disconnected in the previous or current frame.</returns>
        public static Vector2 GetGamePadRightThumbStickChange(this InputManager inputManager, PlayerIndex player)
        {
            Vector2 value = Vector2.Zero;
            if (_currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].ThumbSticks.Right - _previousGamePadStates[player].ThumbSticks.Right;
            }

            return value;
        }

        /// <summary>
        /// Direction of right thumbstick position on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>OctantDirection indicating right thumbstick direction.</returns>
        public static OctantDirection GetGamePadRightThumbStickOctantDirection(this InputManager inputManager, PlayerIndex player)
        {
            OctantDirection direction = OctantDirection.None;
            if (_currentGamePadStates[player].IsConnected)
            {
                direction = _currentGamePadStates[player].ThumbSticks.Right.ToOctantDirection();
            }

            return direction;
        }

        /// <summary>
        /// Direction of right thumbstick position on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>QuadrantDirection indicating right thumbstick direction.</returns>
        public static QuadrantDirection GetGamePadRightThumbStickQuadrantDirection(this InputManager inputManager, PlayerIndex player)
        {
            QuadrantDirection direction = QuadrantDirection.None;
            if (_currentGamePadStates[player].IsConnected)
            {
                direction = _currentGamePadStates[player].ThumbSticks.Right.ToQuadrantDirection();
            }

            return direction;
        }

        /// <summary>
        /// Position of the left trigger on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Current position of the left trigger, or 0 if the player's gamepad is disconnected.</returns>
        public static float GetGamePadLeftTrigger(this InputManager inputManager, PlayerIndex player)
        {
            float value = 0.0f;
            if (_currentGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].Triggers.Left;
            }

            return value;
        }

        /// <summary>
        /// Change in the position of the left trigger on a particular player's gamepad from the previous frame to the current frame.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Difference in the position of the left trigger from the previous frame to the current frame, or 0 if the player's gamepad was disconnected in the current or previous frames.</returns>
        public static float GetGamePadLeftTriggerChange(this InputManager inputManager, PlayerIndex player)
        {
            float value = 0.0f;
            if (_currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].Triggers.Left - _previousGamePadStates[player].Triggers.Left;
            }

            return value;
        }

        /// <summary>
        /// Position of the right trigger on a particular player's gamepad.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Current position of the right trigger, or 0 if the player's gamepad is disconnected.</returns>
        public static float GetGamePadRightTrigger(this InputManager inputManager, PlayerIndex player)
        {
            float value = 0.0f;
            if (_currentGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].Triggers.Right;
            }

            return value;
        }

        /// <summary>
        /// Change in the position of the right trigger on a particular player's gamepad from the previous frame to the current frame.
        /// </summary>
        /// <param name="player">Player whose gamepad will be checked.</param>
        /// <returns>Difference in the position of the right trigger from the previous frame to the current frame, or 0 if the player's gamepad was disconnected in the current or previous frames.</returns>
        public static float GetGamePadRightTriggerChange(this InputManager inputManager, PlayerIndex player)
        {
            float value = 0.0f;
            if (_currentGamePadStates[player].IsConnected && _previousGamePadStates[player].IsConnected)
            {
                value = _currentGamePadStates[player].Triggers.Right - _previousGamePadStates[player].Triggers.Right;
            }

            return value;
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public static void UpdateInputStates(GameTime gameTime)
        {
            _elapsedTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            _previousKeyboardState = _currentKeyboardState;
            _currentKeyboardState = Keyboard.GetState();

            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();
            _mouseVelocityCalculated = false;
            _mouseXVelocityCalculated = false;
            _mouseYVelocityCalculated = false;

            _previousGamePadStates[PlayerIndex.One] = _currentGamePadStates[PlayerIndex.One];
            _previousGamePadStates[PlayerIndex.Two] = _currentGamePadStates[PlayerIndex.Two];
            _previousGamePadStates[PlayerIndex.Three] = _currentGamePadStates[PlayerIndex.Three];
            _previousGamePadStates[PlayerIndex.Four] = _currentGamePadStates[PlayerIndex.Four];

            _currentGamePadStates[PlayerIndex.One] = GamePad.GetState(PlayerIndex.One);
            _currentGamePadStates[PlayerIndex.Two] = GamePad.GetState(PlayerIndex.Two);
            _currentGamePadStates[PlayerIndex.Three] = GamePad.GetState(PlayerIndex.Three);
            _currentGamePadStates[PlayerIndex.Four] = GamePad.GetState(PlayerIndex.Four);
        }

        #endregion
    }
}