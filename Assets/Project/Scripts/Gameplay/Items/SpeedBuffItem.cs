using UnityEngine;

using Gameplay.Player;
using Gameplay.Education;

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
                _educationHandler.ActivateLesson(LessonType.Lesson3);
                PlayerInventory.AddItem(Type);
                ProcessingProgress.PickItem(Type, transform.position.GetHashCode());
                GameplayHandler.ClearItemLevel(this);
            }
        }
    }
}
