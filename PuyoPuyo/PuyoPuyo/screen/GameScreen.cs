using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.screen
{
    public class GameScreen : Screen
    {
        private readonly IServiceProvider _serviceProvider;
        private SpriteBatch _spriteBatch;
        private readonly Main _game;

        // TEST
        Texture2D texture;
        Vector2 position;
        Vector2 origin;

        public GameScreen(IServiceProvider serviceProvider, Main game)
        {

            _serviceProvider = serviceProvider;
            _game = game;
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);

            position = new Vector2(_game.graphics.GraphicsDevice.Viewport.Width / 2,
                _game.graphics.GraphicsDevice.Viewport.Height / 2);
            origin = new Vector2(128, 128);
            texture = _game.Content.Load<Texture2D>("textures/puyos/R");
        }

        public override void UnloadContent()
        {

        }

        public override void Update(GameTime gameTime)
        {
            UpdateTest();
        }

        private void UpdateTest()
        {
            KeyboardState kState = Keyboard.GetState();
            //var gamePadState = GamePad.GetState(PlayerIndex.One);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var key in kState.GetPressedKeys())
                sb.Append("Keys: ").Append(key).Append(" pressed");

            if (sb.Length > 0)
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            else
                System.Diagnostics.Debug.WriteLine("No Keys pressed");

            if (kState.IsKeyDown(Keys.Right))
                position.X += 10;
            if (kState.IsKeyDown(Keys.Left))
                position.X -= 10;
            if (kState.IsKeyDown(Keys.Up))
                position.Y -= 10;
            if (kState.IsKeyDown(Keys.Down))
                position.Y += 10;
        }

        public override void Draw(GameTime gameTime)
        {
            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, position, origin: origin);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
