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

namespace BrassKnuckles.Utility
{
    public interface IDebugDisplay : IGameComponent
    {
        #region Methods

        /// <summary>
        /// Adds a debug item to the display.
        /// </summary>
        /// <param name="name">Name of the item to add.</param>
        /// <param name="itemGenerator">Function to generate the item for display.</param>
        void AddItem(string name, Func<string> itemGenerator);

        #endregion
    }

    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class DebugDisplay : DrawableGameComponent, IDebugDisplay
    {
        #region Private fields

        private int _frameCounter;
        private float _elapsedTime;
        private int _frameRate;

        private SpriteBatch _spriteBatch;
        private SpriteFont _font;
        private float _fontWidth;
        private float _fontHeight;
        private Vector2 _position;

        private Dictionary<string, Func<string>> _itemTable;

        #endregion

        #region Constructors

        public DebugDisplay(Game game)
            : base(game)
        {
            _itemTable = new Dictionary<string, Func<string>>();
            DrawOrder = int.MaxValue;
        }

        #endregion

        #region Protected methods

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            _font = Director.SharedDirector.FontManager.GetFont("Game");

            _fontWidth = _font.MeasureString("M").X;
            _fontHeight = _font.MeasureString("Yy").Y;

            _position = new Vector2(_fontWidth, Game.GraphicsDevice.Viewport.Height - _fontHeight);

            base.LoadContent();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Adds a debug item to the display.
        /// </summary>
        /// <param name="name">Name of the item to add.</param>
        /// <param name="itemGenerator">Function to generate the item for display.</param>
        public void AddItem(string name, Func<string> itemGenerator)
        {
            if (_itemTable.ContainsKey(name))
            {
                _itemTable[name] = itemGenerator;
            }
            else
            {
                _itemTable.Add(name, itemGenerator);
                _position.Y -= _fontHeight;
            }
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_elapsedTime >= 1.0f)
            {
                _elapsedTime -= 1.0f;
                _frameRate = _frameCounter;
                _frameCounter = 0;
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            ++_frameCounter;

            string fps = string.Format("{0} fps", _frameRate);
            string[] itemArray = new string[_itemTable.Values.Count + 1];
            itemArray[0] = fps;
            int i = 1;
            foreach (Func<string> item in _itemTable.Values)
            {
                itemArray[i] = item();
                ++i;
            }
            string text = string.Join("\n", itemArray);
            _position.Y = Director.SharedDirector.DisplaySize.Y - (_fontHeight * (_itemTable.Count + 1));

            _spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            _spriteBatch.DrawString(_font, text, _position, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}
