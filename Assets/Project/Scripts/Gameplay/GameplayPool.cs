using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay.Items;

namespace Gameplay
{
    public class GameplayPool : IInitializable
    {
        private ItemFactory _itemFactory;
    
        private GameObject _parent;
        private GameObject _gameplayPool;
    
        private Dictionary<ItemType, Stack<Item>> _items = new ();
    
        public GameplayPool(GameObject parent)
        {
            _parent = parent;
        }
    
        [Inject]
        public void Construct(ItemFactory itemFactory)
        {
            _itemFactory = itemFactory;
        }
    
        public void Initialize()
        {
            _gameplayPool = new GameObject("GameplayPool");
            _gameplayPool.transform.parent = _parent.transform;
        }

        public Item GetItem(ItemType itemType)
        {
            if (!_items.ContainsKey(itemType))
            {
                _items.Add(itemType, new Stack<Item>());
            }
        
            if (_items[itemType].Count <= 0)
            {
                return _itemFactory.Create(itemType);
            }
        
            Item poolingObject = _items[itemType].Pop();
            poolingObject.gameObject.SetActive(true);

            return poolingObject;
        }
    
        public void ReturnItem(Item item)
        {
            item.gameObject.SetActive(false);
            item.transform.parent = _gameplayPool.transform;
        
            _items[item.Type].Push(item);
        }
    }
}
