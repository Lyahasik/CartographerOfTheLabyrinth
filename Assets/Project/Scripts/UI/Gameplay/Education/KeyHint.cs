using UnityEngine;

using OS;

namespace UI.Gameplay.Education
{
    public class KeyHint : MonoBehaviour
    {
        private void Start()
        {
            if (OSHandler.IsWebMobile())
                Destroy(gameObject);
        }
    }
}
