using PuyoPuyo.GameObjects.Puyos;
using System.Collections.Generic;

namespace PuyoPuyo.GameObjects.Gameboard
{
    public class Gameboard
    {
        public PPuyo Player { get; private set; }
        public List<Puyo> Puyos { get; private set; }

        public Cell[,] Cells { get; private set; }

        public Gameboard(int width, int height)
        {
            for(int col = 0; col < width; col++)
            {
                for(int row = 0; row < height; row++)
                {
                    Cells[col, row] = new Cell(col, row);
                }
            }
        }
    }
}
