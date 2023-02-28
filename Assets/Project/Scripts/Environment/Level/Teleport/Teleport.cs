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

        public int LevelId
        {
            set
            {
                _levelId = value;
                
                if (_teleportHandler.TeleportIsActive(_levelId))
                    _mesh.material.color = Color.green;
            }
        }

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

        public void UpdateActivate()
        {
            if (_teleportHandler.TeleportIsActive(_levelId))
                _mesh.material.color = Color.green;
        }

        private void Activate(int levelId)
        {
            if (_levelId != levelId)
                return;
            
            _mesh.material.color = Color.green;
        }

        public void Reset()
        {
            _mesh.material.color = Color.red;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() == null)
                return;

            if (_teleportHandler.TeleportIsActive(_levelId))
            {
                _teleportHandler.IsFreeTeleport = true;
                _teleportPanel.ActivateStartTeleportWindow();
            }
            else
            {
                if (_playerInventory.TryFindKey(_levelId))
                {
                    _teleportHandler.TeleportActivate(_levelId);
                    _teleportPanel.ActivateStartTeleportWindow();
                }
                else
                    _teleportPanel.ActivateActivationWindow(_levelId);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<PlayerMovement>() == null)
                return;

            if (_teleportHandler.TeleportIsActive(_levelId))
            {
                _teleportPanel.DeactivateStartTeleportWindow();
                _teleportHandler.IsFreeTeleport = false;
            }
            else
                _teleportPanel.DeactivateActivationWindow();
        }
    }
}
