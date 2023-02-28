using TMPro;
using UnityEngine;
using Zenject;

using Environment.Level.Teleport;
using FiniteStateMachine;
using Gameplay;

namespace UI.Alerts
{
    public class TeleportPanel : MonoBehaviour
    {
        //TODO локализовать
        private const string _keyMissingMessage = "Отсутствует активатор зоны телепорта"; 
        private const string _paidTeleportMessage = "Вы далеко от телепорта. Активировать карманный телепорт?"; 
        private const string _advertisingButtonMessage = "Смотреть"; 
        private const string _teleportButtonMessage = "Телепортироваться"; 
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private GameplayHandler _gameplayHandler;
        private TeleportHandler _teleportHandler;
    
        [SerializeField] private GameObject _activationWindow;
        [SerializeField] private TMP_Text _teleportActivationText;
        [SerializeField] private TMP_Text _advertisingButtonText;
        
        [SerializeField] private GameObject _startTeleportWindow;
        [SerializeField] private TMP_Text _startTeleportButtonText;
        
        [SerializeField] private GameObject _paidTeleportWindow;
        [SerializeField] private TMP_Text _paidTeleportText;
        [SerializeField] private TMP_Text _paidTeleportButtonText;

        private int _levelId;

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
            _teleportActivationText.text = _keyMissingMessage;
            _advertisingButtonText.text = _advertisingButtonMessage;
            
            _startTeleportButtonText.text = _teleportButtonMessage;
            
            _paidTeleportText.text = _paidTeleportMessage;
            _paidTeleportButtonText.text = _advertisingButtonMessage;
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
