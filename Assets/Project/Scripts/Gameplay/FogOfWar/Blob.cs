using UnityEngine;

namespace Gameplay.FogOfWar
{
    public class Blob : MonoBehaviour
    {
        private Vector3 _baseScale;
        [SerializeField] private Vector3 _upScale;

        private void Awake()
        {
            _baseScale = transform.localScale;
        }

        public void UpScale()
        {
            transform.localScale = _upScale;
        }
    
        public void DownScale()
        {
            transform.localScale = _baseScale;
        }
    }
}
