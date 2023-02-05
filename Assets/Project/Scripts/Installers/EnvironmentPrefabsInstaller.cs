using Environment;
using Environment.Level;
using Environment.Level.Blocks;
using Environment.Level.Teleport;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class EnvironmentPrefabsInstaller : MonoInstaller
    {
        private GameObject _parent;
    
        public override void InstallBindings()
        {
            ParentInitialize();
        
            Container
                .BindInterfacesAndSelfTo<EnvironmentPool>()
                .AsSingle()
                .WithArguments(_parent);
            Container
                .BindInterfacesAndSelfTo<TeleportPool>()
                .AsSingle()
                .WithArguments(_parent);
        
            Container
                .BindInterfacesAndSelfTo<LevelHandler>()
                .AsSingle()
                .WithArguments(_parent);
            Container
                .BindInterfacesAndSelfTo<TeleportHandler>()
                .AsSingle();
            Container
                .BindInterfacesAndSelfTo<EnvironmentHandler>()
                .AsSingle();
        
            Container
                .BindFactory<GameObject, Block, BlockFactory>()
                .FromFactory<BlockFactory>();
            Container
                .BindFactory<GameObject, Teleport, TeleportFactory>()
                .FromFactory<TeleportFactory>();
        }
    
        private void ParentInitialize()
        {
            _parent = GameObject.Find("Environment");
        
            if (_parent == null)
                _parent = new GameObject("Environment");
        }
    }
}