using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects.Grids
{
    public class Grid
    {
        private readonly int rows;
        private readonly int columns;
        private readonly Cell[,] cells;

        public int Rows => rows;
        public int Columns => columns;

        /// <summary>
        /// Build a grid
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Grid(int rows, int columns)
        {
            // Logic elements init
            this.rows = rows > 0 ? rows : throw new ArgumentException("Invalid row count");
            this.columns = columns > 0 ? columns : throw new ArgumentException("Invalid column count");

            // Create the grid
            cells = new Cell[Rows, Columns];

            // Row-major
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    cells[row, col] = Cell.Create(this, row, col);
                }
            }
        }

        /// <summary>
        /// Give access to cells
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <returns>null if out of range</returns>
        public Cell this[int row, int column]
        {
            get {
                if (column < 0 || row < 0 || column >= Columns || row >= Rows)
                {
                    return null;
                }
                else
                {
                    return cells[row, column];
                }   
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    yield return cells[row, col];
                }
            }
        }
    }
}
