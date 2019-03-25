using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuyoPuyo.Toolbox;

namespace PuyoPuyo.Toolbox
{
    class InputManager
    {
        private KeyboardState kState;
        private GamePadState gState;
        private List<Input> inputs = new List<Input>();

        public float DeadzoneSticks = 0.25f;

        public InputManager()
        {
            //kState = new KeyboardState();
        }

        public Input Perform(bool controlIndex)
        {
            kState = Keyboard.GetState();
            gState = GamePad.GetState(PlayerIndex.One);

            if (kState != null)
            {
                //System.Text.StringBuilder sb = new System.Text.StringBuilder();

                //foreach (var key in kState.GetPressedKeys())
                //    sb.Append("Keys: ").Append(key).Append(" pressed");

                //if (sb.Length > 0)
                //    System.Diagnostics.Debug.WriteLine(sb.ToString());
                //else
                //    System.Diagnostics.Debug.WriteLine("No Keys pressed");

                if (kState.IsKeyDown(Keys.Left) && !inputs.Contains(Input.Left))
                {
                    inputs.Add(Input.Left);
                    //return Input.Left;
                }

                if (kState.IsKeyDown(Keys.Up) && !inputs.Contains(Input.Up))
                {
                    inputs.Add(Input.Up);
                    //return Input.Up;
                }

                if (kState.IsKeyDown(Keys.Right) && !inputs.Contains(Input.Right))
                {
                    inputs.Add(Input.Up);
                    //return Input.Right;
                }

                if (kState.IsKeyDown(Keys.Down) && !inputs.Contains(Input.Down))
                {
                    inputs.Add(Input.Up);
                    //return Input.Down;
                }
            }

            if(gState.IsConnected)
            {
                // Left Stick
                if(gState.ThumbSticks.Left.X < -DeadzoneSticks && !inputs.Contains(Input.LeftStickLeft))
                {
                    inputs.Add(Input.LeftStickLeft);
                    //return Input.LeftStickLeft;
                }

                if (gState.ThumbSticks.Left.Y > DeadzoneSticks && !inputs.Contains(Input.LeftStickUp))
                {
                    inputs.Add(Input.LeftStickUp);
                    //return Input.LeftStickUp;                 
                }

                if (gState.ThumbSticks.Left.X > DeadzoneSticks && !inputs.Contains(Input.LeftStickUp))
                {
                    inputs.Add(Input.LeftStickUp);
                    //return Input.LeftStickUp;                
                }

                if (gState.ThumbSticks.Left.Y < -DeadzoneSticks && !inputs.Contains(Input.LeftStickDown))
                {
                    inputs.Add(Input.LeftStickDown);
                    //return Input.LeftStickDown;
                }

                // Right Stick
                // Left Stick
                if (gState.ThumbSticks.Right.X < -DeadzoneSticks && !inputs.Contains(Input.LeftStickDown))
                {
                    inputs.Add(Input.RightStickLeft);
                    //return Input.RightStickLeft;
                }

                if (gState.ThumbSticks.Right.Y > DeadzoneSticks && !inputs.Contains(Input.LeftStickDown))
                {
                    inputs.Add(Input.RightStickUp);
                    //return Input.RightStickUp;
                }

                if (gState.ThumbSticks.Right.X > DeadzoneSticks && !inputs.Contains(Input.LeftStickDown))
                {
                    inputs.Add(Input.RightStickRight);
                    //return Input.RightStickRight;
                }

                if (gState.ThumbSticks.Right.Y < -DeadzoneSticks && !inputs.Contains(Input.LeftStickDown))
                {
                    inputs.Add(Input.RightStickDown);
                    //return Input.RightStickDown;
                }

                // DPad
                if (gState.IsButtonDown(Buttons.DPadLeft) && !inputs.Contains(Input.Left))
                {
                    inputs.Add(Input.Left);
                    //return Input.Left;
                }

                if (gState.IsButtonDown(Buttons.DPadUp) && !inputs.Contains(Input.Up))
                {
                    inputs.Add(Input.Up);
                    //return Input.Up;
                }

                if (gState.IsButtonDown(Buttons.DPadRight) && !inputs.Contains(Input.Right))
                {
                    inputs.Add(Input.Right);
                    //return Input.Right;
                }

                if (gState.IsButtonDown(Buttons.DPadDown) && !inputs.Contains(Input.Down))
                {
                    inputs.Add(Input.Down);
                    //return Input.Down;
                }

                if (gState.IsButtonDown(Buttons.A) && !inputs.Contains(Input.A))
                {
                    //Console.WriteLine("A");
                    inputs.Add(Input.A);
                    //return Input.A;
                }

                if (gState.IsButtonDown(Buttons.B) && !inputs.Contains(Input.B))
                {
                    //Console.WriteLine("B");
                    inputs.Add(Input.B);
                    //return Input.B;
                }
            }

            Console.WriteLine(inputs.Count);
            foreach (Input i in inputs)
            {
                switch (i)
                {
                    case Input.Left:
                    case Input.LeftStickLeft:
                        if (gState.ThumbSticks.Left.X < -DeadzoneSticks || kState.IsKeyDown(Keys.Left))
                        {
                            //Console.WriteLine("Left returned");
                            inputs.Remove(Input.Left);
                            inputs.Remove(Input.LeftStickLeft);
                            return Input.Left;
                        }
                        break;

                    case Input.Up:
                    case Input.LeftStickUp:
                        if (gState.ThumbSticks.Left.Y > DeadzoneSticks || kState.IsKeyDown(Keys.Up))
                        {
                            inputs.Remove(Input.Up);
                            inputs.Remove(Input.LeftStickUp);
                            return Input.Up;
                        }
                        break;

                    case Input.Right:
                    case Input.LeftStickRight:
                        if (gState.ThumbSticks.Left.X > DeadzoneSticks || kState.IsKeyDown(Keys.Right))
                        {
                            inputs.Remove(Input.Right);
                            inputs.Remove(Input.LeftStickRight);
                            return Input.Right;
                        }
                        break;

                    case Input.Down:
                    case Input.LeftStickDown:
                        if (gState.ThumbSticks.Left.Y < -DeadzoneSticks || kState.IsKeyDown(Keys.Down))
                        {
                            inputs.Remove(Input.Down);
                            inputs.Remove(Input.LeftStickDown);
                            return Input.Down;
                        }
                        break;

                    case Input.Enter:
                    case Input.A:
                        if (gState.IsButtonDown(Buttons.A))
                        {
                            //Console.WriteLine("A returned");
                            inputs.Remove(Input.A);
                            return Input.A;
                        }
                        break;

                    case Input.B:
                        if(gState.IsButtonDown(Buttons.B))
                        {
                            //Console.WriteLine("B returned");
                            inputs.Remove(Input.B);
                            return Input.B;
                        }
                        break;
                }
            }

            return Input.None;
        }
    }
}
