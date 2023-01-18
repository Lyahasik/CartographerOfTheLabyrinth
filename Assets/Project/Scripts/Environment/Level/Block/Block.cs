using UnityEngine;

namespace CartographerOfTheLabyrinth.Environment.Level.Block
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _type;

        public EnvironmentObjectType Type
        {
            get => _type;
        }
    }
}
