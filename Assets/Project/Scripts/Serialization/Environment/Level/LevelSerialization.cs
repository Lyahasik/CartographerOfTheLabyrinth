using Environment;
using Environment.Level;
using Gameplay;
using Serialization.Environment.Level.Block;
using Serialization.Gameplay.Items;
using UnityEngine;

namespace Serialization.Environment.Level
{
    public class LevelSerialization : MonoBehaviour
    {
        private BlockSerialization[] _blocks;
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
            _blocks = GetComponentsInChildren<BlockSerialization>();

            EnvironmentObjectData[] blocksData = new EnvironmentObjectData[_blocks.Length];

            int i = 0;
            foreach (BlockSerialization block in _blocks)
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
