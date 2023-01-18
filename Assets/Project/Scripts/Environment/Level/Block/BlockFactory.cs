using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment.Level.Block
{
    public class BlockFactory : PlaceholderFactory<GameObject, Block>, IFactory<GameObject, Block>
    {
        private DiContainer _container;
    
        [Inject]
        public BlockFactory(DiContainer container)
        {
            _container = container;
        }
    
        public Block Create(GameObject prefab)
        {
            return _container.InstantiatePrefabForComponent<Block>(prefab);
        }
    }
}
