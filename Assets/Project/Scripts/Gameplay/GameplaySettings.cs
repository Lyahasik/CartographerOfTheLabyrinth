using System;
using System.Collections.Generic;
using Gameplay.Items;
using UnityEngine;

namespace Gameplay
{
    [Serializable]
    public class GameplaySettings
    {
        [Header("Movement")]
        public float MouseSensitivity;
        
        [Header("Prefabs")]
        public List<ItemsPrefabData> ItemsData;
    }
}
