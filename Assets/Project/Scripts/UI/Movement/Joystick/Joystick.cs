using UnityEngine;
using UnityEngine.Events;

namespace CartographerOfTheLabyrinth.UI.Movement.Joystick
{
    public class Joystick : MonoBehaviour
    {
        [SerializeField] private Stick _stick;

        private float _range;
    
        public float Range => _range;
    
        public UnityEvent<Vector2> OnDrag;

        private void Awake()
        {
            _range = GetComponent<RectTransform>().rect.width * 0.5f;
        }

        private void Update()
        {
            TryProcessingDrag();
        }

        private void TryProcessingDrag()
        {
            if (!_stick.IsDrag)
                return;

            Vector2 step = _stick.GetStep();
            Vector2 stepNormalized = new Vector2(step.x / _range, step.y / _range);
        
            OnDrag?.Invoke(stepNormalized);
        }
    }
}