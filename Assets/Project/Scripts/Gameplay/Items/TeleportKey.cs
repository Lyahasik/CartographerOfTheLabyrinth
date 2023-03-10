using UnityEngine;

using Gameplay.Player;
using Gameplay.Education;

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
                _educationHandler.ActivateLesson(LessonType.Lesson4);
                PlayerInventory.AddTeleportKey(LevelId);
                ProcessingProgress.PickItem(Type, transform.position.GetHashCode());
                ProcessingProgress.PickTeleportKey();
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
