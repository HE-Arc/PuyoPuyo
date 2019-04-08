using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.Exceptions;
using PuyoPuyo.GameObjects.Grids;
using PuyoPuyo.GameObjects.Puyos;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace PuyoPuyo.GameObjects
{
    public sealed class Gameboard
    {
        [ThreadStatic]
        private static readonly Random rng = new Random();
        private readonly int puyoCount = Enum.GetValues(typeof(PuyoColor)).Length;

        // Delay
        public static readonly int DELAY_SPAWN = 500;
        public static readonly int DELAY_FALL = 350;
        public static readonly int DELAY_FALL_FAST = 150;

        // Game flow
        private readonly Stopwatch stopwatch = new Stopwatch();
        private GameboardState gameboardState = GameboardState.PAUSED;

        // Boolean state machine
        private bool isChainBroken = false;
        private bool isSpawnRequested = false;
        private bool isPuyoFalling = false;
        private bool isGameSpeedUp = false;

        // PuyoColor
        public Player Player { get; private set; }

        // Grid
        public Grid Grid { get; private set; }

        // ScoreManager
        public ScoreManager ScoreManager { get; private set; }

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
            // Create a new grid
            this.Grid = new Grid(rows, columns);

            // ---
            this.ScoreManager = new ScoreManager();
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
            stopwatch.Restart();
        }

        /// <summary>
        /// Spawn a random puyopuyo
        /// </summary>
        public void Spawn()
        {
            Spawn((PuyoColor)rng.Next(0, puyoCount - 1));
        }

        /// <summary>
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(PuyoColor color)
        {
            if (Player is null)
            {
                Player = new Player(this.Grid, color);
                isSpawnRequested = false;
            }
            else throw new PlayerException(PlayerException.OfType.SpawnError);
        }

        private bool CanAlterPlayer()
        {
            // Check if a current chain of explosion is occuring
            if (isChainBroken) return false;

            /* ---------------------------------------- \
             * | Check if player is available           |
             * | Player might be null if                |
             * | - player has not been already spawned  |
             * | - player has been broken in half       |
            /* --------------------------------------- */
            return !(Player is null);
        }

        /// <summary>
        /// Move the player according to the given direction
        /// </summary>
        /// <param name="direction">where to go</param>
        private void Move(Orientation direction)
        {
            if (CanAlterPlayer())
            {
                // Player can move
                switch (direction)
                {
                    case Orientation.Down:
                        // If can't go down, break it in half and nullify it
                        if (!Player.Down())
                            BreakPuyoPuyo();
                        break;
                    case Orientation.Left:
                        // If can't go left, break it in half and nullify it
                        if (!Player.Left())
                            BreakPuyoPuyo();
                        break;
                    case Orientation.Right:
                        // If can't go right, break it in half and nullify it
                        if (!Player.Right())
                            BreakPuyoPuyo();
                        break;
                }
            }
            else return; // throw new PlayerException(PlayerException.OfType.NotAlive);
        }

        private void BreakPuyoPuyo()
        {
            //TODO: continue
            Player = null;
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
        /// Move the player on the down
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Down()
        {
            Move(Orientation.Down);
        }

        /// <summary>
        /// Rotate puyopuyo
        /// </summary>
        /// <param name="rotation"></param>
        public void Rotate(Rotation rotation)
        {
            if (CanAlterPlayer())
            {
                Player.Rotate(rotation);
            }
            else throw new PlayerException(PlayerException.OfType.NotAlive);
        }
        #endregion

        /// <summary>
        /// Get the neighbors of a PuyoColor at given coordinates
        /// </summary>
        /// <param name="column">column</param>
        /// <param name="row">row</param>
        /// <param name="PuyoColor">out : color of the PuyoColor</param>
        /// <returns>Every PuyoColor of the same color in V4</returns>
        public List<Puyo> GetNeighbors(int row, int column)
        {
            // Get neighbors
            List<Puyo> neighbors = new List<Puyo>(4);

            // Get the cell in at the given row and column
            Cell center = Grid[row, column];

            // Test if cell exist and is occupied
            if (!(center == null) && !center.IsFree)
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
                    // Get the cell
                    Cell temp_cell = Grid[p.X, p.Y];

                    // Test if cell exist and is occupied
                    if (!(temp_cell == null) && !temp_cell.IsFree)
                    {
                        neighbors.Add(temp_cell.Puyo);
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
        public Dictionary<PuyoColor, Dictionary<int, List<Puyo>>> GetChains(out int[,] indexes, out int chainCount)
        {
            Dictionary<PuyoColor, Dictionary<int, List<Puyo>>> pieces = new Dictionary<PuyoColor, Dictionary<int, List<Puyo>>>();
            foreach (PuyoColor PuyoColor in Enum.GetValues(typeof(PuyoColor)))
            {
                pieces.Add(PuyoColor, new Dictionary<int, List<Puyo>>());
            }

            int newIndex = 1;

            // Will contains indexes
            indexes = new int[Grid.Rows, Grid.Columns];

            // Row first
            for (int row = Grid.Rows; row >= 0; row--)
            {
                for (int col = 0; col < Grid.Columns; col++)
                {
                    Cell cell = this.Grid[row, col];
                    // Test if cell exist and is occupied
                    if (!(cell == null) && !cell.IsFree)
                    {
                        List<Puyo> neighbors = GetNeighbors(cell.Row, cell.Column);

                        // Not connected
                        if (neighbors.Count == 0)
                        {
                            // Add it as a new piece
                            pieces[cell.Puyo.Color].Add(newIndex, new List<Puyo>()
                            {
                                cell.Puyo
                            });

                            // Add it to the map
                            indexes[cell.Row, cell.Column] = newIndex;

                            // Increment piece index
                            newIndex++;
                        }
                        else
                        {
                            int minIndex = Int32.MaxValue;
                            int puyoIndex = indexes[cell.Row, cell.Column];

                            if (puyoIndex <= 0)
                            {
                                puyoIndex = newIndex;
                            }

                            // Search pieces
                            foreach (Puyo neighbor in neighbors)
                            {
                                int neighborIndex = indexes[neighbor.Row, neighbor.Column];
                                if (neighborIndex > 0 && neighborIndex < minIndex)
                                    minIndex = indexes[neighbor.Row, neighbor.Column];
                            }

                            // Replace puyoindex
                            if (minIndex < puyoIndex)
                            {
                                puyoIndex = minIndex;
                            }


                            // Insert the new PuyoColor to the matching pieces
                            if (!pieces[cell.Puyo.Color].ContainsKey(puyoIndex))
                            {
                                // Add it as a new piece
                                pieces[cell.Puyo.Color].Add(newIndex, new List<Puyo>()
                                {
                                    cell.Puyo
                                });

                                // Add it to the map
                                indexes[cell.Row, cell.Column] = newIndex;

                                // Increment piece index
                                newIndex++;
                            }
                            else
                            {
                                // Add the new
                                pieces[cell.Puyo.Color][puyoIndex].Add(cell.Puyo);

                                // Adapt index on the map
                                indexes[cell.Row, cell.Column] = puyoIndex;
                            }
                        }
                    }
                    else continue;
                }
            }

            List<int> indexToRemove = new List<int>();

            // Remove too small pieces
            foreach (var kv in pieces)
            {
                PuyoColor puyoColor = kv.Key;
                Dictionary<int, List<Puyo>> coloredPieces = kv.Value;

                foreach(KeyValuePair<int, List<Puyo>> piece in coloredPieces)
                {
                    // Register too short puyos
                    if (piece.Value.Count < 4)
                    {
                        indexToRemove.Add(piece.Key);
                        newIndex--;
                    }
                }
            }

            // Remove puyos who are too short
            foreach(int index in indexToRemove)
            {
                foreach (var kv in pieces)
                {
                    PuyoColor puyoColor = kv.Key;
                    Dictionary<int, List<Puyo>> coloredPieces = kv.Value;

                    // Remove the piece
                    coloredPieces.Remove(index);
                }
            }

            // Set chain count
            chainCount = newIndex - 1;

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
                if (isSpawnRequested && !isChainBroken)
                {
                    // Stopwatch must have been resetted and the PuyoColor died
                    if (stopwatch.ElapsedMilliseconds > DELAY_SPAWN)
                    {
                        Spawn();
                    }
                }
                else
                {
                    // Check if player is cut in half
                    // If true : player should not be able to move
                    // If false: game continues normally
                    if (isChainBroken || Player is null)
                    {
                        // Check if it's time to move on
                        if (stopwatch.ElapsedMilliseconds < DELAY_FALL_FAST)
                            return;

                        // Restart stopwatch
                        stopwatch.Restart();

                        // Make every PuyoColor fall
                        isPuyoFalling = false;

                        foreach(Cell cell in Grid)
                        {
                            // Test if cell exist and is occupied
                            if (!(cell == null) && !cell.IsFree)
                            {
                                Cell next_cell = Grid[cell.Row + 1, cell.Column];

                                // Move down puyo
                                if (cell.Puyo.TryMoveToCell(next_cell))
                                    isPuyoFalling = true;
                            }
                        }

                        // Check if any PuyoColor has been falling
                        if (!isPuyoFalling)
                        {
                            // Check if any chain was created
                            var chains = GetChains(out int[,] indexes, out int chainCount);

                            // Check if any chains has been found
                            if (chainCount == 0)
                            {
                                // Request spawn
                                isSpawnRequested = true;
                                isChainBroken = false;
                            }
                            else
                            {
                                isChainBroken = true;
                                // Export chains
                                // TODO:
                                foreach (var kv in chains)
                                {
                                    PuyoColor pc = kv.Key;
                                    Dictionary<int, List<Puyo>> coloredPieces = kv.Value;
                                    foreach (List<Puyo> piece in coloredPieces.Values)
                                    {
                                        // Add it to score Manager
                                        ScoreManager.Add(pc, piece.Count);

                                        // Release cells
                                        foreach(Puyo puyo in piece)
                                        {
                                            this.Grid[puyo.Row, puyo.Column].Release(puyo);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // Player is not cut in a half !
                    else
                    {
                        if (!(Player is null))
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

        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            int X = 0;
            int Y = 0;

            // Row first
            for (int y = 0; y < Grid.Rows; y++)
            {
                X = 0;
                Y += SizeBoardCase;
                for (int x = 0; x < Grid.Columns; x++)
                {
                    Grid.Draw(spriteBatch, X, Y, SizeBoardCase);

                    if(!Grid[y, x].IsFree)
                    {
                        Grid[y, x].Puyo.Draw(spriteBatch, X, Y, Scale);
                    }

                    X += SizeBoardCase;
                }
            }

            ScoreManager.Draw(spriteBatch, Font, new Vector2(Grid.Columns*SizeBoardCase+20, SizeBoardCase));
        }
    }


}
