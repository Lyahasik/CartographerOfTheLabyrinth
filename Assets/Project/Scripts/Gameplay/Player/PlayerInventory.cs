using System.Collections.Generic;
using Gameplay.Items;
using Zenject;

namespace Gameplay.Player
{
    public class PlayerInventory : IInitializable
    {
        private readonly HashSet<int> _keys = new ();
        private readonly Dictionary<ItemType, int> _items = new ();
        
        public void Initialize()
        {
            InitItems();
        }

        private void InitItems()
        {
            _items.Add(ItemType.SpeedBuff, 0);
        }

        public void AddKey(int id)
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
        }
    }
}
