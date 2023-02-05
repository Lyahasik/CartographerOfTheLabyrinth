using System.Collections.Generic;
using UnityEngine;

using Environment.Level.Blocks;

namespace Environment.Level
{
    public class ChunkData
    {
        public Vector2Int Id;
        public bool ForRemoving;
        public List<Block> Blocks;
        public Teleport.Teleport Teleport;
    }
}