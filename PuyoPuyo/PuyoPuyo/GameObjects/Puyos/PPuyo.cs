using Microsoft.Xna.Framework;
using PuyoPuyo.GameObjects.Puyos.Data;
using PuyoPuyo.Toolbox;
using System;

namespace PuyoPuyo.GameObjects.Puyos
{
    public sealed class PPuyo : IUpdateable, IDrawable
    {
        public Puyo Master { get; private set; }
        public Puyo Slave { get; private set; }

        public Orientation Orientation { get; private set; }

        public int DrawOrder => throw new NotImplementedException();
        public bool Visible => throw new NotImplementedException();
        public bool Enabled => throw new NotImplementedException();
        public int UpdateOrder => throw new NotImplementedException();

        private Point GetSlavePositionFromMaster(Puyo master, Orientation orientation)
        {
            switch (Orientation)
            {
                case Orientation.Left:
                    return new Point(master.X - 1, master.Y);
                case Orientation.Right:
                    return new Point(master.X + 1, master.Y);
                case Orientation.Top:
                    return new Point(master.X, master.Y - 1);
                case Orientation.Down:
                    return new Point(master.X, master.Y + 1);
            }

            return Point.Zero;
        }

        /// <summary>
        /// Instantiate a new puyopuyo
        /// <para/>Should be use to generate a new puyopuyo for the gameboard
        /// </summary>
        /// <param name="color">PuyoPuyo's color</param>
        /// <param name="position">PuyoPuyo's position</param>
        /// <param name="orientation">PuyoPuyo's orientation</param>
        public PPuyo(Color color, Point position, Orientation orientation)
        {
            IPuyoData data = PuyoDataFactory.Instance.Get(color);

            // Init master
            Master = new Puyo(data, position);

            // Init slave
            Point position_slave = GetSlavePositionFromMaster(Master, orientation);
            Slave = new Puyo(data, position_slave);
        }

        /// <summary>
        /// Instantiate a new puyopuyo
        /// <para/>Should be used to reunite two puyos
        /// </summary>
        /// <param name="master"></param>
        /// <param name="slave"></param>
        public PPuyo(Puyo master, Puyo slave)
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

        public int Left(double ms) { Master.Left(ms); Slave.Left(ms); return Master.X; }
        public int Right(double ms) { Master.Right(ms); Slave.Right(ms); return Master.X; }
        public int Top(double ms) { Master.Top(ms); Slave.Top(ms); return Master.Y; }
        public int Down(double ms) { Master.Down(ms); Slave.Down(ms); return Master.Y; }

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

        /// <summary>
        /// Update puyos
        /// </summary>
        /// <param name="gameTime"></param>
        public void Update(GameTime gameTime)
        {
            // Update childs
            Master.Update(gameTime);
            Slave.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            Master.Draw(gameTime);
            Slave.Draw(gameTime);
        }
    }
}
