using UnityEngine;

namespace CartographerOfTheLabyrinth.UI.Map
{
    public class MapPanel : MonoBehaviour
    {
        // private void Start()
        // {
        //     //TODO FSM
        //     gameObject.SetActive(false);
        // }

        public void SwitchActive()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }
    }
}
