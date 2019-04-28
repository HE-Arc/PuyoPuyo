using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class PrepareForOneMenuScreen : MenuScreen
    {
        private readonly Main _main;

        public PrepareForOneMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Keyboard", PlayerKeyboard);
            AddMenuItem("GamePad", PlayerGamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);

            SetTitle("Player 1");
        }

        private void PlayerKeyboard()
        {
            Ready();
        }

        private void PlayerGamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.One);

            if (isSetted)
            {
                Ready();
            }
        }

        private void Back()
        {
            InputManager.Instance.Reset();

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", PlayerKeyboard);
            AddMenuItem("GamePad", PlayerGamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);
        }

        private void Ready()
        {
            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Play", StartGame);
            AddMenuItem("Back", Back);
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
                    case Input.Left:
                        break;
                    case Input.Down:
                        SelectNext();
                        break;
                    case Input.Right:
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

        public void StartGame()
        {
            InputManager.Instance.NbPlayer = 1;
            _main.setSize(false);
            Show<GameScreen>();
        }
    }
}
