using PuyoPuyo.GameObjects.Grids;
using PuyoPuyo.GameObjects.Puyos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuyoPuyo.Toolbox
{
    public static class GridHelper
    {
        public static Dictionary<PuyoColor, List<IEnumerable<Puyo>>> GetChains(Grid grid, out int chainCount, int minimumLenght = 4)
        {
            // Init result's dictionary
            Dictionary<PuyoColor, List<IEnumerable<Puyo>>> chains = new Dictionary<PuyoColor, List<IEnumerable<Puyo>>>();
            foreach(PuyoColor puyoColor in Enum.GetValues(typeof(PuyoColor)))
            {
                chains.Add(puyoColor, new List<IEnumerable<Puyo>>());
            }

            // Tools
            HashSet<Puyo> knownPuyos = new HashSet<Puyo>();
            Puyo currentPuyo;

            // Iterate over puyos
            var puyoEnumerator = grid.GetPuyos();
            while(puyoEnumerator.MoveNext())
            {
                // Get puyo
                currentPuyo = puyoEnumerator.Current;

                // Puyo already in a piece
                if (knownPuyos.Contains(currentPuyo)) continue;

                // Create a new piece
                var piece = GetPiece(currentPuyo);

                // Add every puyo of the chain to knowns puyo
                foreach (Puyo p in piece)
                    knownPuyos.Add(p);

                // Check if piece is valid and append it to chains
                if (piece.Count >= minimumLenght)
                    chains[currentPuyo.Color].Add(piece);
            }

            // Get number of chains
            chainCount = chains.Values.Where(pieces => pieces.Count > 0).Count();

            // Return found chains
            return chains;
        }

        private static HashSet<Puyo> GetPiece(Puyo puyo)
        {
            HashSet<Puyo> chain = new HashSet<Puyo>();
            FillChain(puyo, ref chain);
            return chain;
        }

        private static void FillChain(Puyo puyo, ref HashSet<Puyo> chain)
        {
            // Check if puyo is known, try to add it
            if (!chain.Add(puyo)) return;

            // Get neighbors
            List<Puyo> neighbors = puyo.GetNeighbors();
            if (neighbors.Count == 0) return;
            
            foreach(Puyo neighbor in neighbors)
            {
                if (neighbor.Color == puyo.Color || neighbor.Color == PuyoColor.Any)
                {
                    FillChain(neighbor, ref chain);
                }
            }
        }
    }
}
