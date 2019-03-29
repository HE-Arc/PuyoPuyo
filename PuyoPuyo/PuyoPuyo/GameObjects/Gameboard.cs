using Microsoft.Xna.Framework;
using PuyoPuyo.Exceptions;
using PuyoPuyo.GameObjects.Puyos;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PuyoPuyo.GameObjects
{
    public sealed class Gameboard : IUpdateable
    {
        // Puyo
        public Player Player { get; private set; }

        // Grid
        public Puyo[,] Cells { get; private set; }
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        // Interface
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        /// <summary>
        /// Help managing puyos.
        /// <para/> Row-major
        /// </summary>
        /// <param name="columns">columns of the grid</param>
        /// <param name="rows">rows of the grid</param>
        public Gameboard(int columns, int rows)
        {
            Columns = columns > 0 ? columns : throw new ArgumentException("Invalid column count");
            Rows = rows > 0 ? rows : throw new ArgumentException("Invalid row count");

            Cells = new Puyo[Rows, Columns];

            for (int row = 0; row < Rows; row++)  
            {
                for (int col = 0; col < Columns; col++)
                {
                    Cells[row, col] = Puyo.Undefined;
                }
            }
        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(Color color)
        {
            if (Player is null || !Player.Alive)
            {
                Player = new Player(this, color);
            }
            else throw new PlayerException(PlayerException.OfType.SpawnError);
        }

        private bool Move(Orientation direction)
        {
            if (Player is null)
                throw new PlayerException(PlayerException.OfType.NotCreated);

            if (Player.Alive)
            {
                if (ValidateNextMove(direction, out Point npm, out Point nps))
                {
                    Player.Master = npm;
                    Player.Slave = nps;
                    return true;
                }
                else throw new PlayerException(PlayerException.OfType.NotAlive);
            }

            return false;
        }

        public bool Left()
        {
            return Move(Orientation.Left);
        }
        public bool Right()
        {
            return Move(Orientation.Right);
        }
        public bool Up()
        {
            return Move(Orientation.Up);
        }
        public bool Down()
        {
            return Move(Orientation.Down);
        }


        /// <summary>
        /// Validate the next move and return both position
        /// </summary>
        /// <param name="direction">Where the puyo is heading</param>
        /// <param name="npm">New position of Master</param>
        /// <param name="nps">New position of Slave</param>
        /// <returns></returns>
        public bool ValidateNextMove(Orientation direction, out Point npm, out Point nps)
        {
            if (Player != null && Player.Alive)
            {
                // Those will be updated
                int nextIndexMR = Player.Master.X;
                int nextIndexMC = Player.Master.Y;
                int nextIndexSR = Player.Slave.X;
                int nextIndexSC = Player.Slave.Y;

                switch (direction)
                {
                    // Player going left
                    case Orientation.Left:

                        nextIndexMR = nextIndexMR - 1;
                        nextIndexSR = nextIndexSR - 1;

                        npm = new Point(nextIndexMR, nextIndexMC);
                        nps = new Point(nextIndexSR, nextIndexSC);

                        switch (Player.Orientation)
                        {
                            // Slave
                            case Orientation.Left:
                                return nextIndexSR >= 0 && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Both
                            case Orientation.Down:
                            case Orientation.Up:
                                return nextIndexMR >= 0 && nextIndexSR >= 0 && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Master
                            case Orientation.Right:
                                return nextIndexMR >= 0 && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }

                    // Player going right
                    case Orientation.Right:
                        nextIndexMR = nextIndexMR + 1;
                        nextIndexSR = nextIndexSR + 1;

                        npm = new Point(nextIndexMR, nextIndexMC);
                        nps = new Point(nextIndexSR, nextIndexSC);

                        switch (direction)
                        {
                            // Slave
                            case Orientation.Left:
                                return nextIndexSR < Columns && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Both
                            case Orientation.Down:
                            case Orientation.Up:
                                return nextIndexMR < Columns && nextIndexSR < Columns && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Master
                            case Orientation.Right:
                                return nextIndexMR < Columns && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }

                    // Player going up
                    case Orientation.Up:

                        nextIndexMC = nextIndexMC - 1;
                        nextIndexSC = nextIndexSC - 1;

                        npm = new Point(nextIndexMR, nextIndexMC);
                        nps = new Point(nextIndexSR, nextIndexSC);

                        switch (direction)
                        {
                            case Orientation.Left:
                            case Orientation.Right:
                                return nextIndexMC >= 0 && nextIndexSC >= 0 && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            case Orientation.Up:
                                return nextIndexSC >= 0 && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Master
                            case Orientation.Down:
                                return nextIndexMC >= 0 && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }

                    // Player going up
                    case Orientation.Down:

                        nextIndexMC = nextIndexMC + 1;
                        nextIndexSC = nextIndexSC + 1;

                        npm = new Point(nextIndexMR, nextIndexMC);
                        nps = new Point(nextIndexSR, nextIndexSC);

                        switch (direction)
                        {
                            case Orientation.Left:
                            case Orientation.Right:
                                return nextIndexMC < Rows && nextIndexSC < Rows && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            case Orientation.Up:
                                return nextIndexSC < Rows && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined;
                            // Master
                            case Orientation.Down:
                                return nextIndexMC < Rows && Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }
                }
            }
            else throw new PlayerException(PlayerException.OfType.SpawnError);

            npm = Point.Zero;
            nps = Point.Zero;

            return false;
        }

        /// <summary>
        /// Get the color of a puyo at given coordinates
        /// </summary>
        /// <param name="column">column</param>
        /// <param name="row">row</param>
        /// <param name="puyo">out : color of the puyo</param>
        /// <returns>False if x,y are invalid</returns>
        public bool GetPuyo(int row, int column, out Puyo puyo)
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
        public List<Point> GetNeighbors(int row, int column)
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

        /// <summary>
        /// Goes through every cell and bind puyos of the same colors together
        /// </summary>
        /// <param name="indexes">an array mainly used for debug purpose</param>
        /// <returns>A dictionary containing every "piece" for every color</returns>
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

    }
}
