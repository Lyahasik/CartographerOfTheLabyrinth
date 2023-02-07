using UnityEngine;

using Gameplay.Player;

namespace Gameplay.Items
{
    public class DoorKey : Item
    {
        private void Awake()
        {
            Type = ItemType.DoorKey;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                PlayerInventory.AddItem(ItemType.DoorKey);
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
