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
        public Player(Grid grid, Tuple<PuyoColor, PuyoColor> colors, Orientation orientation = Orientation.Up)
        {
            // Keep track of the grid
            this.grid = grid;

            // Set orientation
            Orientation = orientation;

            // Set color
            if(colors.Item1 == PuyoColor.Undefined || colors.Item2 == PuyoColor.Undefined)
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
                IPuyoData master_data = PuyoDataFactory.Instance.Get(colors.Item1);
                IPuyoData slave_data = PuyoDataFactory.Instance.Get(colors.Item2);

                // Create puyos
                Master = new Puyo(grid, 1, middle, master_data);
                Slave = new Puyo(grid, 0, middle, slave_data);
            }
            else throw new Exceptions.PlayerException(Exceptions.PlayerException.OfType.SpawnError);
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

        /// <summary>
        /// Update slave position according to given orientation
        /// </summary>
        /// <returns>true if it succeded</returns>
        private bool UpdateSlaveFromOrientation()
        {
            GetSlaveRowAndColumnFromOrientation(out int predict_row, out int predict_column);
            Cell slave_new_cell = grid[predict_row, predict_column];

            return Slave.Move(slave_new_cell);
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
            Orientation previousOrientation = Orientation;
            Orientation = OrientationHandler.Next(this.Orientation, rotation);
            if (UpdateSlaveFromOrientation())
            {
                return true;
            }
            else
            {
                // If failed rotate it back
                Orientation = previousOrientation;
                return false;
            }
        }

        public bool Left()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row, Master.Column - 1];
            Cell next_s = grid[Slave.Row, Slave.Column - 1];

            // Move and validate
            if (Orientation == Orientation.Left)
            {
                return Slave.Move(next_s) & Master.Move(next_m);
            }
            else
            {
                return Master.Move(next_m) & Slave.Move(next_s);
            }
        }

        public bool Right()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row, Master.Column + 1];
            Cell next_s = grid[Slave.Row, Slave.Column + 1];

            // Move and validate
            if (Orientation == Orientation.Right)
            {
                return Slave.Move(next_s) && Master.Move(next_m) && ValidateSlavePosition();
            }
            else
            {
                return Master.Move(next_m) && Slave.Move(next_s) && ValidateSlavePosition();
            }
        }

        public bool Down()
        {
            // Used for rollback
            Cell current_m = grid[Master.Row, Master.Column];
            Cell current_s = grid[Slave.Row, Slave.Column];

            Cell next_m = grid[Master.Row + 1, Master.Column];
            Cell next_s = grid[Slave.Row + 1, Slave.Column];

            // Move and validate
            if (Orientation == Orientation.Down)
            {
                return Slave.Move(next_s) && Master.Move(next_m) && ValidateSlavePosition();
            }
            else
            {
                return Master.Move(next_m) && Slave.Move(next_s) && ValidateSlavePosition();
            }
        }
        #endregion
    }
}
