using Gameplay.Items;
using Gameplay.Player;
using UnityEngine;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LockedDoor : Door
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                if (_playerInventory.ContainsItem(ItemType.DoorKey))
                {
                    _doorsHandler.OpenLockedDoor(transform.position);
                    _animator.SetTrigger(_openingId);
                    _playerInventory.UseItem(ItemType.DoorKey);
                }
            }
        }
    }
}
