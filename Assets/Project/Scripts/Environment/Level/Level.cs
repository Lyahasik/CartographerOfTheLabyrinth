using UnityEngine;

namespace CartographerOfTheLabyrinth.Environment.Level
{
    public class Level : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
