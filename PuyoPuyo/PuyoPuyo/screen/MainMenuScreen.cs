using System;

namespace PuyoPuyo.screen
{
    public class MainMenuScreen : MenuScreen
    {
        private readonly MainGame _game;

        public MainMenuScreen(IServiceProvider serviceProvider, MainGame game)
            : base(serviceProvider)
        {
            _game = game;
            game.IsMouseVisible = true;
        }

        public override void LoadContent()
        {
            base.LoadContent();

            AddMenuItem("New Game", Show<GameScreen>);
            //AddMenuItem("Load Game", Show<LoadGameScreen>);
            //AddMenuItem("Options", Show<OptionsScreen>);
            AddMenuItem("Exit", _game.Exit);
        }
    }
}
