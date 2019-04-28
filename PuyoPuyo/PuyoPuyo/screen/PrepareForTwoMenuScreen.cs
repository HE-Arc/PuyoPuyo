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

            SetTitle("Player 1");
        }

        /// <summary>
        /// Set WASD to player 1
        /// </summary>
        private void player1KeyBoard()
        {
            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player2KeyBoard);
            AddMenuItem("GamePad", Player2GamePad);
            AddMenuItem("Back", BackToOne);

            SetTitle("Player 2");
        }

        /// <summary>
        /// Set gamepad to player 1
        /// </summary>
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

                SetTitle("Player 2");
            }
        }

        /// <summary>
        /// Set arrows to player 2
        /// </summary>
        private void player2KeyBoard()
        {
            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Play", Show<GameScreen>);
            AddMenuItem("Back", BackToTwo);

            SetTitle("Ready ?");
        }

        /// <summary>
        /// Set GamePad to player 2
        /// </summary>
        private void Player2GamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.Two);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Play", Show<GameScreen>);
                AddMenuItem("Back", BackToTwo);

                SetTitle("Ready ?");
            }
        }

        /// <summary>
        /// Return to choose input system to player 1
        /// </summary>
        private void BackToOne()
        {
            InputManager.Instance.RemovePlayer(PlayerIndex.One);

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player1KeyBoard);
            AddMenuItem("GamePad", Player1GamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);

            SetTitle("Player 1");
        }

        /// <summary>
        /// Return to choose input system to player 2
        /// </summary>
        private void BackToTwo()
        {
            InputManager.Instance.RemovePlayer(PlayerIndex.Two);

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("Keyboard", player2KeyBoard);
            AddMenuItem("GamePad", Player2GamePad);
            AddMenuItem("Back", BackToOne);

            SetTitle("Player 2");
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
