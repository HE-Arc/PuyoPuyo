using Microsoft.Xna.Framework.Graphics;
using PuyoPuyo.Toolbox;
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

        private readonly Dictionary<PuyoColor, IPuyoData> puyodatas;

        private PuyoDataFactory()
        {
            puyodatas = new Dictionary<PuyoColor, IPuyoData>()
            {
                { PuyoColor.Purple, new PuyoData_Purple() },
                { PuyoColor.Yellow, new PuyoData_Yellow() },
                { PuyoColor.Red, new PuyoData_Red() },
                { PuyoColor.Green, new PuyoData_Green() },
                { PuyoColor.Blue, new PuyoData_Blue() },
            };
        }

        public IPuyoData Get(PuyoColor color)
        {
            if (puyodatas.TryGetValue(color, out IPuyoData data))
                return data;
            else
                throw new Exception("This puyo color does not exist");
        }
    }

    internal class PuyoData_Purple : IPuyoData
    {
        public PuyoColor Color { get; private set; }
        public Texture2D Texture { get; private set; }

        public PuyoData_Purple()
        {
            Color = PuyoColor.Purple;
            Texture = TextureManager.Instance.TryGet<Texture2D>("PuyoPurple");
        }
    }

    internal class PuyoData_Yellow : IPuyoData
    {
        public PuyoColor Color { get; private set; }
        public Texture2D Texture { get; private set; }

        public PuyoData_Yellow()
        {
            Color = PuyoColor.Purple;
            Texture = TextureManager.Instance.TryGet<Texture2D>("PuyoYellow");
        }
    }

    internal class PuyoData_Red : IPuyoData
    {
        public PuyoColor Color { get; private set; }
        public Texture2D Texture { get; private set; }

        public PuyoData_Red()
        {
            Color = PuyoColor.Purple;
            Texture = TextureManager.Instance.TryGet<Texture2D>("PuyoRed");
        }
    }

    internal class PuyoData_Green : IPuyoData
    {
        public PuyoColor Color { get; private set; }
        public Texture2D Texture { get; private set; }

        public PuyoData_Green()
        {
            Color = PuyoColor.Purple;
            Texture = TextureManager.Instance.TryGet<Texture2D>("PuyoGreen");
        }
    }

    internal class PuyoData_Blue : IPuyoData
    {
        public PuyoColor Color { get; private set; }
        public Texture2D Texture { get; private set; }

        public PuyoData_Blue()
        {
            Color = PuyoColor.Purple;
            Texture = TextureManager.Instance.TryGet<Texture2D>("PuyoBlue");
        }
    }
}
