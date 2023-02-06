using UnityEngine;

using Gameplay.Player;

namespace Gameplay.Items
{
    public class VisibilityRangeBuffItem : Item
    {
        private void Awake()
        {
            Type = ItemType.VisibilityRangeBuff;
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
