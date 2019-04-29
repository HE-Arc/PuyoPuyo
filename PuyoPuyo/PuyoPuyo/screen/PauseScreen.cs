
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.screen
{
    class PauseScreen : MenuScreen
    {

        public PauseScreen(IServiceProvider serviceProvider, Main main)
                : base(serviceProvider, main)
        {


        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Resume", Show<GameScreen>);
            AddMenuItem("Quit", Quit);

            SetTitle("Pause");
        }

        private void Quit()
        {
            FindScreen<GameScreen>().ResetBoard();
            InputManager.Instance.Reset();
            _main.setSize(false);
            Show<MainMenuScreen>();
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.GraphicsDevice.Clear(Color.TransparentBlack);

            base.Draw(gameTime);
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
                    case Input.Pause:
                        Show<GameScreen>();
                        break;
                    case Input.Validate:
                        selectedItem.Action?.Invoke();
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
    }
}
