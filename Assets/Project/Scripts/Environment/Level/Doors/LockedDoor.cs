using UnityEngine;

using Gameplay.Items;
using Gameplay.Player;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LockedDoor : Door
    {
        //TODO локализовать
        private const string _warningMessage = "Отсутствует ключ";

        [SerializeField] private GameObject _key;

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
