using UnityEngine;
using Zenject;

using Environment.Level.Teleport;
using FiniteStateMachine;
using Gameplay;

namespace UI.Alerts
{
    public class TeleportPanel : MonoBehaviour
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private GameplayHandler _gameplayHandler;
        private TeleportHandler _teleportHandler;
    
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
            TeleportHandler teleportHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _gameplayHandler = gameplayHandler;
            _teleportHandler = teleportHandler;
        }

        private void Start()
        {
            transform.SetSiblingIndex(1);
        }

        public void ActivateActivationWindow(int levelId)
        {
            _activationWindow.SetActive(true);
            _levelId = levelId;
        }
    
        public void DeactivateActivationWindow()
        {
            _activationWindow.SetActive(false);
        }

        public void ViewingAdsActivate()
        {
            Debug.Log("View ads");
            _teleportHandler.TeleportActivate(_levelId);
            _gameplayHandler.ClearTeleportKeyLevel(_levelId);
            DeactivateActivationWindow();
            ActivateStartTeleportWindow();
        }
        
        public void ActivateStartTeleportWindow()
        {
            _startTeleportWindow.SetActive(true);
        }
        
        public void DeactivateStartTeleportWindow()
        {
            _startTeleportWindow.SetActive(false);
        }
        
        public void ActivatePaidTeleportWindow()
        {
            _paidTeleportWindow.SetActive(true);
        }
        
        public void DeactivatePaidTeleportWindow()
        {
            _paidTeleportWindow.SetActive(false);
        }

        public void OpenMap()
        {
            DeactivateStartTeleportWindow();
            _gameMashine.Enter(_container.Instantiate<MapState>());
        }

        public void ViewingAdsTeleport()
        {
            Debug.Log("View ads");
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
