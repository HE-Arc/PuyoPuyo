using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos.Data;
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

        private Vector2 GetSlavePositionFromMaster(Puyo master, Orientation orientation)
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    return master.Position - Vector2.UnitX;
                case Orientation.Right:
                    return master.Position + Vector2.UnitX;
                case Orientation.Top:
                    return master.Position - Vector2.UnitY;
                case Orientation.Down:
                    return master.Position + Vector2.UnitX;
            }

            return Vector2.Zero;
        }

        /// <summary>
        /// Instantiate a new puyopuyo
        /// <para/>Should be use to generate a new puyopuyo for the gameboard
        /// </summary>
        /// <param name="color">PuyoPuyo's color</param>
        /// <param name="position">PuyoPuyo's position</param>
        /// <param name="orientation">PuyoPuyo's orientation</param>
        public PuyoPuyo(Color color, Point position, Orientation orientation)
        {
            IPuyoData data = PuyoDataFactory.Instance.Get(color);

            // Init master
            Vector2 position_master = position.ToVector2();
            Master = new Puyo(data, position_master);

            // Init slave
            Vector2 position_slave = GetSlavePositionFromMaster(Master, orientation);
            Slave = new Puyo(data, position_slave);
        }

        /// <summary>
        /// Instantiate a new puyopuyo
        /// <para/>Should be used to reunite two puyos
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slave"></param>
        public PuyoPuyo(Puyo master, Puyo slave)
        {
            if (!master.Data.Color.Equals(slave.Data.Color))
                throw new ArgumentException("Puyos must be of same color");

            // TODO: Verifiy that they are neighbors

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
