using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace PuyoPuyo.GameObjects
{
    public class GameLogic : IUpdateable
    {
        public HashSet<Gameboard> Gameboards { get; private set; } = new HashSet<Gameboard>();

        public bool Enabled { get; set; }
        public int UpdateOrder { get; set; }

        public GameLogic()
        {

        }

        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        public bool RegisterGameboard(Gameboard gameboard)
        {
            return Gameboards.Add(gameboard);
        }

        public void Update(GameTime gameTime)
        {
            // Handle inputs
            // TODO: add input logic

            // Update gameboards
            foreach(Gameboard gb in Gameboards)
            {
                gb.Update(gameTime);
            }
        }
    }
}
