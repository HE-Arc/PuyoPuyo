using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using PuyoPuyo.GameObjects.Puyos.Data;
using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.GameObjects.Grids;

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
            }

            return false;
        }
    }
}
