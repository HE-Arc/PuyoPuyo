using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuyoPuyo.Toolbox;
using System.Timers;
using PuyoPuyo.GameObjects;

namespace PuyoPuyo.Toolbox
{
    /// <summary>
    /// Singleton to manage keyboard and Gamepad on whole application
    /// </summary>
    class InputManager
    {

        private KeyboardState kState;
        private GamePadState gState;
        private Dictionary<PlayerIndex, PlayerIndex> playersGamePad = new Dictionary<PlayerIndex, PlayerIndex>(); // reference each gamepad use by player
        private Dictionary<Input, InputTimer> inputsUsable1 = new Dictionary<Input, InputTimer>(); // list of timer to avoid to use an action many times at one human input
        private Dictionary<Input, InputTimer> inputsUsable2 = new Dictionary<Input, InputTimer>(); // list of timer to avoid to use an action many times at one human input

        public float DeadzoneSticks = 0.25f; // avoid to move without will

        private static InputManager instance;
        public static InputManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new InputManager();
                }

                return instance;
            }
        }

        public int NbPlayer { get; set; }

        private InputManager()
        {
            foreach (Input item in Enum.GetValues(typeof(Input)))
            {
                InputTimer inputTimer1 = new InputTimer();
                inputsUsable1.Add(item, inputTimer1);

                InputTimer inputTimer2 = new InputTimer();
                inputsUsable2.Add(item, inputTimer2);
            }
        }

        /// <summary>
        /// Set the next free gamepad to the player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public bool SetGamePad(PlayerIndex player)
        {
            foreach (PlayerIndex playerIndex in Enum.GetValues(typeof(PlayerIndex)))
            {
                if (!playersGamePad.ContainsValue(playerIndex))
                {
                    if (!playersGamePad.ContainsKey(player))
                        playersGamePad.Add(player, playerIndex);
                    else
                        playersGamePad[player] = playerIndex;

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// List of action made by the player 1 with the keyboard
        /// </summary>
        /// <param name="inputs"></param>
        private void Player1Keyboard(List<Input> inputs)
        {
            kState = Keyboard.GetState();
            if (kState != null)
            {
                if (kState.IsKeyDown(Keys.A))
                    inputs.Add(Input.Left);

                if (kState.IsKeyDown(Keys.W))
                    inputs.Add(Input.Up);

                if (kState.IsKeyDown(Keys.D))
                    inputs.Add(Input.Right);

                if (kState.IsKeyDown(Keys.S))
                    inputs.Add(Input.Down);

                if (kState.IsKeyDown(Keys.Enter))
                    inputs.Add(Input.Validate);

                if (kState.IsKeyDown(Keys.Escape))
                    inputs.Add(Input.Cancel);

                if (kState.IsKeyDown(Keys.Tab))
                    inputs.Add(Input.Pause);

                if (kState.IsKeyDown(Keys.Q))
                    inputs.Add(Input.CounterclockwiseRotation);

                if (kState.IsKeyDown(Keys.E))
                    inputs.Add(Input.ClockwiseRotation);
            }
        }

        /// <summary>
        /// List of action made by the player 2 with the keyboard
        /// </summary>
        /// <param name="inputs"></param>
        private void Player2Keyboard(List<Input> inputs)
        {
            kState = Keyboard.GetState();
            if (kState != null)
            {
                if (kState.IsKeyDown(Keys.Left))
                    inputs.Add(Input.Left);

                if (kState.IsKeyDown(Keys.Up))
                    inputs.Add(Input.Up);

                if (kState.IsKeyDown(Keys.Right))
                    inputs.Add(Input.Right);

                if (kState.IsKeyDown(Keys.Down))
                    inputs.Add(Input.Down);

                if (kState.IsKeyDown(Keys.P))
                    inputs.Add(Input.Pause);

                if (kState.IsKeyDown(Keys.PageUp))
                    inputs.Add(Input.CounterclockwiseRotation);

                if (kState.IsKeyDown(Keys.PageDown))
                    inputs.Add(Input.ClockwiseRotation);
            }
        }

        /// <summary>
        /// List of action made by the current gamepad
        /// </summary>
        /// <param name="inputs"></param>
        /// <param name="gState"></param>
        private void UseGamePad(List<Input> inputs, GamePadState gState)
        {

            // Left Stick
            if (gState.ThumbSticks.Left.X < -DeadzoneSticks)
            {
                inputs.Add(Input.Left);
            }

            if (gState.ThumbSticks.Left.Y > DeadzoneSticks)
            {
                inputs.Add(Input.Up);
            }

            if (gState.ThumbSticks.Left.X > DeadzoneSticks)
            {
                inputs.Add(Input.Right);
            }

            if (gState.ThumbSticks.Left.Y < -DeadzoneSticks)
            {
                inputs.Add(Input.Down);
            }

            // DPad
            if (gState.IsButtonDown(Buttons.DPadLeft))
            {
                inputs.Add(Input.Left);
            }

            if (gState.IsButtonDown(Buttons.DPadUp))
            {
                inputs.Add(Input.Up);
            }

            if (gState.IsButtonDown(Buttons.DPadRight))
            {
                inputs.Add(Input.Right);
            }

            if (gState.IsButtonDown(Buttons.DPadDown))
            {
                inputs.Add(Input.Down);
            }

            if (gState.IsButtonDown(Buttons.A))
            {
                inputs.Add(Input.Validate);
            }

            if (gState.IsButtonDown(Buttons.B))
            {
                inputs.Add(Input.Cancel);
            }

            if (gState.IsButtonDown(Buttons.Start))
            {
                inputs.Add(Input.Pause);
            }

            if (gState.IsButtonDown(Buttons.LeftTrigger))
            {
                inputs.Add(Input.CounterclockwiseRotation);
            }

            if (gState.IsButtonDown(Buttons.RightTrigger))
            {
                inputs.Add(Input.ClockwiseRotation);
            }
        }

        /// <summary>
        /// Return list of action made by the current player
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public List<Input> Perform(PlayerIndex player)
        {
            Dictionary<Input, InputTimer> inputsUsable = (player == PlayerIndex.One) ? inputsUsable1 : inputsUsable2;

            List<Input> inputs = new List<Input>();
            bool gamePadConnected = false;

            // Check if a gamepad is setted for the current player
            if (playersGamePad.ContainsKey(player))
            {
                gState = GamePad.GetState(playersGamePad[player]);

                if (gState.IsConnected)
                    gamePadConnected = true;
            }

            // Check if the gamepad is always connected
            // If it's true use this else use keyboard
            if (gamePadConnected)
            {
                UseGamePad(inputs, gState);
            }
            else
            {
                if (player == PlayerIndex.One)
                    Player1Keyboard(inputs);
                else
                    Player2Keyboard(inputs);
            }

            // Check if the last use of input isn't too short
            for (int i = inputs.Count - 1; i >= 0; --i)
            {
                if (inputsUsable[inputs[i]].Usable)
                {
                    inputsUsable[inputs[i]].Usable = false;


                    inputsUsable[inputs[i]].Timer.Start();
                }
                else
                {
                    inputs.RemoveAt(i);
                }
            }

            return inputs;
        }

        /// <summary>
        /// Return inputs from default configuration (wasd and first Gamepad)
        /// </summary>
        /// <returns></returns>
        public List<Input> Perform()
        {
            List<Input> inputs = new List<Input>();

            gState = GamePad.GetState(PlayerIndex.One);

            if (gState.IsConnected)
                UseGamePad(inputs, gState);
            else
                Player1Keyboard(inputs);

            for (int i = inputs.Count - 1; i >= 0; --i)
            {
                if (inputsUsable1[inputs[i]].Usable)
                {
                    inputsUsable1[inputs[i]].Usable = false;


                    inputsUsable1[inputs[i]].Timer.Start();
                }
                else
                {
                    inputs.RemoveAt(i);
                }
            }

            return inputs;
        }

        /// <summary>
        /// Remove gamepad setted for the current player
        /// </summary>
        /// <param name="playerIndex"></param>
        public void RemovePlayer(PlayerIndex playerIndex)
        {

            if (playersGamePad.ContainsKey(playerIndex))
                playersGamePad.Remove(playerIndex);
        }

        // Reset gamepad selected
        public void Reset()
        {
            playersGamePad.Clear();
        }
    }
}
