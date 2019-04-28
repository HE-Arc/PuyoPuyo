using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using PuyoPuyo.Exceptions;
using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTestGameboard
    {
        [TestMethod]
        public void TestGameboardValid()
        {
            int columns = 10;
            int rows = 10;

            Gameboard gameboard = new Gameboard(columns, rows, 50, 0);

            Assert.IsTrue(gameboard.Grid.Columns == columns);
            Assert.IsTrue(gameboard.Grid.Rows == rows);
        }

        [TestMethod]
        public void TestGameboardInvalidColumnCount()
        {
            int columns = -1;
            int rows = 10;

            Assert.ThrowsException<ArgumentException>(() => new Gameboard(columns, rows, 50, 0));
        }

        [TestMethod]
        public void TestGameboardInvalidRowCount()
        {
            int columns = 10;
            int rows = -1;

            Assert.ThrowsException<ArgumentException>(() => new Gameboard(columns, rows, 50, 0));
        }

        [TestMethod]
        public void TestGameboardInvalidSpawn()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows, 50, 0);

            // Validated that it is not possible to spawn a undefined puyo
            Assert.ThrowsException<ArgumentException>(() => gameboard.Spawn(new Tuple<PuyoColor, PuyoColor>(PuyoColor.Red, PuyoColor.Any)));
        }

        public void TestGameboardSpawnAndRotate()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows, 50, 0);

            // Spawn a puyo
            gameboard.Spawn(new Tuple<PuyoColor, PuyoColor>(PuyoColor.Red, PuyoColor.Red));

            // Get slave position
            Point previousSlavePosition = new Point(gameboard.Player.Slave.Row, gameboard.Player.Slave.Column);

            // Rotate the player
            gameboard.Player.Rotate(Rotation.Clockwise);

            Point newSlavePosition = new Point(gameboard.Player.Slave.Row, gameboard.Player.Slave.Column);

            // Validated rotation
            Assert.IsTrue(newSlavePosition == previousSlavePosition);
        }

        public void TestGameboardSpawnAndMove()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows, 50, 0);

            // Spawn a puyo
            gameboard.Spawn(new Tuple<PuyoColor, PuyoColor>(PuyoColor.Red, PuyoColor.Red));

            // Move the puyo
            gameboard.Right();
            gameboard.Down();
            gameboard.Left();
            gameboard.Down();

            Point master_pos = new Point(gameboard.Player.Master.Row, gameboard.Player.Master.Column);

            // Validated new position
            Assert.IsTrue((new Point(columns / 2, rows - 2) == master_pos));
        }
    }
}
