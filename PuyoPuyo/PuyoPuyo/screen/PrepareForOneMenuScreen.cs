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
        private bool player1UseGamePad = false;
        private List<PlayerIndex> players = new List<PlayerIndex>();

        public PrepareForOneMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            players.Add(PlayerIndex.One);
            players.Add(PlayerIndex.Two);

            AddMenuItem("WASD", Player1WASD);
            AddMenuItem("Arrows", Player1Arrows);
            AddMenuItem("GamePad", Player1GamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);
        }

        private void Player1WASD()
        {
            bool isSetted = InputManager.Instance.SetKeyBoard(PlayerIndex.One, PlayerIndex.One);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Arrows", Player2Arrows);
                AddMenuItem("GamePad", Player2GamePad);
                AddMenuItem("Back", Back);
            }
        }

        private void Player1Arrows()
        {
            bool isSetted = InputManager.Instance.SetKeyBoard(PlayerIndex.One, PlayerIndex.Two);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("WASD", Player2WASD);
                AddMenuItem("GamePad", Player2GamePad);
                AddMenuItem("Back", Back);
            }
        }

        private void Player1GamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.One);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("WASD", Player2WASD);
                AddMenuItem("Arrows", Player2Arrows);
                AddMenuItem("Back", Back);
            }
        }

        private void Player2WASD()
        {
            bool isSetted = InputManager.Instance.SetKeyBoard(PlayerIndex.Two, PlayerIndex.One);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Play", Show<GameScreen>);
                AddMenuItem("Back", Back);
            }
        }

        private void Player2Arrows()
        {
            bool isSetted = InputManager.Instance.SetKeyBoard(PlayerIndex.Two, PlayerIndex.Two);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Play", Show<GameScreen>);
                AddMenuItem("Back", Back);
            }
        }

        private void Player2GamePad()
        {
            bool isSetted = InputManager.Instance.SetGamePad(PlayerIndex.Two);

            if (isSetted)
            {
                MenuItems.Clear();
                indexMenu = 0;

                AddMenuItem("Play", Show<GameScreen>);
                AddMenuItem("Back", Back);
            }
        }

        private void Back()
        {
            foreach (PlayerIndex player in players)
            {
                InputManager.Instance.RemovePlayer(player);
            }

            MenuItems.Clear();
            indexMenu = 0;

            AddMenuItem("WASD", Player1WASD);
            AddMenuItem("Arrows", Player1Arrows);
            AddMenuItem("GamePad", Player1GamePad);
            AddMenuItem("Back", Show<MainMenuScreen>);
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
