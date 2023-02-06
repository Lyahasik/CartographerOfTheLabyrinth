using System;
using System.Collections.Generic;
using UnityEngine;

using Gameplay.Items;

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
        public Vector3 FollowOffsetUp;
        public float SpeedChange;
        public float TimeFollowOffsetUp;

        [Header("Prefabs")]
        public List<ItemsPrefabData> ItemsData;
    }
}
