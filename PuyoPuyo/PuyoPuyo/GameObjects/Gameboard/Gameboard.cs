using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PuyoPuyo.GameObjects.Gameboard
{
    public sealed class Gameboard : IUpdateable
    {
        // Puyo
        private Orientation _playerOrientation;
        private bool _playerAlive = false;
        private int _playerX;
        private int _playerY;

        // Grid
        public Puyo[,] Cells { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        // Interface
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        // Gamelogic
        public IGameLogic GameLogic { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">width of the grid</param>
        /// <param name="height">height of the grid</param>
        public Gameboard(int width, int height)
        {
            Width = width > 0 ? width : throw new ArgumentException("Invalid width");
            Height = height > 0 ? height : throw new ArgumentException("Invalid height");

            for (int col = 0; col < width; col++)
            {
                for(int row = 0; row < height; row++)
                {
                    Cells[col, row] = Puyo.Undefined;
                }
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Returns the location of the slave
        /// </summary>
        /// <param name="x">x position of master</param>
        /// <param name="y">y position of master</param>
        /// <param name="orientation">orientation of master</param>
        /// <returns>x, y location of slave</returns>
        public static Point GetSlavePositionFromMaster(int x, int y, Orientation orientation)
        {
            switch (orientation)
            {
                case Orientation.Left:
                    return new Point(x - 1, y);
                case Orientation.Right:
                    return new Point(x + 1, y);
                case Orientation.Top:
                    return new Point(x, y - 1);
                case Orientation.Down:
                    return new Point(x, y + 1);
            }

            return Point.Zero;
        }

        /// <summary>
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(Color color, int x, int y)
        {
            if (_playerAlive)
                throw new Exception("Can't spawn a new player until the previous one stopped moving");

            // Check that no puyo is in the cell
            if (GetPuyo(x, y, out Puyo puyo) && puyo == Puyo.Undefined)
            {
                // Create a new puyopuyo
                _playerOrientation = Orientation.Top;
                _playerX = x;
                _playerY = y;

                // Set the player alive
                _playerAlive = true;
            }
            else
            {
                throw new Exception("[x, y] already in use");
            }
        }

        private bool ValidateNextMove(Orientation direction)
        {

            // Those will be updated
            int nextIndexMX = _playerX;
            int nextIndexMY = _playerY;
            var slaveLocation = GetSlavePositionFromMaster(_playerX, _playerY, _playerOrientation);
            int nextIndexSX = slaveLocation.X;
            int nextIndexSY = slaveLocation.Y;

            switch (direction)
            {
                // Player going left
                case Orientation.Left:

                    nextIndexMX = nextIndexMX - 1;
                    nextIndexSX = nextIndexSX - 1;

                    switch (_playerOrientation)
                    {
                        // Slave
                        case Orientation.Left:
                            return nextIndexSX >= 0 && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Both
                        case Orientation.Down:
                        case Orientation.Top:
                            return nextIndexMX >= 0 && nextIndexSX >= 0 && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Right:
                            return nextIndexMX >= 0 && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
                        default:
                            throw new Exception("Invalid direction provided");
                    }

                // Player going right
                case Orientation.Right:
                    nextIndexMX = nextIndexMX + 1;
                    nextIndexSX = nextIndexSX + 1;

                    switch (direction)
                    {
                        // Slave
                        case Orientation.Left:
                            return nextIndexSX < Width && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Both
                        case Orientation.Down:
                        case Orientation.Top:
                            return nextIndexMX < Width && nextIndexSX < Width && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Right:
                            return nextIndexMX < Width && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
                        default:
                            throw new Exception("Invalid direction provided");
                    }

                // Player going up
                case Orientation.Top:

                    nextIndexMY = nextIndexMY - 1;
                    nextIndexSY = nextIndexSY - 1;

                    switch (direction)
                    {
                        case Orientation.Left:
                        case Orientation.Right:
                            return nextIndexMY >= 0 && nextIndexSY >= 0 && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        case Orientation.Top:
                            return nextIndexSY >= 0 && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Down:
                            return nextIndexMY >= 0 && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
                        default:
                            throw new Exception("Invalid direction provided");
                    }

                // Player going up
                case Orientation.Down:

                    nextIndexMY = nextIndexMY + 1;
                    nextIndexSY = nextIndexSY + 1;

                    switch (direction)
                    {
                        case Orientation.Left:
                        case Orientation.Right:
                            return nextIndexMY < Height && nextIndexSY < Height && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        case Orientation.Top:
                            return nextIndexSY < Height && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Down:
                            return nextIndexMY < Height && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
                        default:
                            throw new Exception("Invalid direction provided");
                    }
            }

            return false;
        }

        private void CheckPuyos()
        {

        }

        /// <summary>
        /// Get the color of a puyo at given coordinates
        /// </summary>
        /// <param name="x">column</param>
        /// <param name="y">row</param>
        /// <param name="puyo">out : color of the puyo</param>
        /// <returns>False if x,y are invalid</returns>
        private bool GetPuyo(int x, int y, out Puyo puyo)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                puyo = Puyo.Undefined;
                return false;
            }
            else
            {
                puyo = Cells[x, y];
                return true;
            }
        }

        private ISet<Point> BuildChain(int x, int y)
        {
            HashSet<Point> chain = new HashSet<Point>();
            return GetNeighbors(x, y, chain);
        }

        private ISet<Point> GetNeighbors(int x, int y, ISet<Point> set)
        {
            // Get neighbors
            List<Point> neighbors = new List<Point>(4);

            if (GetPuyo(x, y, out Puyo puyoType))
            {
                if(!(puyoType == Puyo.Undefined))
                {
                    for(int col = 0; col < 2; col++)
                    {
                        for (int row = 0; row < 2; row++)
                        {
                            Point newPoint = new Point(x + col, y + row);
                            if (GetPuyo(newPoint.X, newPoint.Y, out Puyo neighbor))
                            {
                                if (!(neighbor == Puyo.Undefined) && neighbor == puyoType && !set.Contains(newPoint))
                                {
                                    neighbors.Add(newPoint);
                                }
                            }
                        }
                    }

                    // return if no new puyos
                    if (neighbors.Count == 0)
                        return set;

                    // Add new puyos to the set
                    set.UnionWith(neighbors);

                    // Search from children
                    foreach(Point puyo in neighbors)
                    {
                        GetNeighbors(puyo.X, puyo.Y, set);
                    }
                }
            }

            return set;
        }

        public void Update(GameTime gameTime)
        {
            //FIXME: Verifiy position before moving
            //FIXME: Take in account speed increase with Space for animation too

            throw new NotImplementedException();
        }
    }
}
