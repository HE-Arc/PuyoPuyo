using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;
using System;
using System.Text;

namespace PuyoPuyo.Tests
{
    public static class GameboardTesting
    {
        public static bool Test()
        {
            Gameboard gb = new Gameboard(10, 20);

            Random r = new Random();

            // Fill tab
            for (int row = 0; row < gb.Rows; row++)
            {
                for (int col = 0; col < gb.Columns; col++)
                {
                    gb.Cells[row, col] = (Puyo)r.Next(0, 5);
                }
            }

            PrintArray(gb.Cells, gb.Rows, gb.Columns);

            var foo = gb.GetChains(out int[,] indexes);

            PrintArray(indexes, gb.Rows, gb.Columns);

            return true;
        }

        public static void PrintArray(Puyo[,] array, int rows, int columns)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    sb.Append(String.Format("| {0,-10} ", array[row, col].ToString()));
                }
                sb.Append("\r\n");
            }
            Console.WriteLine(sb.ToString());
        }

        public static void PrintArray(int[,] array, int rows, int columns)
        {
            StringBuilder sb = new StringBuilder();
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    sb.Append(String.Format("| {0,-10} ", array[row, col].ToString()));
                }
                sb.Append("\r\n");
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
