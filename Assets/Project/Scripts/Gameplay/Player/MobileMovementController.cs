using CartographerOfTheLabyrinth.OS;
using UnityEngine;

namespace CartographerOfTheLabyrinth.Gameplay.Player
{
    public class MobileMovementController : MonoBehaviour
    {
        private void Start()
        {
            if (!OSHandler.IsWebMobile())
                Destroy(gameObject);
        }
    }
}
