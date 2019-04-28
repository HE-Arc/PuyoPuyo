
using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class GameoverScreen : MenuScreen
    {
        private readonly Main _main;

        private int scorePlayer1;
        private int scorePlayer2;

        private MenuItem menuScorePlayer1;
        private MenuItem menuScorePlayer2;

        public GameoverScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Retry", RestartGame);
            AddMenuItem("Home", ReturnToHome);
            AddMenuItem("Exit", _main.Exit);

            menuScorePlayer1 = new MenuItem(Font, "");
            menuScorePlayer1.Position = new Vector2(50, 150);

            menuScorePlayer2 = new MenuItem(Font, "");
            menuScorePlayer2.Position = new Vector2(50, 200);

            SetTitle("Game Over");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            List<Input> inputs = InputManager.Instance.Perform();

            foreach (Input input in inputs)
            {
                switch (input)
                {
                    case Input.Up:
                        SelectPrevious();
                        break;
                    case Input.Down:
                        SelectNext();
                        break;
                    case Input.Validate:
                        selectedItem.Action?.Invoke();
                        break;
                    case Input.Cancel:
                        _main.Exit();
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

            menuScorePlayer1.Draw(_spriteBatch);
            menuScorePlayer2.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        public void setScorePlayer1(int scorePlayer1)
        {
            this.scorePlayer1 = scorePlayer1;
            menuScorePlayer1.Text = "Score Player 1 : " + this.scorePlayer1;
        }

        public void setScorePlayer2(int scorePlayer2)
        {
            this.scorePlayer2 = scorePlayer2;
            menuScorePlayer2.Text = "Score Player 2 : " + this.scorePlayer2;
        }

        public void ReturnToHome()
        {
            menuScorePlayer1.Text = "";
            menuScorePlayer2.Text = "";
            Show<MainMenuScreen>();
        }

        public void RestartGame()
        {
            if (InputManager.Instance.NbPlayer >= 2)
                _main.setSize(true);

            Show<GameScreen>();
        }
    }
}
