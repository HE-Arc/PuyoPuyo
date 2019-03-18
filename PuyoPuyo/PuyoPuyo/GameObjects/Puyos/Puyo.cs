using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos.Data;
using System;

namespace PuyoPuyo.GameObjects.Puyos
{
    public class Puyo : IUpdateable, IDrawable
    {
        // Data
        public IPuyoData Data { get; private set; }
        public bool IsLinked { get; private set; } = false;

        // Animation
        private int nextX = 0;
        private int nextY = 0;
        private DateTime nextMove;
        private double animationMS;

        // Physics
        public int X { get; private set; } = 0;
        public int Y { get; private set; } = 0;
        public Point Position => new Point(X, Y);
        public double Lerp { get; private set; } = 0;

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
        public Puyo(IPuyoData data, Point position)
        {
            Data = data;
            X = nextX = position.X;
            Y = nextY = position.Y;
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;
        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;

        // Mouve Puyo
        public void Left(double ms) { nextX--; Animate(ms); }
        public void Right(double ms) { nextX++; Animate(ms); }
        public void Top(double ms) { nextY--; Animate(ms); }
        public void Down(double ms) { nextY++; Animate(ms); }

        private void Animate(double ms)
        {
            // Prevent an animation if one is already started
            if (animationMS > 0)
                return;

            animationMS = ms;
            nextMove = DateTime.Now;
        }

        private void UpdateAnimation()
        {
            // Lerp (used in drawing)
            Lerp = (DateTime.Now - nextMove).TotalMilliseconds / animationMS;

            // Move to next cell
            if (Lerp >= 1)
            {
                X = nextX;
                Y = nextY;
                animationMS = 0;
            }
        }

        public void Update(GameTime gameTime)
        {
            UpdateAnimation();

            throw new NotImplementedException();
        }

        public void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }
}
