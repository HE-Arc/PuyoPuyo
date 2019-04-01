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
    class InputManager
    {

        private KeyboardState kState;
        private GamePadState gState;
        private Dictionary<Player, PlayerIndex> playersKeyBoard = new Dictionary<Player, PlayerIndex>();
        private Dictionary<Player, PlayerIndex> playersGamePad = new Dictionary<Player, PlayerIndex>();
        private PlayerIndex gamePadIndex;
        private Dictionary<Input, InputTimer> inputsUsable = new Dictionary<Input, InputTimer>();


        public float DeadzoneSticks = 0.25f;

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

        private InputManager() {
            foreach (Input item in Enum.GetValues(typeof(Input)))
            {
                InputTimer inputTimer = new InputTimer();
                inputsUsable.Add(item, inputTimer);
            }
        }

        public bool SetKeyBoard(Player player, PlayerIndex gamePadIndex)
        {
            if (playersKeyBoard.ContainsValue(gamePadIndex))
                return false;

            if (!playersKeyBoard.ContainsKey(player))
                playersKeyBoard.Add(player, gamePadIndex);
            else
                playersKeyBoard[player] = gamePadIndex;

            return true;
        }

        public bool SetGamePad(Player player, PlayerIndex gamePadIndex)
        {
            if (playersKeyBoard.ContainsValue(gamePadIndex))
                return false;

            if (!playersGamePad.ContainsKey(player))
                playersGamePad.Add(player, gamePadIndex);
            else
                playersGamePad[player] = gamePadIndex;

            return true;
        }

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

        private void PlayerGamePad(List<Input> inputs, Player player = null)
        { 
            gState = (player != null) ? GamePad.GetState(playersGamePad[player]) : GamePad.GetState(gamePadIndex);
            if (gState.IsConnected)
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
        }

        public List<Input> Perform(Player player = null)
        {
            List<Input> inputs = new List<Input>();

            if (player != null)
            {
                if (playersGamePad.ContainsKey(player))
                    PlayerGamePad(inputs, player);

                if (inputs.Count == 0)
                {
                    if (playersKeyBoard[player] == PlayerIndex.One)
                        Player1Keyboard(inputs);
                    else
                        Player2Keyboard(inputs);
                }
            }
            else
            {
                PlayerGamePad(inputs);

                if (inputs.Count == 0)
                    Player1Keyboard(inputs);
            }

            for (int i = inputs.Count -1; i >= 0; --i)
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

        
    }
}
