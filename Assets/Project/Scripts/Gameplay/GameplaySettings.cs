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
        
        [Header("Buffs")]
        public float ScaleBoost;
        public float TimeBoost;

        [Header("Prefabs")]
        public List<ItemsPrefabData> ItemsData;
    }
}
