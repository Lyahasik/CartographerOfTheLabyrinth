using System;
using System.Collections.Generic;
using Zenject;

using Gameplay.Items;
using Gameplay.Progress;

namespace Gameplay.Player
{
    public class PlayerInventory
    {
        private ProcessingProgress _processingProgress;
        
        private HashSet<int> _teleportKeys;
        private Dictionary<ItemType, int> _items;

        public static event Action<ItemType, int> OnSetNumberItem;

        [Inject]
        public void Construct(ProcessingProgress processingProgress)
        {
            _processingProgress = processingProgress;
        }

        public void InitItems(Dictionary<ItemType, int> notUsedItems, HashSet<int> notUsedTeleportKeys)
        {
            _items = notUsedItems;
            _teleportKeys = notUsedTeleportKeys;
            
            OnSetNumberItem?.Invoke(ItemType.SpeedBuff, _items[ItemType.SpeedBuff]);
            OnSetNumberItem?.Invoke(ItemType.VisibilityRangeBuff, _items[ItemType.VisibilityRangeBuff]);
        }

        public void AddTeleportKey(int id)
        {
            _teleportKeys.Add(id);
        }

        public bool TryFindKey(int id)
        {
            bool isContained = _teleportKeys.Contains(id);

            if (isContained)
            {
                _teleportKeys.Remove(id);
                _processingProgress.SaveProgressData();
            }

            return isContained;
        }

        public void AddItem(ItemType type, int number = 1)
        {
            _items[type] += number;
            OnSetNumberItem?.Invoke(type, _items[type]);
        }

        public int GetNumberItem(ItemType type)
        {
            return _items[type];
        }

        public bool ContainsItem(ItemType type)
        {
            if (_items[type] <= 0)
                return false;

            return true;
        }

        public void UseItem(ItemType type)
        {
            _items[type]--;
            _processingProgress.SaveProgressData();;
            
            OnSetNumberItem?.Invoke(type, _items[type]);
        }
    }
}
