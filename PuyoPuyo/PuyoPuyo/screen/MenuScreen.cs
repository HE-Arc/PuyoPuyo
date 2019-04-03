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
    public abstract class MenuScreen : Screen
    {
        private readonly Main _main;
        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch _spriteBatch;
        private MouseState _previousMouseState;
        private Dictionary<Keys, bool> _previousKeys = new Dictionary<Keys, bool>();
        private MenuItem cursor;
        private MenuItem selectedItem;
        private int indexMenu;
        private InputManager im = new InputManager();
        public List<MenuItem> MenuItems { get; }
        protected SpriteFont Font { get; private set; }
        protected ContentManager Content { get; private set; }
        

        protected MenuScreen(IServiceProvider serviceProvider, Main main)
        {
            _serviceProvider = serviceProvider;
            _main = main;
            MenuItems = new List<MenuItem>();
            indexMenu = 0;
        }

        protected void AddMenuItem(string text, Action action = null)
        {
            var menuItem = new MenuItem(Font, text)
            {
                Position = new Vector2(300, 200 + 32 * MenuItems.Count),
                Action = action
            };

            if (MenuItems.Count == 0)
            {
                selectedItem = menuItem;
                menuItem.Color = Color.Yellow;
                cursor = new MenuItem(Font, ">")
                {
                    Position = new Vector2(290, 200 + 32 * MenuItems.Count),
                    Color = Color.Yellow
                };
            }

            MenuItems.Add(menuItem);
        }

        public override void Initialize()
        {
            base.Initialize();

            Content = new ContentManager(_serviceProvider, "Content");

            // Initialize keys
            foreach(Keys k in Enum.GetValues(typeof(Keys)))
            {
                _previousKeys.Add(k, false);
            }
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
        }

        public override void UnloadContent()
        {
            base.UnloadContent();

            Content.Unload();
            Content.Dispose();
        }

       

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            List<Input> inputs = im.Perform();

            foreach (Input input in inputs)
            {
                switch (input)
                {
                    case Input.None:
                        break;
                    case Input.Home:
                        break;
                    case Input.Start:
                        break;
                    case Input.Back:
                        break;
                    case Input.Up:
                        SelectPrevious();
                        break;
                    case Input.Left:
                        break;
                    case Input.Down:
                        SelectNext();
                        break;
                    case Input.Right:
                        break;
                    case Input.Y:
                        break;
                    case Input.X:
                        break;
                    case Input.A:
                        selectedItem.Action?.Invoke();
                        break;
                    case Input.B:
                        break;
                    case Input.LeftShoulder:
                        break;
                    case Input.RightShoulder:
                        break;
                    case Input.LeftTrigger:
                        break;
                    case Input.RightTrigger:
                        break;
                    case Input.LeftStick:
                        break;
                    case Input.LeftStickUp:
                        SelectPrevious();
                        break;
                    case Input.LeftStickLeft:
                        break;
                    case Input.LeftStickDown:
                        SelectNext();
                        break;
                    case Input.LeftStickRight:
                        break;
                    case Input.RightStick:
                        break;
                    case Input.RightStickUp:
                        break;
                    case Input.RightStickLeft:
                        break;
                    case Input.RightStickDown:
                        break;
                    case Input.RightStickRight:
                        break;
                    case Input.PadUp:
                        SelectPrevious();
                        break;
                    case Input.PadLeft:
                        break;
                    case Input.PadDown:
                        SelectNext();
                        break;
                    case Input.PadRight:
                        break;
                    case Input.Enter:
                        selectedItem.Action?.Invoke();
                        break;
                    case Input.Escape:
                        break;
                    default:
                        break;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            if (_spriteBatch == null)
                return;

            _spriteBatch.Begin();

            foreach (var menuItem in MenuItems)
                menuItem.Draw(_spriteBatch);

            if (cursor != null)
                cursor.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        private void SelectPrevious()
        {
            --indexMenu;
            if (indexMenu < 0)
                indexMenu = MenuItems.Count - 1;

            UpdateSelection();
        }

        private void SelectNext()
        {
            ++indexMenu;
            if (indexMenu > MenuItems.Count - 1)
                indexMenu = 0;

            UpdateSelection();
        }

        private void UpdateSelection()
        {
            selectedItem.Color = Color.White;
            selectedItem = MenuItems[indexMenu];
            selectedItem.Color = Color.Yellow;
            cursor.Position = new Vector2(selectedItem.Position.X - 10, selectedItem.Position.Y);
        }
    }
}
