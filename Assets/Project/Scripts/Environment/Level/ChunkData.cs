using System.Collections.Generic;
using UnityEngine;

namespace CartographerOfTheLabyrinth.Environment.Level
{
    public class ChunkData
    {
        public Vector2Int Id;
        public bool ForRemoving;
        public List<Block.Block> Blocks;
        public Teleport.Teleport Teleport;
    }
}