using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PuyoPuyo.GameObjects.Gameboard
{
    public sealed class Gameboard : IUpdateable
    {
        // Puyo
        public PPuyo Player { get; private set; }
        public List<Puyo> Puyos { get; private set; }

        // Grid
        public Cell[,] Cells { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        // Interface
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        // Gamelogic
        public Stopwatch Watcher { get; private set; } = new Stopwatch();
        public int StepMS { get; set; }
        public uint Step { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">width of the grid</param>
        /// <param name="height">height of the grid</param>
        public Gameboard(int width, int height)
        {
            Width = width > 0 ? width : throw new ArgumentException("Invalid width");
            Height = height > 0 ? height : throw new ArgumentException("Invalid height");

            for (int col = 0; col < width; col++)
            {
                for(int row = 0; row < height; row++)
                {
                    Cells[col, row] = new Cell(col, row);
                }
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(Color color, int x, int y)
        {
            if (Player is null)
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    throw new ArgumentException("Invalid [x, y] given");

                // Create a new puyopuyo
                Player = new PPuyo(color, new Point(x, y), Toolbox.Orientation.Top);
            }
            else
            {
                throw new Exception("Can't create a new puyopuyo until previous one stop moving");
            }
        }

        public void Start()
        {
            Watcher.Start();
        }

        public void Pause()
        {
            Watcher.Stop();
        }

        public void Resume()
        {
            Watcher.Start();
        }

        public void Stop()
        {
            Watcher.Stop();
            Watcher.Reset();
        }

        public void Update(GameTime gameTime)
        {
            //FIXME: Verifiy position before moving
            //FIXME: Take in account speed increase with Space for animation too
            if (Watcher.ElapsedMilliseconds > this.StepMS)
            {
                Player.Down(StepMS);
                Watcher.Restart();
            }
            else
            {
                // Update animation
                Player.Update(gameTime);
            }
            throw new NotImplementedException();
        }
    }
}
