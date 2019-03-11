using Microsoft.Xna.Framework;
using PuyoPuyo.Toolbox;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects.Puyos
{
    public class PuyoPuyo : IUpdateable, IDrawable
    {
        public Puyo Master { get; private set; }
        public Puyo Slave { get; private set; }

        public Orientation Orientation { get; private set; }

        public int DrawOrder => throw new NotImplementedException();
        public bool Visible => throw new NotImplementedException();
        public bool Enabled => throw new NotImplementedException();
        public int UpdateOrder => throw new NotImplementedException();

        public PuyoPuyo(Puyo master, Puyo slave)
        {
            this.Master = master;
            this.Slave = slave;
        }

        public event EventHandler<EventArgs> DrawOrderChanged;
        public event EventHandler<EventArgs> VisibleChanged;
        public event EventHandler<EventArgs> EnabledChanged;
        public event EventHandler<EventArgs> UpdateOrderChanged;

        /// <summary>
        /// Rotate the puyo
        /// </summary>
        /// <param name="rotation">Determine the rotation</param>
        public void Rotate(Rotation rotation)
        {
            Orientation = OrientationHandler.NewOrientation(Orientation, rotation == Rotation.Clockwise ? 1 : -1);
        }

        /// <summary>
        /// Dissolve this PuyoPuyo
        /// </summary>
        /// <returns>A tuple of the two composite puyo</returns>
        public Tuple<Puyo, Puyo> Dissolve()
        {
            return new Tuple<Puyo, Puyo>(Master, Slave);
        }
        
        /// <summary>
        /// Switch master and slave
        /// </summary>
        public void SwitchMain()
        {
            Puyo temp = Master;
            Master = Slave;
            Slave = temp;

            Orientation = OrientationHandler.GetMirrorOrientation(Orientation);
        }

        public void Draw(GameTime gameTime)
        {
            Master.Draw(gameTime);
            Slave.Draw(gameTime);
        }

        public void Update(GameTime gameTime)
        {
            // If can't move down, dissolve

            // Update childs
            Master.Update(gameTime);
            Slave.Update(gameTime);
        }
    }


}
