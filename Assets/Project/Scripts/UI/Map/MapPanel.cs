using UnityEngine;

namespace CartographerOfTheLabyrinth.UI.Map
{
    public class MapPanel : MonoBehaviour
    {
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
