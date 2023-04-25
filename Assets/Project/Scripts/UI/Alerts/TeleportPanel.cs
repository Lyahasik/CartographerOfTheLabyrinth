using UnityEngine;
using Zenject;

using Environment.Level.Teleport;
using FiniteStateMachine;
using Gameplay;
using Publish;

namespace UI.Alerts
{
    public class TeleportPanel : MonoBehaviour
    {
        private const int _indexAdsTeleportActivate = 2;
        private const int _indexAdsTeleport = 3;
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private GameplayHandler _gameplayHandler;
        private TeleportHandler _teleportHandler;
        private MouseHandler _mouseHandler;
        private PublishHandler _publishHandler;
    
        [SerializeField] private GameObject _activationWindow;
        
        [SerializeField] private GameObject _startTeleportWindow;
        
        [SerializeField] private GameObject _paidTeleportWindow;

        private int _levelId;

        public int LevelId
        {
            set => _levelId = value;
        }

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            GameplayHandler gameplayHandler,
            TeleportHandler teleportHandler,
            MouseHandler mouseHandler,
            PublishHandler publishHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _gameplayHandler = gameplayHandler;
            _teleportHandler = teleportHandler;
            _mouseHandler = mouseHandler;
            _publishHandler = publishHandler;
        }

        private void Awake()
        {
            transform.SetSiblingIndex(2);
        }

        private void OnEnable()
        {
            PublishHandler.OnActivateAward += GetAwardActivateTeleport;
            PublishHandler.OnActivateAward += GetAwardTeleport;
        }

        private void OnDisable()
        {
            PublishHandler.OnActivateAward -= GetAwardActivateTeleport;
            PublishHandler.OnActivateAward -= GetAwardTeleport;
        }

        public void ActivateActivationWindow(int levelId)
        {
            _mouseHandler.ActivateCursor();
            _activationWindow.SetActive(true);
            _levelId = levelId;
        }
    
        public void DeactivateActivationWindow()
        {
            _activationWindow.SetActive(false);
            _mouseHandler.DeactivateCursor();
        }

        public void ViewingAdsActivate()
        {
            _publishHandler.ViewVideoAds(_indexAdsTeleportActivate);
        }

        public void GetAwardActivateTeleport(int indexAward)
        {
            if (indexAward != _indexAdsTeleportActivate)
                return;
            
            _teleportHandler.TeleportActivate(_levelId);
            _gameplayHandler.ClearTeleportKeyLevel(_levelId);
            DeactivateActivationWindow();
            ActivateStartTeleportWindow();
        }
        
        public void ActivateStartTeleportWindow()
        {
            _mouseHandler.ActivateCursor();
            _startTeleportWindow.SetActive(true);
        }
        
        public void DeactivateStartTeleportWindow()
        {
            _startTeleportWindow.SetActive(false);
            _mouseHandler.DeactivateCursor();
        }
        
        public void ActivatePaidTeleportWindow()
        {
            _mouseHandler.ActivateCursor();
            _paidTeleportWindow.SetActive(true);
        }
        
        public void DeactivatePaidTeleportWindow()
        {
            _paidTeleportWindow.SetActive(false);
            _mouseHandler.DeactivateCursor();
        }

        public void OpenMap()
        {
            DeactivateStartTeleportWindow();
            _gameMashine.Enter(_container.Instantiate<MapState>());
        }

        public void ViewingAdsTeleport()
        {
            _publishHandler.ViewVideoAds(_indexAdsTeleport);
        }

        public void GetAwardTeleport(int indexAward)
        {
            if (indexAward != _indexAdsTeleport)
                return;
            
            _teleportHandler.Teleport(_levelId);
            DeactivatePaidTeleportWindow();
        }

        public void DeactivateAllWindows()
        {
            DeactivateActivationWindow();
            DeactivateStartTeleportWindow();
            DeactivatePaidTeleportWindow();
        }
    }
}
