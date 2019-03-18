using System;

namespace PuyoPuyo.screen
{
    public class MainMenuScreen : MenuScreen
    {
        private readonly Main _main;

        public MainMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            main.IsMouseVisible = true;
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("New Game", Show<GameScreen>);
            AddMenuItem("Tutorial", Show<TutorialScreen>);
            //AddMenuItem("Options", Show<OptionsScreen>);
            AddMenuItem("Exit", _main.Exit);
        }
    }
}
