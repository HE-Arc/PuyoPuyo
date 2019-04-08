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
        protected SpriteFont Font { get; private set; }

        // GameBoard Test
        Gameboard gb = new Gameboard(6, 12);


        public GameScreen(IServiceProvider serviceProvider, Main game)
        {
            _serviceProvider = serviceProvider;
            _game = game;
            gb.Resume();
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Font = _game.Content.Load<SpriteFont>("Font");
            gb.Resume();

        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            UpdateInputs();
            gb.Update(gameTime);
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

            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            gb.Draw(_spriteBatch, Font);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
