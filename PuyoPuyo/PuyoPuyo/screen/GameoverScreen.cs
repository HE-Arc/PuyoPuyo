
using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class GameoverScreen : MenuScreen
    {
        private readonly Main _main;

        private int score;

        private MenuItem menuScore;

        public GameoverScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Retry", Show<GameScreen>);
            AddMenuItem("Home", Show<MainMenuScreen>);
            AddMenuItem("Exit", _main.Exit);

            menuScore = new MenuItem(Font, "Your score : " + score);
            menuScore.Position = new Vector2(50, 150);

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

            menuScore.Draw(_spriteBatch);

            _spriteBatch.End();
        }

        public void setScore(int score)
        {
            this.score = score;
            menuScore.Text = "Your score : " + this.score;
        }
    }
}
