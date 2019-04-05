using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.screen
{
    public class PrepareForOneMenuScreen : MenuScreen
    {
        private readonly Main _main;
        public bool IsTwoPlayer { get; set; }

        public PrepareForOneMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            main.IsMouseVisible = true;
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();
            AddMenuItem("Print", this.ConsolePrint);
            AddMenuItem("Play", Show<GameScreen>);
            AddMenuItem("Back", Show<MainMenuScreen>);
        }

        private void ConsolePrint()
        {
            Console.WriteLine("pika");
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
