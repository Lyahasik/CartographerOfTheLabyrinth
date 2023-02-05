using UnityEngine;
using Zenject;

namespace Environment.Level.Teleport
{
    public class TeleportFactory : 
        PlaceholderFactory<GameObject, Teleport>
    {
        private DiContainer _container;
    
        [Inject]
        public TeleportFactory(DiContainer container)
        {
            _container = container;
        }
    
        public override Teleport Create(GameObject prefab)
        {
            return _container.InstantiatePrefabForComponent<Teleport>(prefab);
        }
    }
}
