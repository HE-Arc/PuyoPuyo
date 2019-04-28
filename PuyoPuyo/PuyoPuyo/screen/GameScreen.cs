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
        private Texture2D Background;
        private readonly Main _game;
        protected SpriteFont Font { get; private set; }

        
        Gameboard gbPlayer1 = new Gameboard(columns, rows, 50, 0);
        Gameboard gbPlayer2 = new Gameboard(columns, rows, 750, 700);


        public GameScreen(IServiceProvider serviceProvider, Main game)
        {
            _serviceProvider = serviceProvider;
            _game = game;
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            
            Font = _game.Content.Load<SpriteFont>("GameFont");

            gbPlayer1.LoadTexture(_game.Content.Load<Texture2D>("textures/bg/ingame_bg"));
            gbPlayer2.LoadTexture(_game.Content.Load<Texture2D>("textures/bg/ingame_bg"));

            gbPlayer1.Resume();
            gbPlayer2.Resume();
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            UpdateInputs();

            try
            {
                gbPlayer1.Update(gameTime);

                if (InputManager.Instance.NbPlayer >= 2)
                    gbPlayer2.Update(gameTime);
            }
            catch (PlayerException pe)
            {
                // Gameover

                // Set score to gameoverScreen
                FindScreen<GameoverScreen>().setScorePlayer1(gbPlayer1.ScoreManager.Score);


                // Reset game
                gbPlayer1 = null;
                gbPlayer1 = new Gameboard(columns, rows, 50, 0);
                gbPlayer1.Resume();

                if (InputManager.Instance.NbPlayer >= 2)
                {
                    FindScreen<GameoverScreen>().setScorePlayer2(gbPlayer2.ScoreManager.Score);

                    gbPlayer2 = null;
                    gbPlayer2 = new Gameboard(columns, rows, 700, 0);
                    gbPlayer2.Resume();
                }

                // Reset size of windows
                _game.setSize(false);

                // Show gameoverScreen
                Show<GameoverScreen>();
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
                            gbPlayer1.Left();
                            break;
                        case Input.Down:
                            gbPlayer1.Down();
                            break;
                        case Input.Right:
                            gbPlayer1.Right();
                            break;
                        case Input.Pause:
                            Show<PauseScreen>();
                            break;
                        case Input.Validate:
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            gbPlayer1.Rotate(Rotation.Counterclockwise);
                            break;
                        case Input.ClockwiseRotation:
                            gbPlayer1.Rotate(Rotation.Clockwise);
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
                            gbPlayer2.Left();
                            break;
                        case Input.Down:
                            gbPlayer2.Down();
                            break;
                        case Input.Right:
                            gbPlayer2.Right();
                            break;
                        case Input.Pause:
                            break;
                        case Input.Validate:
                            break;
                        case Input.Cancel:
                            break;
                        case Input.CounterclockwiseRotation:
                            gbPlayer2.Rotate(Rotation.Counterclockwise);
                            break;
                        case Input.ClockwiseRotation:
                            gbPlayer2.Rotate(Rotation.Clockwise);
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

            gbPlayer1.Draw(_spriteBatch, Font);
            if (InputManager.Instance.NbPlayer >= 2)
                gbPlayer2.Draw(_spriteBatch, Font);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
