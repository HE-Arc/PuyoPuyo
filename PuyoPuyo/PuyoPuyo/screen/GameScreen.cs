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

        public virtual void LoadContent()
        {
            position = new Vector2(_game.GraphicsDevice.Viewport.Width / 2,
                _game.GraphicsDevice.Viewport.Height / 2);
            origin = new Vector2(128, 128);
            texture = _game.Content.Load<Texture2D>("");
        }

        public virtual void UnloadContent()
        {

        }

        public virtual void Update(GameTime gameTime)
        {
            
        }

        private void UpdateTest()
        {
            KeyboardState kState = Keyboard.GetState();

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var key in kState.GetPressedKeys())
                sb.Append("Keys: ").Append(key).Append(" pressed");

            if (sb.Length > 0)
                System.Diagnostics.Debug.WriteLine(sb.ToString());
            else
                System.Diagnostics.Debug.WriteLine("No Keys pressed");

            //if (kState.IsKeyDown(Keys.Right))


        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
