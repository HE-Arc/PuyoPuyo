using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuyoPuyo.Toolbox;
using System.Timers;

namespace PuyoPuyo.Toolbox
{
    class InputManager
    {

        private KeyboardState kState;
        private GamePadState gState;
        private List<Input> inputs = new List<Input>();
        private List<Input> returnedInputs = new List<Input>();
        private Dictionary<Input, InputTimer> inputsUsable = new Dictionary<Input, InputTimer>();

        public float DeadzoneSticks = 0.25f;

        public InputManager()
        {
            foreach (Input item in Enum.GetValues(typeof(Input)))
            {
                InputTimer inputTimer = new InputTimer();
                inputsUsable.Add(item, inputTimer);
            }
        }

        public List<Input> Perform()
        {
            List<Input> lstInputs = new List<Input>();

            //keyboard
            kState = Keyboard.GetState();
            if (kState != null)
            {
                if (kState.IsKeyDown(Keys.Left))
                    lstInputs.Add(Input.Left);

                if (kState.IsKeyDown(Keys.Up))
                    lstInputs.Add(Input.Up);

                if (kState.IsKeyDown(Keys.Right))
                    lstInputs.Add(Input.Right);

                if (kState.IsKeyDown(Keys.Down))
                    lstInputs.Add(Input.Down);

                if (kState.IsKeyDown(Keys.Enter))
                    lstInputs.Add(Input.Enter);

                if (kState.IsKeyDown(Keys.Escape))
                    lstInputs.Add(Input.Escape);

                if (kState.IsKeyDown(Keys.Back))
                    lstInputs.Add(Input.Back);       
            }

            // Gamepad
            gState = GamePad.GetState(PlayerIndex.One);
            if (gState.IsConnected)
            {
                // Left Stick
                if(gState.ThumbSticks.Left.X < -DeadzoneSticks)
                {
                    lstInputs.Add(Input.LeftStickLeft);
                }

                if (gState.ThumbSticks.Left.Y > DeadzoneSticks)
                {
                    lstInputs.Add(Input.LeftStickUp);           
                }

                if (gState.ThumbSticks.Left.X > DeadzoneSticks)
                {
                    lstInputs.Add(Input.LeftStickRight);              
                }

                if (gState.ThumbSticks.Left.Y < -DeadzoneSticks)
                {
                    lstInputs.Add(Input.LeftStickDown);
                }

                // Right Stick
                if (gState.ThumbSticks.Right.X < -DeadzoneSticks)
                {
                    lstInputs.Add(Input.RightStickLeft);
                }

                if (gState.ThumbSticks.Right.Y > DeadzoneSticks)
                {
                    lstInputs.Add(Input.RightStickUp);
                }

                if (gState.ThumbSticks.Right.X > DeadzoneSticks)
                {
                    lstInputs.Add(Input.RightStickRight);
                }

                if (gState.ThumbSticks.Right.Y < -DeadzoneSticks)
                {
                    lstInputs.Add(Input.RightStickDown);
                }

                // DPad
                if (gState.IsButtonDown(Buttons.DPadLeft))
                {
                    lstInputs.Add(Input.PadLeft);
                }

                if (gState.IsButtonDown(Buttons.DPadUp))
                {
                    lstInputs.Add(Input.PadUp);
                }

                if (gState.IsButtonDown(Buttons.DPadRight))
                {
                    lstInputs.Add(Input.PadRight);
                }

                if (gState.IsButtonDown(Buttons.DPadDown))
                {
                    lstInputs.Add(Input.PadDown);
                }

                if (gState.IsButtonDown(Buttons.A))
                {
                    lstInputs.Add(Input.A);
                }

                if (gState.IsButtonDown(Buttons.B))
                {
                    lstInputs.Add(Input.B);
                }
            }

            for (int i = lstInputs.Count -1; i >= 0; --i)
            {
                if (inputsUsable[lstInputs[i]].Usable)
                {
                    inputsUsable[lstInputs[i]].Usable = false;


                    inputsUsable[lstInputs[i]].Timer.Start();
                }
                else
                {
                    lstInputs.RemoveAt(i);
                }
            }

            return lstInputs;
        }

        
    }
}
