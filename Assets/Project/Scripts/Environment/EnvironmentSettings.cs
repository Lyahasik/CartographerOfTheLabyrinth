using System;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
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
        public List<EnvironmentPrefabData> BlocksData;
        public GameObject Teleport;
    }
}
