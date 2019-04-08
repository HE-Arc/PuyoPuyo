using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.Toolbox
{
    public class ScoreManager
    {
        // Score base on Puyo Puyo Tsu
        // https://puyonexus.com/wiki/Scoring


        /* Pour quentîn
         * Concept :
         * Segan instancie un ScoreManager par boardgame
         * Lorsqu'il explose des puyos, il fait des add()
         * Quand il a finit, il fait un calculate qui
         * calcule le score, calcule la nuisance
         * et vide les listes pour le coup suivant */

        private List<PuyoColor> lstPuyoColors;
        private List<int> lstGroup;
        private bool versus;
        private static readonly int[] pondColorBonus = { 0, 3, 6, 12, 24 };
        private static readonly int[] pondGroupBonus = { 0, 2, 3, 4, 5, 6, 7, 10 };
        private static readonly int[] pondChainBonus = { 0, 8, 16, 32, 64, 96, 128, 160, 192, 224, 256, 288, 320, 352, 384, 416, 448, 480, 512, 544, 576, 608, 640, 672 };
        private static readonly int[] pondChainBonusVersus = { 4, 20, 24, 32, 48, 96, 160, 240, 320, 480, 600, 700, 800, 900, 999 };
        public bool ClearedBoard { get; set; }
        public int Score { get; internal set; }

        public ScoreManager(bool versus = false)
        {
            lstPuyoColors = new List<PuyoColor>();
            lstGroup = new List<int>();
            Score = 0;
            this.versus = versus;
        }

        public void Add(PuyoColor puyoColor, int group)
        {
            if (!lstPuyoColors.Contains(puyoColor))
                lstPuyoColors.Add(puyoColor);

            lstGroup.Add(group);
        }

        public void CalculateScore()
        {
            int colorBonus = (lstPuyoColors.Count > pondColorBonus.Length) ? pondColorBonus.Last() : pondColorBonus[lstPuyoColors.Count - 1];

            int groupBonus = 0;
            int puyoBonus = 0; // TODO
            foreach (int nbElement in lstGroup)
            {
                groupBonus += (nbElement > pondGroupBonus.Length) ? pondGroupBonus.Last() : pondGroupBonus[nbElement - 1];
            }

            int chainBonus = (versus) ? (lstGroup.Count > pondChainBonus.Length) ? pondChainBonus.Last() : pondChainBonus[lstGroup.Count - 1] : (lstGroup.Count > pondChainBonusVersus.Length) ? pondChainBonusVersus.Last() : pondChainBonusVersus[lstGroup.Count - 1];

            int bonus = colorBonus + groupBonus + chainBonus;

            bonus = (bonus < 1) ? 1 : (bonus > 999) ? 999 : bonus;

            Score = (10 * lstGroup.Sum() + puyoBonus) * bonus;

            lstGroup.Clear();
            lstPuyoColors.Clear();
        }

        public void Nuisance()
        {
            // TODO
        }

        public void Draw(SpriteBatch spritebatch, SpriteFont font, Vector2 position)
        {
            spritebatch.DrawString(font, "Score : " + Score, position, Color.White);
        }
    }
}
