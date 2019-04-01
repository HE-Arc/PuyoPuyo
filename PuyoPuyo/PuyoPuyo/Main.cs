﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using PuyoPuyo.screen;
using PuyoPuyo.Tests;
using PuyoPuyo.Toolbox;

namespace PuyoPuyo
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        public static Microsoft.Xna.Framework.Content.ContentManager ContentManager;
        public static GraphicsDeviceManager GraphicsDeviceManager;

        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        private ScreenGameComponent screenGameComponent;
        private TextureManager textureManager = TextureManager.Instance;

        public Main()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Init screen management
            SetScreenManagement();

            // Publish services
            ContentManager = this.Content;
            GraphicsDeviceManager = this.graphics;

            // Init texture manager
            textureManager.Initialize(Content);

            GameboardTesting.Test();
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load assets
            textureManager.LoadContent();

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void SetScreenManagement()
        {
            Components.Add(screenGameComponent = new ScreenGameComponent(this));

            screenGameComponent.Register(new MainMenuScreen(this.Services, this));
            screenGameComponent.Register(new GameScreen(this.Services, this));
            screenGameComponent.Register(new TutorialScreen(this.Services, this));
        }
    }
}
