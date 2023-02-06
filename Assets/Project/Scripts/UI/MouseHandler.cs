using UnityEngine;

using OS;

namespace UI
{
    public class MouseHandler : MonoBehaviour
    {
        void Start()
        {
            Cursor.visible = false;
        
            if (!OSHandler.IsWebMobile())
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
