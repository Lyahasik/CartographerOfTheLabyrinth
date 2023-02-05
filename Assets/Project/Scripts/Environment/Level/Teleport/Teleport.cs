using UnityEngine;
using Zenject;

using Gameplay.Player;

namespace Environment.Level.Teleport
{
    public class Teleport : MonoBehaviour
    {
        private TeleportHandler _teleportHandler;
        private PlayerInventory _playerInventory;

        private int _levelId;

        [Inject]
        public void Construct(TeleportHandler teleportHandler, PlayerInventory playerInventory)
        {
            _teleportHandler = teleportHandler;
            _playerInventory = playerInventory;
        }

        public int LevelId
        {
            set => _levelId = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                if (_playerInventory.TryFindKey(_levelId))
                    _teleportHandler.TeleportActivate(_levelId);
            }
        }
    }
}
