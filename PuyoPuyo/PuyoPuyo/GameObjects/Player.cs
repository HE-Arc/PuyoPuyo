using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects
{
    public class Player
    {
        private static Random random = new Random();

        private Point posMaster;
        private Orientation orientation;

        public bool Alive { get; set; } 
        public Orientation Orientation { get
            {
                return orientation;
            }
            set
            {
                orientation = value;
                Slave = GetSlavePositionFromMaster();
            }
        }
        public Puyo Color { get; private set; }
        public Point Master
        {
            get
            {
                return posMaster;
            }
            set
            {
                posMaster = value;
                Slave = GetSlavePositionFromMaster();
            }
        }
        public Point Slave { get; private set; }

        public Player(Gameboard gameboard, Puyo color)
        {
            Alive = true;
            Orientation = Orientation.Up;
            Color = color != Puyo.Undefined ? color : throw new ArgumentException("Invalid puyo (color) given");

            // Create new random spawn point
            Point p = new Point(0, random.Next(0, gameboard.Columns));

            // Check that no puyo is in the cell
            if (gameboard.GetPuyo(p.X, p.Y, out Puyo puyo) && puyo == Puyo.Undefined)
            {
                Master = p;
                Slave = GetSlavePositionFromMaster();
            }
            else
            {
                throw new ArgumentException("[x, y] already in use");
            }
        }

        /// <summary>
        /// Returns the location of the slave
        /// </summary>
        /// <returns>x, y location of slave</returns>
        public Point GetSlavePositionFromMaster()
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    return new Point(Master.X, Master.Y - 1);
                case Orientation.Right:
                    return new Point(Master.X, Master.Y + 1);
                case Orientation.Up:
                    return new Point(Master.X - 1, Master.Y);
                case Orientation.Down:
                    return new Point(Master.X + 1, Master.Y);
            }

            return Point.Zero;
        }
    }
}
