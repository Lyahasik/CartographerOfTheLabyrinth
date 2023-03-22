using UnityEngine;

using OS;

namespace UI
{
    public class MouseHandler : MonoBehaviour
    {
        public void ActivateCursor()
        {
            if (OSHandler.IsWebMobile())
                return;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void DeactivateCursor()
        {
            if (OSHandler.IsWebMobile())
                return;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
