using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.Toolbox
{
    public class TextureManager
    {
        #region Singleton
        private static readonly object padlock = new object();
        private static TextureManager instance = null;
        public static TextureManager Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance is null)
                        instance = new TextureManager();
                    return instance;
                }
            }
        }
        #endregion

        private ContentManager contentManager;
        private readonly Dictionary<string, object> textures;

        private TextureManager()
        {
            textures = new Dictionary<string, object>();
        }

        /// <summary>
        /// Simplify asset loading
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="assetName">name of the assert, without extension</param>
        /// <param name="subPath">subfolder, must end with /</param>
        /// <returns>The loaded asset</returns>
        private T Load<T>(string assetName, string subPath = "")
        {
            if (contentManager == null)
                throw new Exception("Initialize me first !");

            string directory;
            Type typeT = typeof(T);

            if (typeT == typeof(Texture2D))
                directory = "textures/";
            else if (typeT == typeof(SpriteFont))
                directory = "fonts/";
            else
                throw new Exception("Not supported asset type given !");

            return contentManager.Load<T>(directory + subPath + assetName);
        }

        public void LoadContent(ContentManager cm)
        {
            contentManager = cm;
            LoadTextures();
        }

        private void LoadTextures()
        {
            textures.Add("PuyoRed", Load<Texture2D>("R", "puyos/"));
            textures.Add("PuyoGreen", Load<Texture2D>("G", "puyos/"));
            textures.Add("PuyoBlue", Load<Texture2D>("B", "puyos/"));
            textures.Add("PuyoYellow", Load<Texture2D>("Y", "puyos/"));
            textures.Add("PuyoPurple", Load<Texture2D>("P", "puyos/"));
            textures.Add("InGameBg", Load<Texture2D>("ingame_bg", "bg/"));
        }

        public T TryGet<T>(string textureName)
        {
            if(textures.TryGetValue(textureName, out object texture))
            {
                return (T)texture;
            }
            else
            {
                throw new ArgumentException("No texture found");
            }
        }
    }
}
