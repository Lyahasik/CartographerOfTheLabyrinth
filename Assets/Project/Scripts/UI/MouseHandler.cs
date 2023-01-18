using CartographerOfTheLabyrinth.OS;
using UnityEngine;

namespace CartographerOfTheLabyrinth.UI
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
