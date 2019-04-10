using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using PuyoPuyo.GameObjects.Puyos.Data;
using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.GameObjects.Grids;
using System.Collections.Generic;

namespace PuyoPuyo.GameObjects.Puyos
{
    public class Puyo
    {
        private readonly Grid grid;
        private readonly IPuyoData data;

        public int Row { get; private set; }
        public int Column { get; private set; }

        /// <summary>
        /// Color of this puyo
        /// <para></para>
        /// </summary>
        public PuyoColor Color => data.Color;

        /// <summary>
        /// Texture of this puyo
        /// </summary>
        public Texture2D Texture => data.Texture;

        /// <summary>
        /// Build a new puyo
        /// <para>Throws exeception</para>
        /// </summary>
        /// <param name="grid">ref on the grid</param>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <param name="data">data of the puyo</param>
        public Puyo(Grid grid, int row, int column, IPuyoData data)
        {
            // Validate parameters
            if (grid == null || data == null)
                throw new System.ArgumentException("Invalid parameter provided");

            // Set data
            this.data = data;

            // Get the cell
            Cell c = grid[row, column];

            // Try to insert the puyo
            if (c == null || !c.Insert(this))
            {
                throw new Exceptions.PlayerException(Exceptions.PlayerException.OfType.SpawnError);
            }

            this.grid = grid;
            this.Row = row;
            this.Column = column;
        }

        /// <summary>
        /// Try to move a puyo to a cell
        /// </summary>
        /// <param name="cell"></param>
        /// <returns></returns>
        public bool Move(Cell cell)
        {
            if (cell == null || !cell.IsFree)
                return false;

            // Get the current cell
            Cell currentCell = grid[Row, Column];

            // Try to move this puyo
            if(cell.Insert(this))
            {
                // update row and column
                this.Row = cell.Row;
                this.Column = cell.Column;

                // release previous position
                currentCell.Release(this);
                return true;
            }

            // It failed
            return false;
        }

        /// <summary>
        /// Get the neighbors of this puyo
        /// <para>Neigbors are puyos with the same color as this one</para>
        /// </summary>
        /// <returns>Every PuyoColor of the same color in V4</returns>
        public List<Puyo> GetNeighbors()
        {
            // Get neighbors
            List<Puyo> neighbors = new List<Puyo>(4);

            Point[] lrud = new Point[4]
            {
                new Point(Row, Column - 1),
                new Point(Row, Column + 1),
                new Point(Row - 1, Column),
                new Point(Row + 1, Column)
            };

            foreach (Point p in lrud)
            {
                // Get the cell
                Cell temp_cell = grid[p.X, p.Y];

                // Test if cell exist and is occupied
                if (temp_cell == null || temp_cell.IsFree)
                    continue;
                else neighbors.Add(temp_cell.Puyo);
            }

            return neighbors;
        }

        public void Draw(SpriteBatch spriteBatch, int X, int Y, Vector2 Scale)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), origin: new Vector2(0, 0), scale: Scale);
        }
    }
}
