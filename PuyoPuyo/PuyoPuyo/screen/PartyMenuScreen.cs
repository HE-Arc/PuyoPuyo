using System;

namespace PuyoPuyo.screen
{
    public class PartyMenuScreen : MenuScreen
    {
        private readonly Main _main;

        public PartyMenuScreen(IServiceProvider serviceProvider, Main main)
            : base(serviceProvider, main)
        {
            main.IsMouseVisible = true;
            _main = main;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("Play", Show<GameScreen>);
            AddMenuItem("Two Players", Show<TutorialScreen>);
            //AddMenuItem("Options", Show<OptionsScreen>);
            AddMenuItem("Exit", _main.Exit);
        }
    }
}
