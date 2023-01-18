using CartographerOfTheLabyrinth.Gameplay.Player;
using CartographerOfTheLabyrinth.OS;
using CartographerOfTheLabyrinth.UI.Map;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Installers
{
    public class GameplayPrefabsInstaller : MonoInstaller
    {
        private GameObject _parent;
    
        public PlayerMovement Player;
        public PlayerWatcher PlayerWatcher;
    
        public override void InstallBindings()
        {
            ParentInitialize();
            MovementControllerInitialize();
        
            Container
                .BindInterfacesAndSelfTo<PlayerMovement>()
                .FromComponentInNewPrefab(Player)
                .UnderTransform(_parent.transform)
                .AsSingle();
            Container
                .Bind<PlayerWatcher>()
                .FromComponentInNewPrefab(PlayerWatcher)
                .UnderTransform(_parent.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .BindInterfacesAndSelfTo<MapController>()
                .AsSingle()
                .NonLazy();
        }

        private void ParentInitialize()
        {
            _parent = GameObject.Find("Gameplay");
        
            if (_parent == null)
                _parent = new GameObject("Gameplay");
        }

        private void MovementControllerInitialize()
        {
            if (OSHandler.IsWebMobile())
                return;
        
            Container
                .BindInterfacesAndSelfTo<PCMovementController>()
                .AsSingle()
                .NonLazy();
        }
    }
}