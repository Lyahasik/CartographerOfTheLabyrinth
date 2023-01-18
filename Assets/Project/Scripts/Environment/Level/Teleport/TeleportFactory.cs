using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment.Level.Teleport
{
    public class TeleportFactory : 
        PlaceholderFactory<GameObject, Teleport>, 
        IFactory<GameObject, Teleport>
    {
        private DiContainer _container;
    
        [Inject]
        public TeleportFactory(DiContainer container)
        {
            _container = container;
        }
    
        public Teleport Create(GameObject prefab)
        {
            return _container.InstantiatePrefabForComponent<Teleport>(prefab);
        }
    }
}
