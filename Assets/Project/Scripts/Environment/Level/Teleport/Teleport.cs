using CartographerOfTheLabyrinth.Gameplay.Player;
using UnityEngine;
using Zenject;

namespace CartographerOfTheLabyrinth.Environment.Level.Teleport
{
    public class Teleport : MonoBehaviour
    {
        [Inject] private TeleportHandler _teleportHandler;

        private int _levelId;

        public int LevelId
        {
            set => _levelId = value;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                _teleportHandler.TeleportActivate(_levelId);
            }
        }
    }
}
