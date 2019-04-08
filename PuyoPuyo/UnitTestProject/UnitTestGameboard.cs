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

            Gameboard gameboard = new Gameboard(columns, rows);

            Assert.IsTrue(gameboard.Columns == columns);
            Assert.IsTrue(gameboard.Rows == rows);
        }

        [TestMethod]
        public void TestGameboardInvalidColumnCount()
        {
            int columns = -1;
            int rows = 10;

            Assert.ThrowsException<ArgumentException>(() => new Gameboard(columns, rows));
        }

        [TestMethod]
        public void TestGameboardInvalidRowCount()
        {
            int columns = 10;
            int rows = -1;

            Assert.ThrowsException<ArgumentException>(() => new Gameboard(columns, rows));
        }

        [TestMethod]
        public void TestGameboardValidSpawn()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows);

            // Spawn a puyo
            gameboard.Spawn(Puyo.Red);

            // Validate its color
            Assert.IsTrue(gameboard.Player.Color == Puyo.Red);
        }

        [TestMethod]
        public void TestGameboardInvalidSpawn()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows);

            // Validated that it is not possible to spawn a undefined puyo
            Assert.ThrowsException<ArgumentException>(() => gameboard.Spawn(Puyo.Undefined));
        }

        [TestMethod]
        public void TestGameboardInvalidSpawnPlayerAlreadyAlive()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows);

            // Spawn a puyo
            gameboard.Spawn(Puyo.Red);

            // Validated that it is not possible to spawn a puyo while one is already alive
            Assert.ThrowsException<PlayerException>(() => gameboard.Spawn(Puyo.Red));
        }

        public void TestGameboardSpawnAndRotate()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows);

            // Spawn a puyo
            gameboard.Spawn(Puyo.Red);

            // Get slave position
            Point previousSlavePosition = gameboard.Player.Slave;

            // Rotate the player
            gameboard.Player.Orientation = Orientation.Left;

            // Validated rotation
            Assert.IsTrue((new Point(previousSlavePosition.X - 1, previousSlavePosition.Y + 1) == gameboard.Player.Master));
        }

        public void TestGameboardSpawnAndMove()
        {
            int columns = 10;
            int rows = 10;

            // Init gameboard
            Gameboard gameboard = new Gameboard(columns, rows);

            // Spawn a puyo
            gameboard.Spawn(Puyo.Red);

            // Move the puyo
            gameboard.Right();
            gameboard.Down();
            gameboard.Left();
            gameboard.Down();

            // Validated new position
            Assert.IsTrue((new Point(columns / 2, rows - 2) == gameboard.Player.Master));
        }
    }
}
