using UnityEngine;
using Zenject;

namespace UI.Map.Icons
{
    public class TeleportIconFactory : 
        PlaceholderFactory<Transform, int, Vector3, TeleportIcon>,
        IFactory<Transform, int, Vector3, TeleportIcon>
    {
        private DiContainer _container;
        private UISettings _settings;
    
        [Inject]
        public TeleportIconFactory(DiContainer container, UISettings settings)
        {
            _container = container;
            _settings = settings;
        }
    
        public TeleportIcon Create(Transform parent, int levelId, Vector2 position)
        {
            TeleportIcon teleportIcon = _container
                .InstantiatePrefabForComponent<TeleportIcon>(
                    _settings.PrefabTeleportIcon,
                    parent,
                    new object[] { levelId, position });

            return teleportIcon;
        }
    }
}
