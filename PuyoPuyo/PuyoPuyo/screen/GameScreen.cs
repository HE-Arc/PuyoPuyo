using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using PuyoPuyo.Exceptions;
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
        private const int columns = 6;
        private const int rows = 14;

        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch _spriteBatch;
        private readonly Main _game;
        protected SpriteFont Font { get; private set; }

        // GameBoard Test
        Gameboard gb = new Gameboard(columns, rows);
        bool isGameOver = false;


        public GameScreen(IServiceProvider serviceProvider, Main game)
        {
            _serviceProvider = serviceProvider;
            _game = game;
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Font = _game.Content.Load<SpriteFont>("GameFont");
            gb.Resume();

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            UpdateInputs();
            if (isGameOver) return;
            try
            {
                gb.Update(gameTime);
            }
            catch (PlayerException pe)
            {
                // Gameover
                isGameOver = true;
            }
            catch (Exception e)
            {
                Console.Error.WriteLine("[Gameboard] : " + e.Message);
            }
        }

        private void UpdateInputs()
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
                            if (isGameOver)
                            {
                                gb = null;
                                gb = new Gameboard(columns, rows);
                                gb.Resume();
                                isGameOver = false;
                            }
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            gb.Rotate(Rotation.Counterclockwise);
                            break;
                        case Input.ClockwiseRotation:
                            gb.Rotate(Rotation.Clockwise);
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
                            if (isGameOver)
                            {
                                gb = null;
                                gb = new Gameboard(columns, rows);
                                gb.Resume();
                                isGameOver = false;
                            }
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            gb.Rotate(Rotation.Counterclockwise);
                            break;
                        case Input.ClockwiseRotation:
                            gb.Rotate(Rotation.Clockwise);
                            break;
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (_spriteBatch == null)
                return;

            _game.GraphicsDevice.Clear(Color.DarkGray);

            _spriteBatch.Begin();

            //TODO : Buttons to retry, return to menu, help, etc
            //TODO : Use GameoverScreen (needs ability to display score, etc)
            if(isGameOver)
            {
                Vector2 gol = new Vector2(
                        _game.GraphicsDevice.Viewport.Width / 2,
                        _game.GraphicsDevice.Viewport.Height / 2
                        );
                string gos = String.Format("Gameover !\r\nYour score : {0}\r\n press 'Enter' to retry", gb.ScoreManager.Score);
                _spriteBatch.DrawString(Font, gos, gol, Color.IndianRed);
            }
            else
            {
                gb.Draw(_spriteBatch, Font);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
