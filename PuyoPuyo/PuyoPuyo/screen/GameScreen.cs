using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Screens;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Tests;
using PuyoPuyo.Toolbox;
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
        private InputManager im;
        private bool puyoTest = true;
        protected SpriteFont Font { get; private set; }

        // GameBoard Test
        Gameboard gb = new Gameboard(6, 12);


        public GameScreen(IServiceProvider serviceProvider, Main game)
        {
            _serviceProvider = serviceProvider;
            _game = game;
            im = new InputManager();
            
            
            //gb.Spawn(PuyoColor.Yellow);
            //gb.GetChains(out int[,] foo);
        }

        public override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(_game.GraphicsDevice);
            Font = _game.Content.Load<SpriteFont>("Font");
            gb.Resume();
            
        }

        public override void UnloadContent()
        {

        }
       
        public override void Update(GameTime gameTime)
        {
            if (puyoTest)
            {
                Console.WriteLine("pika");
                gb.Spawn(PuyoColor.Yellow);
                gb.GetChains(out int[,] foo, out int chainCount);
                puyoTest = false;
            }

            gb.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (_spriteBatch == null)
                return;

            _game.GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            gb.Draw(_spriteBatch, Font);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
