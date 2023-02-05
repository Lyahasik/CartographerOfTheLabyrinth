using UnityEngine;

namespace UI.Map
{
    public class MapPanel : MonoBehaviour
    {
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
