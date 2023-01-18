using System;
using System.Collections.Generic;
using UnityEngine;

namespace CartographerOfTheLabyrinth.Environment
{
    [Serializable]
    public class EnvironmentSettings
    {
        [Header("Chunks")]
        public int CountChunksX;
        public int CountChunksZ;
        public int SizeChunk;
        public int DistantionRender;

        [Header("Prefabs")]
        public List<PrefabData> BlocksData;
        public GameObject Teleport;
    }
}
