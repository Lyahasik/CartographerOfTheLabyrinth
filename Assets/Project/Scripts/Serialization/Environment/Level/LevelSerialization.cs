using CartographerOfTheLabyrinth.Environment;
using CartographerOfTheLabyrinth.Environment.Level;
using CartographerOfTheLabyrinth.Serialization.Environment.Level.Block;
using UnityEngine;

namespace CartographerOfTheLabyrinth.Serialization.Environment.Level
{
    public class LevelSerialization : MonoBehaviour
    {
        private BlockSerialization[] _blocks;

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
    }
}
