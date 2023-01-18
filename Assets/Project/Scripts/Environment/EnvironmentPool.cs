using System.Collections.Generic;
using CartographerOfTheLabyrinth.Environment.Level.Block;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment
{
    public class EnvironmentPool : IInitializable
    {
        private DiContainer _container;
        private EnvironmentSettings _settings;
        private BlockFactory _blockFactory;

        private GameObject _parent;
        private GameObject _environmentPool;

        private Dictionary<EnvironmentObjectType, GameObject> _prefabs = new ();
        private Dictionary<EnvironmentObjectType, Stack<Block>> _blocks = new ();

        public EnvironmentPool(GameObject parent)
        {
            _parent = parent;
        }

        [Inject]
        public void Construct(EnvironmentSettings settings, BlockFactory blockFactory)
        {
            _settings = settings;
            _blockFactory = blockFactory;
        }

        public void Initialize()
        {
            _environmentPool = new GameObject("EnvironmentPool");
            _environmentPool.transform.parent = _parent.transform;
            
            CollectPrefabs();
        }

        private void CollectPrefabs()
        {
            foreach (PrefabData prefabData in _settings.BlocksData)
            {
                _prefabs.Add(prefabData.EnvironmentObjectType, prefabData.BlockPrefab);
            }
        }

        public Block GetBlock(EnvironmentObjectType environmentObjectType)
        {
            if (!_blocks.ContainsKey(environmentObjectType))
            {
                _blocks.Add(environmentObjectType, new Stack<Block>());
            }
        
            if (_blocks[environmentObjectType].Count <= 0)
            {
                return _blockFactory.Create(_prefabs[environmentObjectType]);
            }
        
            Block poolingObject = _blocks[environmentObjectType].Pop();
            poolingObject.gameObject.SetActive(true);

            return poolingObject;
        }

        public void ReturnBlock(Block block)
        {
            block.gameObject.SetActive(false);
            block.transform.parent = _environmentPool.transform;
        
            _blocks[block.Type].Push(block);
        }
    }
}
