using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PuyoPuyo.GameObjects.Puyos.Data
{
    public interface IPuyoData
    {
        Color Color { get; }
        Texture2D Texture { get; }
    }
}
