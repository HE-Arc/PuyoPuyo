using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos.Data;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.GameObjects.Puyos
{
    public class Puyo : IUpdateable, IDrawable
    {
        // Data
        public IPuyoData Data { get; private set; }
        public bool IsLinked { get; private set; } = false;

        // Physics
        public Vector2 Position { get; private set; } = Vector2.Zero;
        public Vector2 Speed { get; private set; } = Vector2.Zero;

        // Internal
        public bool Enabled { get; set; }
        public bool Visible { get; set; }

        // Internal ordering
        public int UpdateOrder => throw new NotImplementedException();
        public int DrawOrder => throw new NotImplementedException();

        /// <summary>
        /// Constructor of a Puyo
        /// </summary>
        /// <param name="data"></param>
        /// <param name="position"></param>
        public Puyo(IPuyoData data, Vector2 position)
        {
            Data = data;
            Position = position;
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
           
        public void Update(GameTime gameTime)
        {
            float __seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 __nextPosition = new Vector2(Position.X + Speed.X * __seconds, Position.Y + Speed.Y * __seconds);



            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
