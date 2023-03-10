using UnityEngine;
using Zenject;

using OS;
using UI.Alerts;
using UI.Gameplay;
using UI.Gameplay.Education;
using UI.Gameplay.Movement;
using UI.Map;
using UI.Map.Icons;
using UI.Settings;
using UI.Shop;

namespace Installers
{
    public class UIPrefabsInstaller : MonoInstaller
    {
        private GameObject _canvas;

        public MovementPanel MovementPanel;
        public GameplayPanel GameplayPanel;
        public EducationPanel EducationPanel;
        public SettingsPanel SettingsPanel;
        public MapPanel MapPanel;
        public ShopPanel ShopPanel;
        public MessagePanel MessagePanel;
        public TeleportPanel TeleportPanel;
    
        public override void InstallBindings()
        {
            ParentInitialize();
            MovementControllerInitialize();
        
            Container
                .Bind<TeleportPanel>()
                .FromComponentInNewPrefab(TeleportPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .BindInterfacesAndSelfTo<GameplayPanel>()
                .FromComponentInNewPrefab(GameplayPanel)
                .UnderTransform(_canvas.transform)
                .AsSingle()
                .NonLazy();
        
            Container
                .Bind<SettingsPanel>()
                .FromComponentInNewPrefab(SettingsPanel)
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
                .Bind<ShopPanel>()
                .FromComponentInNewPrefab(ShopPanel)
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
                .BindInterfacesAndSelfTo<EducationPanel>()
                .FromComponentInNewPrefab(EducationPanel)
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