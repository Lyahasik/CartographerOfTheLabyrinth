using UnityEngine;

using Environment;
using Environment.Level;
using Gameplay.Items;
using Serialization.Environment.Level.Block;
using Serialization.Gameplay.Items;

namespace Serialization.Environment.Level
{
    public class LevelSerialization : MonoBehaviour
    {
        private EnvironmentObjectSerialization[] _blocks;
        private ItemSerialization[] _items;

        private void Awake()
        {
            EnvironmentSerializator.IncrementLevel();
        }

        private void Start()
        {
            CollectLevelData();
        }

        private void CollectLevelData()
        {
            LevelData data = new ();
            data.Title = gameObject.name;
        
            CollectBlocksData(ref data);
            CollectItemsData(ref data);
        
            EnvironmentSerializator.AddLevel(data);
        }

        private void CollectBlocksData(ref LevelData data)
        {
            _blocks = GetComponentsInChildren<EnvironmentObjectSerialization>();

            EnvironmentObjectData[] blocksData = new EnvironmentObjectData[_blocks.Length];

            int i = 0;
            foreach (EnvironmentObjectSerialization block in _blocks)
            {
                blocksData[i] = block.EnvironmentObjectData;
                i++;
            }

            data.ObjectsData = blocksData;
        }

        private void CollectItemsData(ref LevelData data)
        {
            _items = GetComponentsInChildren<ItemSerialization>();

            ItemData[] itemsData = new ItemData[_items.Length];

            int i = 0;
            foreach (ItemSerialization item in _items)
            {
                itemsData[i] = item.ItemData;
                i++;
            }

            data.ItemsData = itemsData;
        }
    }
}
