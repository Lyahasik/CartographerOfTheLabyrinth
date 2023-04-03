using UnityEngine;

using OS;

namespace UI
{
    public class MouseHandler : MonoBehaviour
    {
        private bool _isActive;

        public bool IsActive => _isActive;
        
        public void ActivateCursor()
        {
            if (OSHandler.IsWebMobile())
                return;

            _isActive = true;
            
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        public void DeactivateCursor()
        {
            if (OSHandler.IsWebMobile())
                return;

            _isActive = false;
            
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
