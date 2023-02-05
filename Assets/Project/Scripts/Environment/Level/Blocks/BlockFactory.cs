using UnityEngine;
using Zenject;

namespace Environment.Level.Blocks
{
    public class BlockFactory : PlaceholderFactory<GameObject, Block>
    {
        private DiContainer _container;
    
        [Inject]
        public BlockFactory(DiContainer container)
        {
            _container = container;
        }
    
        public override Block Create(GameObject prefab)
        {
            return _container.InstantiatePrefabForComponent<Block>(prefab);
        }
    }
}
