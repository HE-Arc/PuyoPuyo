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

            gb.Resume();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            //gb.Cells[0, 0] = Puyo.Red;
            //gb.Cells[0, 1] = Puyo.Blue;
            //gb.Cells[1, 2] = Puyo.Green;
            //gb.Cells[1, 3] = Puyo.Purple;
            //gb.Cells[2, 0] = Puyo.Yellow;
            gb.Spawn(Puyo.Yellow);
            gb.GetChains(out int[,] foo);
            GameboardTesting.PrintArray(foo, gb.Rows, gb.Columns);
        }

        public override void UnloadContent()
        {

        }
       
        public override void Update(GameTime gameTime)
        {
            UpdateTest();
            gb.Update(gameTime);
        }

        private void UpdateTest()
        {
            int nbPlayer = InputManager.Instance.NbPlayer;

            // Action player 1
            if (nbPlayer >= 1)
            {
                List<Input> inputs = InputManager.Instance.Perform(PlayerIndex.One);


                foreach (Input input in inputs)
                {
                    switch (input)
                    {
                        case Input.Up:
                            gb.Up();
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
                        case Input.Pause:
                            Show<PauseScreen>();
                            break;
                        case Input.Validate:
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            break;
                        case Input.ClockwiseRotation:
                            break;
                    }
                }
            }


            // Action Player 2
            if (nbPlayer >= 2)
            {
                List<Input> inputs = InputManager.Instance.Perform(PlayerIndex.Two);

                foreach (Input input in inputs)
                {
                    switch (input)
                    {
                        case Input.Up:
                            gb.Up();
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
                        case Input.Pause:
                            break;
                        case Input.Validate:
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            break;
                        case Input.ClockwiseRotation:
                            break;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            gb.Draw(_spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
