using Gameplay.Player;
using UnityEngine;

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
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
