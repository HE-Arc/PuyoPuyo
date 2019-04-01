using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.screen
{
    public class GameScreen : Screen
    {
        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch _spriteBatch;
        private readonly Main _game;
        private InputManager im;

        // GameBoard Test
        Gameboard gb = new Gameboard(6, 12);

        // TEST
        Texture2D texture;
        Vector2 position;
        Vector2 origin;

        public GameScreen(IServiceProvider serviceProvider, Main game)
        {

            _serviceProvider = serviceProvider;
            _game = game;
            im = new InputManager();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            position = new Vector2(_game.GraphicsDevice.Viewport.Width / 2,
                _game.GraphicsDevice.Viewport.Height / 2);
            origin = new Vector2(128, 128);
            texture = _game.Content.Load<Texture2D>("textures/puyos/R");

            gb.Cells[0, 0] = Puyo.Red;
            gb.Cells[0, 1] = Puyo.Blue;
            gb.Cells[1, 2] = Puyo.Green;
            gb.Cells[1, 3] = Puyo.Purple;
            gb.Cells[2, 0] = Puyo.Yellow;
        }

        public override void UnloadContent()
        {

        }
       
        public override void Update(GameTime gameTime)
        {
            UpdateTest();
        }

        private void UpdateTest()
        {
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
                        position.Y -= 10;
                        break;
                    case Input.Left:
                        position.X -= 10;
                        break;
                    case Input.Down:
                        position.Y += 10;
                        break;
                    case Input.Right:
                        position.X += 10;
                        break;
                    case Input.Y:
                        break;
                    case Input.X:
                        break;
                    case Input.A:
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
                        position.Y -= 10;
                        break;
                    case Input.LeftStickLeft:
                        position.X -= 10;
                        break;
                    case Input.LeftStickDown:
                        position.Y += 10;
                        break;
                    case Input.LeftStickRight:
                        position.X += 10;
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
                        break;
                    case Input.PadLeft:
                        break;
                    case Input.PadDown:
                        break;
                    case Input.PadRight:
                        break;
                    case Input.Enter:
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
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, origin: origin);
            gb.Draw(_spriteBatch);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
