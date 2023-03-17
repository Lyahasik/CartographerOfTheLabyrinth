using UnityEngine;

using Gameplay.Player;
using Audio;

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
                PlayerInventory.AddItem(Type);
                AudioHandler.ActivateClip(_pickClipName);
                
                ProcessingProgress.PickItem(Type, transform.position.GetHashCode());
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
