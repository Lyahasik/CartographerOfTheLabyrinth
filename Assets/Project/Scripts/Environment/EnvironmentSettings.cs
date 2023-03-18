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

        [Header("Materials")]
        public List<int> MaterialRanges;
        public List<Material> MaterialsSide;
        public List<Material> MaterialsTop;

        [Header("Prefabs")]
        public List<EnvironmentPrefabData> BlocksData;
    }
}
