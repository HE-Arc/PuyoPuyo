
using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class GameoverScreen : MenuScreen
    {
        private readonly Main _main;

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
    }
}
