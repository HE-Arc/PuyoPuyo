using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace PuyoPuyo.GameObjects.Puyos.Data
{
    public class PuyoDataFactory
    {
        #region Singleton
        private static readonly object padlock = new object();
        private static PuyoDataFactory instance = null;
        public static PuyoDataFactory Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance is null)
                        instance = new PuyoDataFactory();
                    return instance;
                }
            }
        }
        #endregion

        private readonly Dictionary<Color, IPuyoData> puyodatas;

        private PuyoDataFactory()
        {
            puyodatas = new Dictionary<Color, IPuyoData>()
            {
                { Color.Purple, new PuyoData_Purple() },
                { Color.Yellow, new PuyoData_Yellow() },
                { Color.Red, new PuyoData_Red() },
                { Color.Green, new PuyoData_Green() },
                { Color.Blue, new PuyoData_Blue() },
            };
        }

        public IPuyoData Get(Color color)
        {
            if (puyodatas.TryGetValue(color, out IPuyoData data))
                return data;
            else
                throw new Exception("This puyo color does not exist");
        }
    }

    internal class PuyoData_Purple : IPuyoData
    {
        public Color Color => Color.Purple;

        public Texture2D Texture => Main.ContentManager.Load<Texture2D>("res/textures/puyos/PuyoPurple");
    }

    internal class PuyoData_Yellow : IPuyoData
    {
        public Color Color => Color.Yellow;

        public Texture2D Texture => Main.ContentManager.Load<Texture2D>("res/textures/puyos/PuyoYellow");
    }

    internal class PuyoData_Red : IPuyoData
    {
        public Color Color => Color.Red;

        public Texture2D Texture => Main.ContentManager.Load<Texture2D>("res/textures/puyos/PuyoRed");
    }

    internal class PuyoData_Green : IPuyoData
    {
        public Color Color => Color.Green;

        public Texture2D Texture => Main.ContentManager.Load<Texture2D>("res/textures/puyos/PuyoGreen");
    }

    internal class PuyoData_Blue : IPuyoData
    {
        public Color Color => Color.Blue;

        public Texture2D Texture => Main.ContentManager.Load<Texture2D>("res/textures/puyos/PuyoBlue");
    }
}
