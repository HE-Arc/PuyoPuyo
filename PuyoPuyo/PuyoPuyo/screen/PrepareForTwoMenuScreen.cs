using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class PrepareForTwoMenuScreen : MenuScreen
    {
        private readonly Main _main;

        public PrepareForTwoMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            InputManager.Instance.NbPlayer = 2;

            AddMenuItem("Keyboard", player1KeyBoard);
            AddMenuItem("GamePad", Player1GamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);
        }

        private void player1KeyBoard()
        {
            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player2KeyBoard);
            AddMenuItem("GamePad", Player2GamePad);
            AddMenuItem("Back", BackToOne);
        }

        private void Player1GamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.One);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Keyboard", player2KeyBoard);
                AddMenuItem("GamePad", Player2GamePad);
                AddMenuItem("Back", BackToOne);
            }
        }

        private void player2KeyBoard()
        {
            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Play", Show<GameScreen>);
            AddMenuItem("Back", BackToTwo);
        }

        private void Player2GamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.Two);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Play", Show<GameScreen>);
                AddMenuItem("Back", BackToTwo);
            }
        }

        private void BackToOne()
        {
            InputManager.Instance.RemovePlayer(PlayerIndex.One);

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player1KeyBoard);
            AddMenuItem("GamePad", Player1GamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);
        }

        private void BackToTwo()
        {
            InputManager.Instance.RemovePlayer(PlayerIndex.Two);

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player2KeyBoard);
            AddMenuItem("GamePad", Player2GamePad);
            AddMenuItem("Back", BackToOne);
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
    }
}
