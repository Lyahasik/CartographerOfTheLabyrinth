using UnityEngine;

using Gameplay.Player;

namespace Gameplay.Items
{
    public class TeleportKey : Item
    {
        private void Awake()
        {
            Type = ItemType.TeleportKey;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                PlayerInventory.AddTeleportKey(LevelId);
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
