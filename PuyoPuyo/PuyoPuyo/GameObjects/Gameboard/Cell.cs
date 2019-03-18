﻿using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos;

namespace PuyoPuyo.GameObjects.Gameboard
{
    public sealed class Cell
    {
        public static int SIZE = Properties.PuyoPuyo.Default.CELL_SIZE;

        public int X { get; private set; }
        public int Y { get; private set; }

        public bool IsUsed { get; private set; }
        public Puyo Puyo { get; private set; }

        public Cell(int column, int row)
        {
            X = column;
            Y = row;
            IsUsed = false;
            Puyo = null;
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {

        }
    }
}
