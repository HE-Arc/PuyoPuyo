using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using System;

namespace PuyoPuyo.screen
{
    public class MenuItem
    {
        public MenuItem(SpriteFont font, string text)
        {
            Text = text;
            Font = font;
            Color = Color.White;
        }

        public SpriteFont Font { get; }
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public RectangleF BoundingRectangle => new RectangleF(Position, Font.MeasureString(Text));
        public Action Action { get; set; }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(Font, Text, Position, Color);
        }

    }
}
