using System;
using System.Collections.Generic;
using Zenject;

using Gameplay.Items;

namespace Gameplay.Player
{
    public class PlayerInventory : IInitializable
    {
        private readonly HashSet<int> _keys = new ();
        private readonly Dictionary<ItemType, int> _items = new ();

        public static event Action<ItemType, int> OnSetNumberItem;

        public void Initialize()
        {
            InitItems();
        }

        private void InitItems()
        {
            _items.Add(ItemType.DoorKey, 0);
            
            _items.Add(ItemType.SpeedBuff, 0);
            OnSetNumberItem?.Invoke(ItemType.SpeedBuff, 0);
            
            _items.Add(ItemType.VisibilityRangeBuff, 0);
            OnSetNumberItem?.Invoke(ItemType.VisibilityRangeBuff, 0);
        }

        public void AddTeleportKey(int id)
        {
            _keys.Add(id);
        }

        public bool TryFindKey(int id)
        {
            bool isContained = _keys.Contains(id);
        
            if (isContained)
                _keys.Remove(id);

            return isContained;
        }

        public void AddItem(ItemType type)
        {
            _items[type] += 1;
            OnSetNumberItem?.Invoke(type, _items[type]);
        }

        public bool ContainsItem(ItemType type)
        {
            if (_items[type] <= 0)
                return false;

            return true;
        }

        public void UseItem(ItemType type)
        {
            _items[type] -= 1;
            
            OnSetNumberItem?.Invoke(type, _items[type]);
        }
    }
}
