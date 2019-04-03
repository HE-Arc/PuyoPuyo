using PuyoPuyo.GameObjects;
using PuyoPuyo.Toolbox;
using System;
using System.Text;

namespace PuyoPuyo.Tests
{
    public static class GameboardTesting
    {
        public static void PrintArray(PuyoColor[,] array, int rows, int columns)
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
