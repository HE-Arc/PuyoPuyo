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
        public static Dictionary<PuyoColor, List<List<Puyo>>> GetChains(Grid grid, out int chainCount, int minimumLenght = 4)
        {
            // Init result's dictionary
            Dictionary<PuyoColor, List<List<Puyo>>> chains = new Dictionary<PuyoColor, List<List<Puyo>>>();
            foreach(PuyoColor puyoColor in Enum.GetValues(typeof(PuyoColor)))
            {
                chains.Add(puyoColor, new List<List<Puyo>>());
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
                if (knownPuyos.Contains(currentPuyo))
                    continue;
                else
                {
                    // Add it to knows puyos
                    knownPuyos.Add(currentPuyo);

                    // Create a new piece
                    List<Puyo> piece = new List<Puyo>()
                    {
                        currentPuyo
                    };

                    // Iterate over neighbor
                    piece.AddRange(GetAllNeighbors(currentPuyo));

                    // Check if piece is valid and append it to chains
                    if (piece.Count >= minimumLenght)
                        chains[currentPuyo.Color].Add(piece);
                }
            }

            // Get number of chains
            chainCount = chains.Values.Where(pieces => pieces.Count > 0).Count();

            // Return found chains
            return chains;
        }

        private static List<Puyo> GetAllNeighbors(Puyo puyo)
        {
            return GetAllNeighbors(puyo, new HashSet<Puyo>());
        }

        private static List<Puyo> GetAllNeighbors(Puyo puyo, HashSet<Puyo> knowns)
        {
            // Check if puyo is known, try to add it
            if (!knowns.Add(puyo)) return new List<Puyo>();

            // Get neighbors
            List<Puyo> neighbors = puyo.GetNeighbors();
            if (neighbors.Count == 0) return new List<Puyo>();

            List<Puyo> chain = new List<Puyo>();

            // Iterate over neighbors
            // TODO : return what and when ?
            // throw new NotImplementedException();
            
            foreach(Puyo neighbor in neighbors)
            {
                if (neighbor.Color == puyo.Color)
                {
                    chain.Add(neighbor);
                    chain.AddRange(GetAllNeighbors(neighbor, knowns));
                }
            }

            return chain;
        }
    }
}
