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
                ProcessingProgress.PickItem(Type, transform.position.GetHashCode());
                ProcessingProgress.PickTeleportKey();
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
