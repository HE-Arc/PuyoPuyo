using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.GameObjects.Puyos.Data
{
    public class PuyoDataManager
    {
        #region Singleton
        private static readonly object padlock = new object();
        private static PuyoDataManager instance = null;
        public PuyoDataManager Instance
        {
            get
            {
                lock(padlock)
                {
                    if (instance is null)
                        instance = new PuyoDataManager();
                    return instance;
                }
            }
        }
        #endregion

        private PuyoDataManager()
        {

        }

        
    }
}
