using System;
using Gameplay;
using Gameplay.Items;
using UnityEngine;

namespace Serialization.Gameplay.Items
{
    public class ItemSerialization : MonoBehaviour
    {
        [SerializeField] private ItemType _itemType;

        private ItemData _itemData;

        public ItemData ItemData => _itemData;

        private void Awake()
        {
            _itemData.LevelNumber = Int32.Parse(transform.parent.name.Replace("Level", String.Empty));
            _itemData.Type = (int) _itemType;
            _itemData.Position = new [] {transform.position.x, transform.position.z};
        }
    }
}
