using PuyoPuyo.Toolbox;
using Microsoft.Xna.Framework.Graphics;

namespace PuyoPuyo.GameObjects.Puyos.Data
{
    public interface IPuyoData
    {
        PuyoColor Color { get; }
        Texture2D Texture { get; }
    }
}
