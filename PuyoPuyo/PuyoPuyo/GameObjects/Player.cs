using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Grids;
using PuyoPuyo.GameObjects.Puyos;
using PuyoPuyo.GameObjects.Puyos.Data;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects
{
    /// <summary>
    /// Ease the control of the puyopuyo controlled by the player
    /// <para>If the player is broken in half, please nullify this</para>
    /// </summary>
    public class Player
    {
        private readonly Grid grid;

        /// <summary>
        /// Orientation of this puyopuyo
        /// </summary>
        public Orientation Orientation { get; private set; }

        /// <summary>
        /// Color of this puyopuyo
        /// </summary>
        public PuyoColor Color => Master.Color;

        /// <summary>
        /// Center of rotation
        /// </summary>
        public Puyo Master { get; private set; }

        /// <summary>
        /// Second puyo of this puyopuyo
        /// </summary>
        public Puyo Slave { get; private set; }
        
        /// <summary>
        /// Build a puyopuyo
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="color"></param>
        /// <param name="orientation"></param>
        public Player(Grid grid, PuyoColor color, Orientation orientation = Orientation.Up)
        {
            // Keep track of the grid
            this.grid = grid;

            // Set orientation
            Orientation = orientation;

            // Set color
            if(color == PuyoColor.Undefined)
                throw new ArgumentException("Invalid puyo (color) given");

            // Get half columns count
            int middle = grid.Columns / 2;

            // Get cell for slave
            Cell cell_s = grid[0, middle];

            // Get cell for master
            Cell cell_m = grid[1, middle];

            // Check that no puyo is in the cell
            if (cell_m.IsFree && cell_s.IsFree)
            {
                // Get puyo data
                IPuyoData data = PuyoDataFactory.Instance.Get(color);

                // Create puyos
                Master = new Puyo(grid, 1, middle, data);
                Slave = new Puyo(grid, 0, middle, data);
            }
            else
            {
                throw new Exceptions.PlayerException(Exceptions.PlayerException.OfType.SpawnError);
            }
        }

        #region Private functions
        private bool ValidateSlavePosition()
        {
            GetSlaveRowAndColumnFromOrientation(out int predict_row, out int predict_column);
            return Slave.Row == predict_row && Slave.Column == predict_column;        
        }

        /// <summary>
        /// Returns the row and column of the slave
        /// </summary>
        private void GetSlaveRowAndColumnFromOrientation(out int row, out int column)
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    row = Master.Row;
                    column = Master.Column - 1;
                    break;
                case Orientation.Right:
                    row = Master.Row;
                    column = Master.Column + 1;
                    break;
                case Orientation.Up:
                    row = Master.Row - 1;
                    column = Master.Column;
                    break;
                case Orientation.Down:
                    row = Master.Row + 1;
                    column = Master.Column;
                    break;
                default:
                    row = column = -1;
                    break;
            }
        }
        private bool ValidateMove(Cell current_m, Cell current_s, Cell next_m, Cell next_s)
        {
            // Move and validate
            if (Master.TryMoveToCell(next_m) && Slave.TryMoveToCell(next_s) && ValidateSlavePosition())
            {
                return true;
            }
            else
            {
                // Reset positions
                Master.TryMoveToCell(current_m);
                Slave.TryMoveToCell(current_s);

                return false;
            }
        }
        /// <summary>
        /// Update slave position according to given orientation
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns>true if it succeded</returns>
        private bool UpdateSlaveFromOrientation(Orientation orientation)
        {
            GetSlaveRowAndColumnFromOrientation(out int predict_row, out int predict_column);
            Cell slave_new_cell = grid[predict_row, predict_column];
            return Slave.TryMoveToCell(slave_new_cell);
        }
        #endregion

        #region Controls
        /// <summary>
        /// Rotate this puyopuyo
        /// </summary>
        /// <param name="rotation">(counter)clockwisely</param>
        /// <returns>true if it succeded</returns>
        public bool Rotate(Rotation rotation)
        {
            Orientation orientation = OrientationHandler.Next(this.Orientation, rotation);
            if (UpdateSlaveFromOrientation(orientation))
            {
                Orientation = orientation;
                return true;
            }

            return false;
        }

        public bool Left()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row, Master.Column - 1];
            Cell next_s = grid[Slave.Row, Slave.Column - 1];

            // Move and validate
            return ValidateMove(current_m, current_s, next_m, next_s);
        }

        public bool Right()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row, Master.Column + 1];
            Cell next_s = grid[Slave.Row, Slave.Column + 1];

            // Move and validate
            return ValidateMove(current_m, current_s, next_m, next_s);
        }

        public bool Down()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row + 1, Master.Column];
            Cell next_s = grid[Slave.Row + 1, Slave.Column];

            // Move and validate
            return ValidateMove(current_m, current_s, next_m, next_s);
        }
        #endregion
    }
}
