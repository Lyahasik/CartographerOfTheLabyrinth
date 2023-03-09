using UnityEngine;

using Gameplay.Items;
using Gameplay.Player;

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
