using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.Toolbox
{
    class InputManager
    {
        private KeyboardState kState;
        private GamePadState gamePadState;

        public InputManager()
        {
            kState = new KeyboardState();
        }

        public void Update(ref Vector2 position)
        {
            kState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (kState != null)
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder();

                foreach (var key in kState.GetPressedKeys())
                    sb.Append("Keys: ").Append(key).Append(" pressed");

                if (sb.Length > 0)
                    System.Diagnostics.Debug.WriteLine(sb.ToString());
                else
                    System.Diagnostics.Debug.WriteLine("No Keys pressed");

                if (kState.IsKeyDown(Keys.Right))
                    position.X += 10;
                if (kState.IsKeyDown(Keys.Left))
                    position.X -= 10;
                if (kState.IsKeyDown(Keys.Up))
                    position.Y -= 10;
                if (kState.IsKeyDown(Keys.Down))
                    position.Y += 10;
            }

            if(gamePadState != null)
            {
                gamePadState = GamePad.GetState(PlayerIndex.One);

                //...
            }
        }
    }
}
