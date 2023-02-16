using UnityEngine;
using Zenject;

using Gameplay.Player;
using UI.Alerts;

namespace Environment.Level.Teleport
{
    public class Teleport : MonoBehaviour
    {
        private TeleportHandler _teleportHandler;
        private PlayerInventory _playerInventory;
        private TeleportPanel _teleportPanel;

        [SerializeField] private MeshRenderer _mesh;

        private int _levelId;

        [Inject]
        public void Construct(TeleportHandler teleportHandler, PlayerInventory playerInventory, TeleportPanel teleportPanel)
        {
            _teleportHandler = teleportHandler;
            _playerInventory = playerInventory;
            _teleportPanel = teleportPanel;
        }

        private void OnEnable()
        {
            _teleportHandler.OnActivate += Activate;
        }

        private void OnDisable()
        {
            _teleportHandler.OnActivate -= Activate;
        }

        public int LevelId
        {
            set => _levelId = value;
        }

        private void Activate(int levelId)
        {
            if (_levelId != levelId)
                return;
            
            _mesh.material.color = Color.green;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_teleportHandler.TeleportIsActive(_levelId)
                || other.GetComponent<PlayerMovement>() == null)
                return;
            
            if (_playerInventory.TryFindKey(_levelId))
            {
                _teleportHandler.TeleportActivate(_levelId);
            }
            else
                _teleportPanel.ActivateActivationWindow(_levelId);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_teleportHandler.TeleportIsActive(_levelId)
                || other.GetComponent<PlayerMovement>() == null)
                return;
            
            _teleportPanel.DeactivateActivationWindow();
        }
    }
}
