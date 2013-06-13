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
using BrassKnuckles.Input;
using BrassKnuckles.EntityFramework;
using BrassKnuckles.Utility;
using BrassKnuckles.Renderer;
using BrassKnuckles.EntityFramework.Managers;
using BrassKnuckles.AI;
using BrassKnuckles.UI;
using Nuclex.UserInterface;
using Nuclex.Input;
using Nuclex.Game.States;

namespace BrassKnuckles
{
    /// <summary>
    /// Contains information about the display after the display size has been changed.
    /// </summary>
    public sealed class DisplaySizeEventArgs : EventArgs
    {
        #region Properties

        /// <summary>
        /// The previous display size.
        /// </summary>
        public Vector2 OriginalDisplaySize { get; private set; }

        /// <summary>
        /// The current display size.
        /// </summary>
        public Vector2 CurrentDisplaySize { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new DisplaySizeEventArgs instance with the specified original and current display sizes.
        /// </summary>
        /// <param name="originalDisplaySize">Original display size prior to the change.</param>
        /// <param name="currentDisplaySize">Current display size after the change.</param>
        public DisplaySizeEventArgs(Vector2 originalDisplaySize, Vector2 currentDisplaySize)
        {
            if (originalDisplaySize == null)
            {
                throw new ArgumentNullException("oldDisplaySize");
            }

            if (currentDisplaySize == null)
            {
                throw new ArgumentNullException("newDisplaySize");
            }

            OriginalDisplaySize = originalDisplaySize;
            CurrentDisplaySize = currentDisplaySize;
        }

        #endregion
    }

    /// <summary>
    /// Game component that manages access to content, controls the game loop, and manages scenes and updating and drawing their child nodes.
    /// </summary>
    public class Director : DrawableGameComponent
    {
        #region Constants

        SpriteSortMode spriteSortMode = SpriteSortMode.BackToFront;
        BlendState blendState = BlendState.AlphaBlend;
        SamplerState samplerState = SamplerState.LinearClamp;
        DepthStencilState depthStencilState = DepthStencilState.None;
        RasterizerState rasterizerState = RasterizerState.CullCounterClockwise;

        #endregion

        #region Properties

        /// <summary>
        /// Accesses the singleton shared Director instance.
        /// </summary>
        public static Director SharedDirector { get; protected set; }

        /// <summary>
        /// Gets the size of the active display.
        /// </summary>
        public Vector2 DisplaySize { get; protected set; }

        /// <summary>
        /// Coordinates for the center of the active display.
        /// </summary>
        public Vector2 DisplayCenter { get; protected set; }

        /// <summary>
        /// SpriteBatch instance for nodes to use when drawing themselves.
        /// </summary>
        public SpriteBatch SpriteBatch { get; protected set; }

        /// <summary>
        /// Gets or sets the current GameTime.
        /// </summary>
        public GameTime GameTime { get; set; }

        /// <summary>
        /// Flag indicating if game graphics are dependent on the screen resolution or can be scaled to fit the screen. If true, game graphics are scaled to fit the current screen resolution; if false,
        /// game graphics are drawn at their native resolution. Defaults to true.
        /// </summary>
        public bool ResolutionIndependent { get; set; }

        /// <summary>
        /// Default screen resolution for game graphics. Ignored if the ResolutionIndependent flag is false.
        /// </summary>
        public Vector2 PreferredResolution { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="IEntityWorld"/> instance.
        /// </summary>
        public IEntityWorld EntityWorld { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="Nuclex.Game.States.GameStateManager"/> instance.
        /// </summary>
        public GameStateManager GameStateManager { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="IMessageBoard"/> instance.
        /// </summary>
        public IMessageBoard MessageBoard { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="Nuclex.Input.InputManager"/> instance.
        /// </summary>
        public InputManager InputManager { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="IInputBindingManager"/> instance.
        /// </summary>
        public IInputBindingManager InputBindingManager { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="ITextureManager"/> instance.
        /// </summary>
        public ITextureManager TextureManager { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="IFontManager"/> instance.
        /// </summary>
        public IFontManager FontManager { get; protected set; }

        /// <summary>
        /// Gets the shared <see cref="GuiManager"/> instance.
        /// </summary>
        public GuiManager GuiManager { get; protected set; }

        /// <summary>
        /// Defines a transformation matrix to apply when rendering. Defaults to the identity matrix.
        /// </summary>
        public Matrix TransformationMatrix { get; set; }

#if DEBUG
        /// <summary>
        /// Gets the shared <see cref="IDebugDisplay"/> instance.
        /// </summary>
        public IDebugDisplay DebugDisplay { get; protected set; }
#endif

        /// <summary>
        /// Matrix used to scale sprites. If the ResolutionIndependent flag is false, this is still used, but set to the identity matrix so that no scaling is performed.
        /// </summary>
        protected Matrix SpriteScale { get; set; }

        #endregion

        #region Events

        /// <summary>
        /// Raised when the graphics device display size has changed.
        /// </summary>
        public event EventHandler<DisplaySizeEventArgs> DisplaySizeChanged;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new Director instance using the specified Game.
        /// </summary>
        /// <param name="game">Game instance for the Director to use.</param>
        /// <param name="preferredResolution">Preferred resolution for game graphics.</param>
        /// <remarks>Protected to prevent instantiating Director instances outside of the static SharedDirector property.</remarks>
        protected Director(Game game, Vector2 preferredResolution)
            : base(game)
        {
            if (preferredResolution == null)
            {
                throw new ArgumentNullException("defaultResolution");
            }

            PreferredResolution = preferredResolution;
            DisplaySize = preferredResolution;
            DisplayCenter = new Vector2(DisplaySize.X / 2.0f, DisplaySize.Y / 2.0f);
            ResolutionIndependent = false;
            TransformationMatrix = new Matrix();

            EntityWorld = new EntityWorld(game);
            RenderSystem renderSystem = EntityWorld.SystemManager.SetSystem<RenderSystem>(new RenderSystem(), SystemExecutionType.Draw);
            AnimationSystem animationSystem = EntityWorld.SystemManager.SetSystem<AnimationSystem>(new AnimationSystem(), SystemExecutionType.Update);
            MovementSystem movementSystem = EntityWorld.SystemManager.SetSystem<MovementSystem>(new MovementSystem(), SystemExecutionType.Update);
            ControlSystem controlSystem = EntityWorld.SystemManager.SetSystem<ControlSystem>(new ControlSystem(), SystemExecutionType.Update);

            MessageBoard = new MessageBoard(game);
            GameStateManager = new GameStateManager(game.Services);
            InputManager = new InputManager(game.Services);
            InputBindingManager = new InputBindingManager(game);
            TextureManager = new TextureManager(game.Content);
            FontManager = new FontManager(game.Content);
            GuiManager = new GuiManager(game.Services);

            game.Components.Add(EntityWorld);
            game.Components.Add(MessageBoard);
            game.Components.Add(InputManager);
            game.Components.Add(InputBindingManager);

            game.Services.AddService(typeof(IEntityWorld), EntityWorld);
            game.Services.AddService(typeof(IMessageBoard), MessageBoard);
            game.Services.AddService(typeof(IInputBindingManager), InputBindingManager);
            game.Services.AddService(typeof(ITextureManager), TextureManager);
            game.Services.AddService(typeof(IFontManager), FontManager);
#if DEBUG
            DebugDisplay = new DebugDisplay(game);
            game.Components.Add(DebugDisplay);
            game.Services.AddService(typeof(IDebugDisplay), DebugDisplay);
#endif
        }

        #endregion

        #region Event handlers

        private void GraphicsDevice_DeviceReset(object sender, EventArgs e)
        {
            OnGraphicsDeviceReset();
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// Sets the sprite scale matrix based on screen resolution and the ResolutionIndependent flag.
        /// </summary>
        protected void SetSpriteScale()
        {
            float screenXScale = 1.0f;
            float screenYScale = 1.0f;

            if (ResolutionIndependent)
            {
                screenXScale = (float)GraphicsDevice.Viewport.Width / PreferredResolution.X;
                screenYScale = (float)GraphicsDevice.Viewport.Height / PreferredResolution.Y;
            }

            SpriteScale = Matrix.CreateScale(screenXScale, screenYScale, 0.0f);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            DisplaySize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Game.GraphicsDevice.DeviceReset += new EventHandler<EventArgs>(GraphicsDevice_DeviceReset);

            SpriteBatch = new SpriteBatch(GraphicsDevice);

            SetSpriteScale();

            base.LoadContent();
        }

        /// <summary>
        /// Handles any operations that need to occur after the graphics device has been reset.
        /// </summary>
        protected virtual void OnGraphicsDeviceReset()
        {
            SetSpriteScale();

            Vector2 newDisplaySize = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            DisplaySizeEventArgs displaySizeEventArgs = new DisplaySizeEventArgs(DisplaySize, newDisplaySize);
            DisplaySize = newDisplaySize;
            DisplayCenter = new Vector2(DisplaySize.X / 2.0f, DisplaySize.Y / 2.0f);

            if (DisplaySizeChanged != null)
            {
                DisplaySizeChanged(this, displaySizeEventArgs);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Creates the shared Director instance used by other components. If the SharedDirector instance has already been created, this does nothing.
        /// </summary>
        /// <param name="game">Game instance for the Director to use.</param>
        /// <param name="defaultResolution">Default resolution for game graphics.</param>
        public static void CreateSharedDirector(Game game, Vector2 defaultResolution)
        {
            if (SharedDirector == null)
            {
                SharedDirector = new Director(game, defaultResolution);
            }
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            GuiManager.Initialize();
            EntityWorld.SystemManager.InitializeAll();

            base.Initialize();
        }

        /// <summary>
        /// Allows the game component to update itself and updates the active scene.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            GameTime = gameTime;
            InputManagerExtension.UpdateInputStates(gameTime);
            GameStateManager.Update(gameTime);
            EntityWorld.SystemManager.UpdateSynchronous(SystemExecutionType.Update);
            GuiManager.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// Renders the active scene.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            Matrix matrix = TransformationMatrix;
            if (ResolutionIndependent)
            {
                matrix *= SpriteScale;
            }

            GraphicsDevice.Clear(Color.Black);

            SpriteBatch.Begin(spriteSortMode, blendState, samplerState, depthStencilState, rasterizerState, null, matrix);
            GameStateManager.Draw(gameTime);
            EntityWorld.SystemManager.UpdateSynchronous(SystemExecutionType.Draw);
            SpriteBatch.End();

            GuiManager.Draw(gameTime);

            base.Draw(gameTime);
        }

        #endregion
    }
}
