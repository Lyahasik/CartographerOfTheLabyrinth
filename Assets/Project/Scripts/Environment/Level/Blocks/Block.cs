using UnityEngine;

namespace Environment.Level.Blocks
{
    public class Block : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _type;

        public EnvironmentObjectType Type => _type;
    }
}
