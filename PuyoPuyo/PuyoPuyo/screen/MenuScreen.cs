using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using PuyoPuyo.Toolbox;
using System.Linq;

namespace PuyoPuyo.screen
{
    /// <summary>
    /// Define the gestion of menus
    /// </summary>
    public abstract class MenuScreen : Screen
    {
        private readonly Main _main;
        private readonly IServiceProvider _serviceProvider;
        protected SpriteBatch _spriteBatch;
        private MenuItem cursor;
        protected MenuItem selectedItem;
        protected int indexMenu;
        private MenuItem title;
        public List<MenuItem> MenuItems { get; }
        protected SpriteFont Font { get; private set; }
        protected ContentManager Content { get; private set; }

        /// <summary>
        /// Instanciate menu screen
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="main"></param>
        protected MenuScreen(IServiceProvider serviceProvider, Main main)
        {
            _serviceProvider = serviceProvider;
            _main = main;
            MenuItems = new List<MenuItem>();
            indexMenu = 0;
        }

        /// <summary>
        /// Add button with action on screen
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        protected void AddMenuItem(string text, Action action = null)
        {
            var menuItem = new MenuItem(Font, text)
            {
                Position = new Vector2(200, 300 + 100 * MenuItems.Count),
                Action = action
            };

            // Select first item
            if (MenuItems.Count == 0)
            {
                selectedItem = menuItem;
                menuItem.Color = Color.Yellow;
                cursor = new MenuItem(Font, ">")
                {
                    Position = new Vector2(150, 300 + 100 * MenuItems.Count),
                    Color = Color.Yellow
                };
            }

            MenuItems.Add(menuItem);
        }

        public override void Initialize()
        {
            base.Initialize();

            Content = new ContentManager(_serviceProvider, "Content");

        }

        public override void Dispose()
        {
            base.Dispose();

            _spriteBatch.Dispose();
        }

        public override void LoadContent()
        {
            base.LoadContent();

            var graphicsDeviceService = (IGraphicsDeviceService)_serviceProvider.GetService(typeof(IGraphicsDeviceService));

            _spriteBatch = new SpriteBatch(graphicsDeviceService.GraphicsDevice);
            Font = Content.Load<SpriteFont>("Font");

            title = new MenuItem(Font, "title");
            title.Position = new Vector2(150, 50);
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Content.Unload();
            Content.Dispose();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_spriteBatch == null)
                return;

            _spriteBatch.Begin();

            title.Draw(_spriteBatch);

            foreach (var menuItem in MenuItems)
                menuItem.Draw(_spriteBatch);

            if (cursor != null)
                cursor.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        /// <summary>
        /// Select previous link
        /// </summary>
        protected void SelectPrevious()
        {
            --indexMenu;
            if (indexMenu < 0)
                indexMenu = MenuItems.Count - 1;

            UpdateSelection();
        }

        /// <summary>
        /// select second link
        /// </summary>
        protected void SelectNext()
        {
            ++indexMenu;
            if (indexMenu > MenuItems.Count - 1)
                indexMenu = 0;

            UpdateSelection();
        }

        /// <summary>
        /// Move cursor at the selected link
        /// </summary>
        private void UpdateSelection()
        {
            selectedItem.Color = Color.White;
            selectedItem = MenuItems[indexMenu];
            selectedItem.Color = Color.Yellow;
            cursor.Position = new Vector2(selectedItem.Position.X - 50, selectedItem.Position.Y);
        }

        /// <summary>
        /// Update title screen
        /// </summary>
        /// <param name="title"></param>
        protected void SetTitle(string title)
        {
            this.title.Text = title;
        }
    }
}
