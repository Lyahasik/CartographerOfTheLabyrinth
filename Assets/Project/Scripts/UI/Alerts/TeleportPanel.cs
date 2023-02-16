using TMPro;
using UnityEngine;
using Zenject;

using Environment.Level.Teleport;

namespace UI.Alerts
{
    public class TeleportPanel : MonoBehaviour
    {
        //TODO локализовать
        private const string _keyMissingMessage = "Отсутствует активатор зоны телепорта"; 
        private const string _advertisingButtonMessage = "Смотреть"; 
    
        [SerializeField] private GameObject _activationWindow;
        [SerializeField] private TMP_Text _teleportActivationText;
        [SerializeField] private TMP_Text _advertisingButtonText;

        private bool _isActiveActivationWindow;
        private int _levelId;

        private TeleportHandler _teleportHandler;

        [Inject]
        public void Construct(TeleportHandler teleportHandler)
        {
            _teleportHandler = teleportHandler;
        }

        private void Start()
        {
            _teleportActivationText.text = _keyMissingMessage;
            _advertisingButtonText.text = _advertisingButtonMessage;
        }

        public void ActivateActivationWindow(int levelId)
        {
            _activationWindow.SetActive(true);
            _isActiveActivationWindow = true;
            _levelId = levelId;
        }
    
        public void DeactivateActivationWindow()
        {
            if (!_isActiveActivationWindow)
                return;
        
            _activationWindow.SetActive(false);
            _isActiveActivationWindow = false;
        }

        public void ViewingAds()
        {
            Debug.Log("View ads");
            _teleportHandler.TeleportActivate(_levelId);
            DeactivateActivationWindow();
        }
    }
}
