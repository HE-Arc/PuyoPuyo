using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Grids;

namespace PuyoPuyo.GameObjects.Puyos
{
    public class Cell
    {
        #region Statics
        /// <summary>
        /// Create a new cell from given parameters
        /// <para>does NOT assign this new cell to the given grid !</para>
        /// <para>Throw exceptions</para>
        /// </summary>
        /// <param name="grid">Reference grid</param>
        /// <param name="row">row</param>
        /// <param name="column">col</param>
        /// <returns>A valid cell for the given grid</returns>
        public static Cell Create(Grid grid, int row, int column)
        {
            // Set grid
            if (grid is null)
                throw new System.ArgumentException("Grid can't be null");

            // Verifiy parameters
            if (column < 0 || row < 0 || column >= grid.Columns || row >= grid.Rows)
                throw new System.ArgumentException("Invalid dimensional parameters used");

            // Verifiy if cell exists
            if (grid[row, column] is Cell)
                throw new System.Exception(string.Format("Cell at [{0},{1}] already exists !", row, column));

            // Return new cell
            return new Cell(grid, row, column);
        }
        #endregion

        private readonly Grid grid;
        private readonly int row;
        private readonly int column;

        public Puyo Puyo { get; private set; }
        public bool IsFree => Puyo == null;
        public int Row => row;
        public int Column => column;
        
        private Cell(Grid grid, int row, int column)
        {
            this.grid = grid;
            this.row = row;
            this.column = column;
        }

        /// <summary>
        /// Insert the given puyo inside this cell
        /// </summary>
        /// <param name="puyo">Puyo to insert</param>
        /// <returns>False if this cell already has a puyo</returns>
        public bool Insert(Puyo puyo)
        {
            if(Puyo == null)
            {
                Puyo = puyo;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Release the current puyo
        /// </summary>
        /// <param name="puyo">Used to validate the release</param>
        /// <returns>False if the given puyo is not the same as the one in the cell</returns>
        public bool Release(Puyo puyo)
        {
            bool allowed = Puyo.ReferenceEquals(Puyo, puyo);
            if (allowed)
            {
                this.Puyo = null;
            }
            return allowed;
        }
    }
}