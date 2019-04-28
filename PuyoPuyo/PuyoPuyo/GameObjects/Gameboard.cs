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
using PuyoPuyo.GameObjects.Puyos.Data;

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
        public static readonly int DELAY_FALL_FAST = (int)(DELAY_FALL / 3);

        private int fallAcceleration = 0;


        // Game flow
        private readonly Stopwatch stopwatch = new Stopwatch();
        private GameboardState gameboardState = GameboardState.PAUSED;

        // Boolean state machine
        private bool isChainBroken = false;
        private bool isSpawnRequested = false;
        private bool isPuyoFalling = false;

        // PuyoColor
        public Player Player { get; private set; }
        public Queue<Tuple<PuyoColor, PuyoColor>> NextPuyos;

        // Grid
        public Grid Grid { get; private set; }

        // ScoreManager
        public ScoreManager ScoreManager { get; private set; }

        // Next Puyo
        private int nbNextPuyoShown = 4;

        // Draw elements
        private Texture2D Background;
        private const int SizeBoardCase = 65;
        private Vector2 Scale = new Vector2(0.50f); // 128 => 1 | SizeBoardCase => SizeBoardCase*1/128 = 

        private int offsetX;
        private int offsetY;

        

        /// <summary>
        /// Help managing puyos.
        /// <para/> Row-major
        /// </summary>
        /// <param name="columns">columns of the grid</param>
        /// <param name="rows">rows of the grid</param>
        public Gameboard(int columns, int rows, int offsetX, int offsetY)
        {
            // Create a new grid
            this.Grid = new Grid(rows, columns);

            // ---
            this.ScoreManager = new ScoreManager();

            // ---
            NextPuyos = new Queue<Tuple<PuyoColor, PuyoColor>>();

            // Get offset
            this.offsetX = offsetX;
            this.offsetY = offsetY;

            // Background texture          
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
        /// Spawn a puyopuyo
        /// </summary>
        /// <param name="color"></param>
        public void Spawn(Tuple<PuyoColor, PuyoColor> colors)
        {
            if (Player is null)
            {
                Player = new Player(this.Grid, colors);
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
            return !(Player == null);
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
            if (CanAlterPlayer())
            {
                switch (Player.Orientation)
                {
                    case Orientation.Left:
                        if (Player.Slave.Column > 0) Move(Orientation.Left);
                        break;
                    case Orientation.Right:
                        if (Player.Master.Column > 0) Move(Orientation.Left);
                        break;
                    case Orientation.Up:
                    case Orientation.Down:
                        if (Player.Master.Column > 0 && Player.Slave.Column > 0) Move(Orientation.Left);
                        break;
                }
            }
            else return; //throw new PlayerException(PlayerException.OfType.NotAlive);
        }

        /// <summary>
        /// Move the player on the right
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Right()
        {
            if (CanAlterPlayer())
            {
                switch (Player.Orientation)
                {
                    case Orientation.Left:
                        if (Player.Master.Column < Grid.Columns - 1) Move(Orientation.Right);
                        break;
                    case Orientation.Right:
                        if (Player.Slave.Column < Grid.Columns - 1) Move(Orientation.Right);
                        break;
                    case Orientation.Up:
                    case Orientation.Down:
                        if (Player.Master.Column < Grid.Columns - 1 && Player.Slave.Column < Grid.Columns - 1) Move(Orientation.Right);
                        break;
                }
            }
            else return; //throw new PlayerException(PlayerException.OfType.NotAlive);
        }

        /// <summary>
        /// Move the player on the down
        /// <para/>Move contains test on player and booleans
        /// </summary>
        public void Down()
        {
            if (CanAlterPlayer())
            {
                Move(Orientation.Down);
            }
            else return; //throw new PlayerException(PlayerException.OfType.NotAlive);
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
            else return; //throw new PlayerException(PlayerException.OfType.NotAlive);
        }
        #endregion

        

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
                // Calcul fallAcceleration in function of the Score value
                if(ScoreManager.Score != 0)
                    fallAcceleration = (int)Math.Log10(Convert.ToDouble(ScoreManager.Score)) * 10; 

                // Generate puyopuyo
                while (NextPuyos.Count < 5)
                {
                    NextPuyos.Enqueue(new Tuple<PuyoColor, PuyoColor>((PuyoColor)rng.Next(0, puyoCount - 1), (PuyoColor)rng.Next(0, puyoCount - 1)));
                }

                // Handle spawn
                if (isSpawnRequested && !isChainBroken)
                {
                    // Stopwatch must have been resetted and the PuyoColor died
                    if (stopwatch.ElapsedMilliseconds > DELAY_SPAWN - fallAcceleration)
                    {
                        Spawn(NextPuyos.Dequeue());
                    }
                }
                else
                {
                    // Check if player is cut in half
                    // If true : player should not be able to move
                    // If false: game continues normally
                    if (isChainBroken || Player == null)
                    {
                        // Check if it's time to move on
                        if (stopwatch.ElapsedMilliseconds < DELAY_FALL_FAST - fallAcceleration)
                            return;

                        // Restart stopwatch
                        stopwatch.Restart();

                        // Make every PuyoColor fall
                        isPuyoFalling = false;

                        for (int row = Grid.Rows - 1; row >= 0; row--)
                        {
                            for (int col = 0; col < Grid.Columns; col++)
                            {
                                // Get cell
                                Cell cell = Grid[row, col];
                                Puyo puyo = cell.Puyo;

                                // Player is handled below
                                if (puyo == Player?.Master || puyo == Player?.Slave)
                                    continue;

                                // Test if cell exist and is occupied
                                if (cell == null || cell.IsFree)
                                    continue;

                                // Get next cell
                                Cell next_cell = Grid[cell.Row + 1, cell.Column];
                                if (next_cell == null || !next_cell.IsFree)
                                    continue;

                                // Try to move puyo
                                if(puyo.Move(next_cell)) isPuyoFalling = true;
                            }
                        }

                        // Check if any PuyoColor has been falling
                        if (!isPuyoFalling)
                        {
                            // Check if any chain was created
                            var chains = GridHelper.GetChains(Grid, out int chainCount);

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

                                foreach (var kv in chains)
                                {
                                    PuyoColor pc = kv.Key;
                                    List<IEnumerable<Puyo>> coloredPieces = kv.Value;

                                    foreach (HashSet<Puyo> piece in coloredPieces)
                                    {
                                        // Release cells
                                        foreach (Puyo puyo in piece)
                                        {
                                            this.Grid[puyo.Row, puyo.Column].Release();
                                        }

                                        // Add it to score Manager
                                        ScoreManager.Add(pc, piece.Count);
                                    }
                                }

                                // Compute
                                ScoreManager.CalculateScore();
                            }
                        }
                    }

                    // Player is not cut in a half !
                    else
                    {
                        if (Player != null)
                        {
                            // Check if it's time to move on
                            if (stopwatch.ElapsedMilliseconds < DELAY_FALL - fallAcceleration) return;

                            // Restart stopwatch
                            stopwatch.Restart();
                            Down();
                        }
                        else
                        {
                            isSpawnRequested = true;
                        }
                    }
                }
            }
        }

        public void LoadTexture(Texture2D t)
        {
            Background = t;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteFont Font)
        {
            spriteBatch.Draw(TextureManager.Instance.TryGet<Texture2D>("InGameBg"), new Vector2(offsetY, 0));

            int X = offsetX;
            int Y = 0;

            // Row first
            for (int y = 2; y < Grid.Rows; y++)
            {
                X = offsetX;
                Y += SizeBoardCase;
                for (int x = 0; x < Grid.Columns; x++)
                {
                    //Grid.Draw(spriteBatch, X, Y, SizeBoardCase);

                    if(!Grid[y, x].IsFree)
                    {
                        Grid[y, x].Puyo.Draw(spriteBatch, X, Y, Scale);
                    }

                    X += SizeBoardCase;
                }
            }

            int offsetScore = 45;
            ScoreManager.Draw(spriteBatch, Font, new Vector2(Grid.Columns*SizeBoardCase+offsetX+offsetScore, SizeBoardCase+ 12));

            int nbNextPuyoShow = 0;
            double offsetPuyo= 2.5;
            foreach(var p in NextPuyos)
            {
                if (nbNextPuyoShow < nbNextPuyoShown)
                {
                    IPuyoData master = PuyoDataFactory.Instance.Get(p.Item1);
                    IPuyoData slave = PuyoDataFactory.Instance.Get(p.Item2);

                    spriteBatch.Draw(slave.Texture, new Vector2(Grid.Columns * SizeBoardCase + offsetX + 98, (int)(offsetPuyo * SizeBoardCase) + 20) , origin: new Vector2(0, 0), scale: Scale);
                    spriteBatch.Draw(master.Texture, new Vector2(Grid.Columns * SizeBoardCase + offsetX + 98, (int)((offsetPuyo + 1) * SizeBoardCase) + 20), origin: new Vector2(0, 0), scale: Scale);

                    offsetPuyo += 2.5;
                    nbNextPuyoShow++;
                }
            }
        }
    }


}
