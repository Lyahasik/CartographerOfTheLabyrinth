using UnityEngine;

namespace Environment.Level.Blocks
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Block : MonoBehaviour
    {
        [SerializeField] private EnvironmentObjectType _type;

        private MeshRenderer _meshRenderer;

        public EnvironmentObjectType Type => _type;

        public void UpdateMaterials(Material side, Material top)
        {
            Material[] newMaterials = { side, top };

            _meshRenderer.materials = newMaterials;
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }
    }
}
