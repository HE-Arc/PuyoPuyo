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

        private int row;
        private int column;

        /// <summary>
        /// <para>Get : Get the current row</para>
        /// <para>Set : Free current cell and insert into the new cell according to given value</para>
        /// </summary>
        public int Row
        {
            get
            {
                return row;
            }
            set
            {
                Cell c = grid[value, column];
                this.TryMoveToCell(c);
            }
        }

        /// <summary>
        /// <para>Get : Get the current column</para>
        /// <para>Set : Free current cell and insert into the new cell according to given value</para>
        /// </summary>
        public int Column
        {
            get
            {
                return column;
            }
            set
            {
                Cell c = grid[row, value];
                this.TryMoveToCell(c);
            }
        }

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
            if (grid is null || data is null)
                throw new System.ArgumentException("Invalid parameter provided");

            // Set data
            this.data = data;

            // Get the cell
            Cell c = grid[row, column];

            // Try to insert the puyo
            if (c is null || !c.Insert(this))
            {
                throw new Exceptions.PlayerException(Exceptions.PlayerException.OfType.SpawnError);
            }

            this.grid = grid;
            this.row = row;
            this.column = column;
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
                new Point(row, column - 1),
                new Point(row, column + 1),
                new Point(row - 1, column),
                new Point(row + 1, column)
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

        /// <summary>
        /// Move this puyo to the provided cell
        /// </summary>
        /// <param name="cell">where to teleport this puyo</param>
        /// <returns>true if it succeded</returns>
        public bool TryMoveToCell(Cell cell)
        {
            if (!(cell is null) && cell.IsFree)
            {
                grid[row, column].Release(this);
                cell.Insert(this);

                // Update row and column
                this.row = cell.Row;
                this.column = cell.Column;

                return true;
            }

            return false;
        }

        public void Draw(SpriteBatch spriteBatch, int X, int Y, Vector2 Scale)
        {
            spriteBatch.Draw(Texture, new Vector2(X, Y), origin: new Vector2(0, 0), scale: Scale);
        }
    }
}
