using UnityEngine;
using Zenject;

using OS;
using UI.Alerts;
using UI.Icons;
using UI.Map;
using UI.Map.Icons;
using UI.Movement;

namespace Installers
{
    public class UIPrefabsInstaller : MonoInstaller
    {
        private GameObject _canvas;

        public MovementPanel MovementPanel;
        public IconsPanel IconsPanel;
        public MapPanel MapPanel;
        public MessagePanel MessagePanel;
        public TeleportPanel TeleportPanel;
    
        public override void InstallBindings()
        {
            ParentInitialize();
            MovementControllerInitialize();
        
            Container
                .BindInterfacesAndSelfTo<IconsPanel>()
                .FromComponentInNewPrefab(IconsPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<MapPanel>()
                .FromComponentInNewPrefab(MapPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<MessagePanel>()
                .FromComponentInNewPrefab(MessagePanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<TeleportPanel>()
                .FromComponentInNewPrefab(TeleportPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .BindFactory<Transform, int, Vector3, TeleportIcon, TeleportIconFactory>()
                .FromFactory<TeleportIconFactory>();
        }

        private void ParentInitialize()
        {
            _canvas = GameObject.Find("Canvas");

            if (_canvas == null)
            {
                _canvas = new GameObject("Canvas");
                _canvas.AddComponent<Canvas>();
            }
        }

        private void MovementControllerInitialize()
        {
            if (!OSHandler.IsWebMobile())
                return;
        
            Container
                .Bind<MovementPanel>()
                .FromComponentInNewPrefab(MovementPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        }
    }
}