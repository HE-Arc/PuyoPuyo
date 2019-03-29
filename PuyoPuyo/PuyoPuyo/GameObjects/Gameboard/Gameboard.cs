using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

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
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        // Interface
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        // Gamelogic
        public IGameLogic GameLogic { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="columns">columns of the grid</param>
        /// <param name="rows">rows of the grid</param>
        public Gameboard(int columns, int rows)
        {
            Columns = columns > 0 ? columns : throw new ArgumentException("Invalid width");
            Rows = rows > 0 ? rows : throw new ArgumentException("Invalid height");

            Cells = new Puyo[Rows, Columns];

            for (int col = 0; col < Columns; col++)
            {
                for(int row = 0; row < Rows; row++)
                {
                    Cells[row, col] = Puyo.Undefined;
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
        public void Spawn(Color color, int row, int column)
        {
            if (_playerAlive)
                throw new Exception("Can't spawn a new player until the previous one stopped moving");

            // Check that no puyo is in the cell
            if (GetPuyo(row, column, out Puyo puyo) && puyo == Puyo.Undefined)
            {
                // Create a new puyopuyo
                _playerOrientation = Orientation.Top;
                _playerX = row;
                _playerY = column;

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
                            return nextIndexSX < Columns && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Both
                        case Orientation.Down:
                        case Orientation.Top:
                            return nextIndexMX < Columns && nextIndexSX < Columns && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Right:
                            return nextIndexMX < Columns && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
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
                            return nextIndexMY < Rows && nextIndexSY < Rows && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        case Orientation.Top:
                            return nextIndexSY < Rows && Cells[nextIndexSX, nextIndexSY] == Puyo.Undefined;
                        // Master
                        case Orientation.Down:
                            return nextIndexMY < Rows && Cells[nextIndexMX, nextIndexMY] == Puyo.Undefined;
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
        /// <param name="column">column</param>
        /// <param name="row">row</param>
        /// <param name="puyo">out : color of the puyo</param>
        /// <returns>False if x,y are invalid</returns>
        private bool GetPuyo(int row, int column, out Puyo puyo)
        {
            if (column < 0 || row < 0 || column >= Columns || row >= Rows)
            {
                puyo = Puyo.Undefined;
                return false;
            }
            else
            {
                puyo = Cells[row, column];
                return true;
            }
        }

        /// <summary>
        /// Get the neighbors of a puyo at given coordinates
        /// </summary>
        /// <param name="column">column</param>
        /// <param name="row">row</param>
        /// <param name="puyo">out : color of the puyo</param>
        /// <returns>Every puyo of the same color in V4</returns>
        private List<Point> GetNeighbors(int row, int column)
        {
            // Get neighbors
            List<Point> neighbors = new List<Point>(4);

            if (GetPuyo(row, column, out Puyo puyoType))
            {
                if(!(puyoType == Puyo.Undefined))
                {
                    Point[] lrud = new Point[4]
                    {
                        new Point(row, column - 1),
                        new Point(row, column + 1),
                        new Point(row - 1, column),
                        new Point(row + 1, column)
                    };

                    foreach(Point p in lrud)
                    {
                        if (GetPuyo(p.X, p.Y, out Puyo neighbor))
                        {
                            if (!(neighbor == Puyo.Undefined) && neighbor == puyoType)
                            {
                                neighbors.Add(p);
                            }
                        }
                    }
                }
            }

            return neighbors;
        }

        public Dictionary<Puyo, Dictionary<int, List<Point>>> GetChains(out int[,] indexes)
        {
            Dictionary<Puyo, Dictionary<int, List<Point>>> pieces = new Dictionary<Puyo, Dictionary<int, List<Point>>>();
            foreach (Puyo puyo in Enum.GetValues(typeof(Puyo)))
            {
                pieces.Add(puyo, new Dictionary<int, List<Point>>());
            }

            int newIndex = 1;

            // Will contains indexes
            indexes = new int[Rows, Columns];

            for (int row = Rows; row >= 0 ; row--)
            {
                for (int col = 0; col < Columns; col++)
                {
                    if (GetPuyo(row, col, out Puyo puyoColor))
                    {
                        List<Point> neighbors = GetNeighbors(row, col);

                        // Not connected
                        if (neighbors.Count == 0)
                        {
                            // Add it as a new piece
                            pieces[puyoColor].Add(newIndex, new List<Point>()
                            {
                                new Point(row, col)
                            });

                            // Add it to the map
                            indexes[row, col] = newIndex;

                            // Increment piece index
                            newIndex++;
                        }
                        else
                        {
                            int minIndex = Int32.MaxValue;
                            int puyoIndex = indexes[row, col];

                            if (puyoIndex <= 0)
                            {
                                puyoIndex = newIndex;
                            }

                            // Search pieces
                            foreach (Point neighbor in neighbors)
                            {
                                int neighborIndex = indexes[neighbor.X, neighbor.Y];
                                if (neighborIndex > 0 && neighborIndex < minIndex)
                                    minIndex = indexes[neighbor.X, neighbor.Y];
                            }

                            // Replace puyoindex
                            if (minIndex < puyoIndex)
                            {
                                puyoIndex = minIndex;
                            }


                            // Insert the new puyo to the matching pieces
                            if (!pieces[puyoColor].ContainsKey(puyoIndex))
                            {
                                pieces[puyoColor].Add(puyoIndex, new List<Point>());
                                newIndex++;
                            }
                            pieces[puyoColor][puyoIndex].Add(new Point(row, col));

                            // Adapt index on the map
                            indexes[row, col] = puyoIndex;
                        }
                    }
                }
            }

            return pieces;
        }

        public void Update(GameTime gameTime)
        {
            //FIXME: Verifiy position before moving
            //FIXME: Take in account speed increase with Space for animation too

            throw new NotImplementedException();
        }

        public static bool Test()
        {
            Gameboard gb = new Gameboard(10, 20);

            Random r = new Random();

            // Fill tab
            for (int row = 0; row < gb.Rows; row++)
            {
                for (int col = 0; col < gb.Columns; col++)
                {
                    gb.Cells[row, col] = (Puyo)r.Next(0, 5);
                }
            }

            PrintArray(gb.Cells, gb.Rows, gb.Columns);

            var foo = gb.GetChains(out int[,] indexes);

            PrintArray(indexes, gb.Rows, gb.Columns);

            return true;
        }

        public static void PrintArray(Puyo[,] array, int rows, int columns)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    sb.Append(String.Format("| {0,-10} ", array[row, col].ToString()));
                }
                sb.Append("\r\n");
            }
            Console.WriteLine(sb.ToString());
        }

        public static void PrintArray(int[,] array, int rows, int columns)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    sb.Append(String.Format("| {0,-10} ", array[row, col].ToString()));
                }
                sb.Append("\r\n");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
