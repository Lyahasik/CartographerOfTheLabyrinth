using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay;
using Gameplay.Buffs;
using Gameplay.Education;
using Gameplay.Items;
using Gameplay.Player;
using Gameplay.Progress;
using OS;

namespace Installers
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
                .BindInterfacesAndSelfTo<PlayerInventory>()
                .AsSingle()
                .NonLazy();
        
            Container
                .BindInterfacesAndSelfTo<GameMashine>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindInterfacesAndSelfTo<GameplayHandler>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<GameplayPool>()
                .AsSingle()
                .WithArguments(_parent);
            
            Container
                .BindInterfacesAndSelfTo<BuffsHandler>()
                .AsSingle()
                .NonLazy();
            
            Container
                .BindFactory<ItemType, Item, ItemFactory>()
                .FromFactory<ItemFactory>();

            Container
                .BindInterfacesAndSelfTo<ProcessingProgress>()
                .AsSingle()
                .NonLazy();
        
            Container
                .BindInterfacesAndSelfTo<EducationHandler>()
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