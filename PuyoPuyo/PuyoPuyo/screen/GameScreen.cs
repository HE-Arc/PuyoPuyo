using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Tests;
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
        private bool puyoTest = true;

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
          
            

            //gb.Spawn(PuyoColor.Yellow);
            //gb.GetChains(out int[,] foo);
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Console.WriteLine("Fromage");
            gb.Resume();
            
        }

        public override void UnloadContent()
        {

        }
       
        public override void Update(GameTime gameTime)
        {
            if (puyoTest)
            {
                Console.WriteLine("pika");
                gb.Spawn(PuyoColor.Yellow);
                gb.GetChains(out int[,] foo);
                puyoTest = false;
            }

            UpdateTest();
            gb.Update(gameTime);
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
                        break;
                    case Input.Left:
                        gb.Left();
                        break;
                    case Input.Down:
                        gb.Down();
                        break;
                    case Input.Right:
                        gb.Right();
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
            if (_spriteBatch == null)
                return;

            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            gb.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
