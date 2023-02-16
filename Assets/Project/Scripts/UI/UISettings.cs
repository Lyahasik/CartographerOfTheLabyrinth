using System;
using UnityEngine;

namespace UI.Map
{
    [Serializable]
    public class UISettings
    {
        [Header("Main")]
        public float MessageTimeLife;
        
        [Header("Map")]
        public float PixelsPerUnit;

        public GameObject PrefabTeleportIcon;
    }
}
