using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.GameObjects.Puyos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects.Grids
{
    public class Grid
    {
        private readonly int rows;
        private readonly int columns;
        private readonly Cell[,] cells;

        public int Rows => rows;
        public int Columns => columns;

        /// <summary>
        /// Build a grid
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Grid(int rows, int columns)
        {
            // Logic elements init
            this.rows = rows > 0 ? rows : throw new ArgumentException("Invalid row count");
            this.columns = columns > 0 ? columns : throw new ArgumentException("Invalid column count");

            // Create the grid
            cells = new Cell[Rows, Columns];

            // Row-major
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    cells[row, col] = Cell.Create(this, row, col);
                }
            }
        }

        /// <summary>
        /// Give access to cells
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="column">column</param>
        /// <returns>null if out of range</returns>
        public Cell this[int row, int column]
        {
            get {
                if (column < 0 || row < 0 || column >= Columns || row >= Rows)
                {
                    return null;
                }
                else
                {
                    return cells[row, column];
                }   
            }
        }

        /// <summary>
        /// Enables iteration over cells
        /// </summary>
        /// <returns>Enumerator on cells contained in this grid</returns>
        public IEnumerator<Cell> GetEnumerator()
        {
            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Columns; col++)
                {
                    yield return cells[row, col];
                }
            }
        }

        /// <summary>
        /// Enables iteration over puyos
        /// </summary>
        /// <returns>Enumerator on puyos inside this grid</returns>
        public IEnumerator<Puyo> GetPuyos()
        {
            foreach(Cell cell in this)
            {
                if (cell == null || cell.IsFree)
                    continue;
                yield return cell.Puyo;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int X, int Y, int SizeBoardCase)
        {
            RectangleSprite.DrawRectangle(spriteBatch, new Rectangle(X, Y, SizeBoardCase, SizeBoardCase), Color.Red, 2);
        }
    }

    class RectangleSprite
    {
        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }
    }
}
