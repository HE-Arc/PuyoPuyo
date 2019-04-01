using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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
        [ThreadStatic]
        private static readonly Random rng = new Random();
        private readonly int puyoCount = Enum.GetValues(typeof(Puyo)).Length;

        public static readonly int DELAY_SPAWN = 500;
        public static readonly int DELAY_FALL = 200;
        public static readonly int DELAY_FALL_FAST = 50;

        private readonly Stopwatch stopwatch = new Stopwatch();
        private GameboardState gameboardState = GameboardState.PAUSED;

        private bool isPlayerBroken = false;
        private bool isChainBroken = false;
        private bool isSpawnRequested = false;
        private bool isPuyoFalling = false;
        private bool isGameSpeedUp = false;

        // Puyo
        public Player Player { get; private set; }

        // Grid
        public Puyo[,] Cells { get; private set; }
        public int Columns { get; private set; }
        public int Rows { get; private set; }

        // Interface
        public bool Enabled { get; set; }
        public int UpdateOrder => 0;

        // Draw elements
        private Texture2D BoardCase;
        private const int SizeBoardCase = 50;
        private Vector2 Scale = new Vector2(0.38f);

        /// <summary>
        /// Help managing puyos.
        /// <para/> Row-major
        /// </summary>
        /// <param name="columns">columns of the grid</param>
        /// <param name="rows">rows of the grid</param>
        public Gameboard(int columns, int rows)
        {
            // Logic elements init
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
        /// Get a random puyo
        /// </summary>
        /// <returns>A colorfull puyo</returns>
        public Puyo GetNextRandomPuyo()
        {
            return (Puyo)(rng.Next(0, puyoCount));
        }

        /// <summary>
        /// Pause the game
        /// </summary>
        public void Pause()
        {
            gameboardState = GameboardState.PAUSED;
        }

        /// <summary>
        /// Resume the game
        /// </summary>
        public void Resume()
        {
            gameboardState = GameboardState.RUNNING;
        }

        /// <summary>
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(Puyo color)
        {
            if (Player is null || !Player.Alive)
            {
                Player = new Player(this, color);
                isPlayerBroken = false;
            }
            else throw new PlayerException(PlayerException.OfType.SpawnError);
        }

        /// <summary>
        /// Move the player according to the given direction
        /// </summary>
        /// <param name="direction">where to go</param>
        private void Move(Orientation direction)
        {
            if (Player is null)
                throw new PlayerException(PlayerException.OfType.NotCreated);

            if (Player.Alive && !isPlayerBroken)
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

                        switch (Player.Orientation)
                        {
                            // Slave
                            case Orientation.Left:
                                if (nextIndexSR >= 0)
                                {
                                    if (Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Both
                            case Orientation.Down:
                            case Orientation.Up:
                                if (nextIndexMR >= 0 && nextIndexSR >= 0)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Master
                            case Orientation.Right:
                                if (nextIndexMR >= 0)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }
                        break;

                    // Player going right
                    case Orientation.Right:
                        nextIndexMR = nextIndexMR + 1;
                        nextIndexSR = nextIndexSR + 1;

                        switch (direction)
                        {
                            // Slave
                            case Orientation.Left:
                                if (nextIndexSR < Columns)
                                {
                                    if (Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Both
                            case Orientation.Down:
                            case Orientation.Up:
                                if (nextIndexMR < Columns && nextIndexSR < Columns)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Master
                            case Orientation.Right:
                                if (nextIndexMR < Columns)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }
                        break;

                    // Player going up
                    case Orientation.Up:

                        nextIndexMC = nextIndexMC - 1;
                        nextIndexSC = nextIndexSC - 1;

                        switch (direction)
                        {
                            case Orientation.Left:
                            case Orientation.Right:
                                if (nextIndexMC >= 0 && nextIndexSC >= 0)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            case Orientation.Up:
                                if (nextIndexSC >= 0)
                                {
                                    if (Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Master
                            case Orientation.Down:
                                if (nextIndexMC >= 0)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }
                        break;

                    // Player going up
                    case Orientation.Down:

                        nextIndexMC = nextIndexMC + 1;
                        nextIndexSC = nextIndexSC + 1;

                        switch (direction)
                        {
                            case Orientation.Left:
                            case Orientation.Right:
                                if (nextIndexMC < Rows && nextIndexSC < Rows)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined && Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            case Orientation.Up:
                                if (nextIndexSC < Rows)
                                {
                                    if (Cells[nextIndexSR, nextIndexSC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            // Master
                            case Orientation.Down:
                                if (nextIndexMC < Rows)
                                {
                                    if (Cells[nextIndexMR, nextIndexMC] == Puyo.Undefined)
                                    {
                                        // Free cell, move puyo
                                        Player.Master = new Point(nextIndexMR, nextIndexMC);
                                    }
                                    else
                                    {
                                        BreakAndFall();
                                    }
                                }
                                break;
                            default:
                                throw new ArgumentException("Invalid direction provided");
                        }
                        break;
                }
            }
            else throw new PlayerException(PlayerException.OfType.NotAlive);
        }

        /// <summary>
        /// Break the player in half and kill it
        /// <para/>Do not check if player is null. This method is only invoked in Move which already validated that user exist
        /// </summary>
        private void BreakAndFall()
        {
            isPlayerBroken = true;
            Player.Alive = false;
        }

        #region Control command
        /// <summary>
        /// Move the player on the left
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Left()
        {
            Move(Orientation.Left);
        }

        /// <summary>
        /// Move the player on the right
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Right()
        {
            Move(Orientation.Right);
        }

        /// <summary>
        /// Move the player on the up
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Up()
        {
            Move(Orientation.Up);
        }

        /// <summary>
        /// Move the player on the down
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Down()
        {
            Move(Orientation.Down);
        }
        #endregion

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

        /// <summary>
        /// Update the gameboard
        /// </summary>
        /// <param name="gameTime">Not used</param>
        public void Update(GameTime gameTime)
        {
            if (gameboardState == GameboardState.PAUSED)
            {
                throw new NotImplementedException();
            }
            else
            {
                // Handle spawn
                if (isSpawnRequested)
                {
                    // Stopwatch must have been resetted and the puyo died
                    if (stopwatch.ElapsedMilliseconds > DELAY_SPAWN)
                    {
                        Spawn(GetNextRandomPuyo());
                    }
                }
                else
                {
                    // Check if player is cut in half
                    // If true : player should not be able to move
                    // If false: game continues normally
                    if (isPlayerBroken)
                    {
                        // Check if it's time to move on
                        if (stopwatch.ElapsedMilliseconds < DELAY_FALL_FAST)
                            return;

                        // Restart stopwatch
                        stopwatch.Restart();

                        // Make every puyo fall
                        isPuyoFalling = false;
                        for (int row = Rows; row >= 0; row--)
                        {
                            for (int col = 0; col < Columns; col++)
                            {
                                if (Cells[row, col] == Puyo.Undefined) continue;
                                else
                                {
                                    if (Cells[row - 1, col] == Puyo.Undefined)
                                    {
                                        // Move down the puyo
                                        Cells[row - 1, col] = Cells[row, col];

                                        // Free the previous cell
                                        Cells[row, col] = Puyo.Undefined;

                                        // Set var to true
                                        isPuyoFalling = true;
                                    }
                                }
                            }
                        }

                        // Check if any puyo has been falling
                        if (!isPuyoFalling)
                        {
                            // Check if any chain was created
                            var chains = GetChains(out int[,] indexes);
                            // Check if any chains has been found
                            if (chains.Count == 0)
                            {
                                // Request spawn
                                isSpawnRequested = true;
                            }
                            else
                            {
                                // Export chains
                                // TODO:
                            }
                        }
                    }
                    // Player is not cut in a half !
                    else
                    {
                        if (!(Player is null) && Player.Alive)
                        {
                            if (isGameSpeedUp)
                            {
                                // Check if it's time to move on
                                if (stopwatch.ElapsedMilliseconds < DELAY_FALL_FAST)
                                    return;

                                // Restart stopwatch
                                stopwatch.Restart();
                                Down();
                            }
                            else
                            {
                                // Check if it's time to move on
                                if (stopwatch.ElapsedMilliseconds < DELAY_FALL)
                                    return;

                                // Restart stopwatch
                                stopwatch.Restart();
                                Down();
                            }
                        }
                        else
                        {
                            isSpawnRequested = true;
                        }
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            BoardCase = new Texture2D(spriteBatch.GraphicsDevice, SizeBoardCase, SizeBoardCase);
            int X = 0;
            int Y = 0;

            // Row first
            for (int y = 0; y < Rows; y++)
            {
                X = 0;
                Y += SizeBoardCase;
                for (int x = 0; x < Columns; x++)
                {
                    RectangleSprite.DrawRectangle(spriteBatch, new Rectangle(X, Y, SizeBoardCase, SizeBoardCase), Color.Red, 2);
                    Texture2D t = null;

                    switch (Cells[y, x])
                    {
                        case Puyo.Red:
                            t = TextureManager.Instance.TryGet<Texture2D>("PuyoRed");
                            break;
                        case Puyo.Green:
                            t = TextureManager.Instance.TryGet<Texture2D>("PuyoGreen");
                            break;
                        case Puyo.Blue:
                            t = TextureManager.Instance.TryGet<Texture2D>("PuyoBlue");
                            break;
                        case Puyo.Yellow:
                            t = TextureManager.Instance.TryGet<Texture2D>("PuyoYellow");
                            break;
                        case Puyo.Purple:
                            t = TextureManager.Instance.TryGet<Texture2D>("PuyoPurple");
                            break;
                    }

                    if (t != null)
                    {
                        spriteBatch.Draw(t, new Vector2(X, Y), origin: new Vector2(0, 0), scale: Scale);
                    }

                    X += SizeBoardCase;
                }
            }

        }
    }

    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }
}
