using UnityEngine;

namespace Environment.Level.Blocks
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Block : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _type;

        private MeshRenderer _meshRenderer;

        public EnvironmentObjectType Type => _type;

        public Material Material
        {
            set => _meshRenderer.material = value;
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
