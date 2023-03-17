using UnityEngine;

using Gameplay.Items;
using Gameplay.Player;
using Audio;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LockedDoor : Door
    {
        [SerializeField] private GameObject _key;
        
        private void Start()
        {
            _localeEntryKey = "LockedDoorMessage";
            UpdateLocale();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                if (_playerInventory.ContainsItem(ItemType.DoorKey))
                {
                    _key.SetActive(true);
                    _doorsHandler.OpenDoor(transform.position);
                    _animator.SetTrigger(_openingId);
                    AudioHandler.ActivateClip(_doorOpeningClipName);
                    _playerInventory.UseItem(ItemType.DoorKey);
                }
                else
                {
                    _messagePanel.TemporarilyActivateMessage(_warningMessage);
                }
            }
        }
    }
}
