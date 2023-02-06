using UnityEngine;

using OS;

namespace Gameplay.Player
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
