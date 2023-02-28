using UnityEngine;

using Gameplay.Player;

namespace Gameplay.Items
{
    public class SpeedBuffItem : Item
    {
        private void Awake()
        {
            Type = ItemType.SpeedBuff;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                PlayerInventory.AddItem(Type);
                ProcessingProgress.PickItem(Type, transform.position.GetHashCode());
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
