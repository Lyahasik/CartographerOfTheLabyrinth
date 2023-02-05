using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Movement.Joystick
{
    public class Stick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        private RectTransform _rectTransform;

        private float _range;
        private Vector2 _beginPosition;
        private Vector2 _downShiftPosition;
        private Vector2 _dragShiftPosition;

        private bool _isDrag;

        public bool IsDrag => _isDrag;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _beginPosition = _rectTransform.position;
        }

        private void Start()
        {
            _range = transform.parent.GetComponent<Joystick>().Range;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _downShiftPosition = (Vector2) _rectTransform.position - eventData.position;
        
            _isDrag = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _dragShiftPosition = eventData.position - _beginPosition + _downShiftPosition;
            _dragShiftPosition = Vector3.ClampMagnitude(_dragShiftPosition, _range);

            _rectTransform.position = _beginPosition + _dragShiftPosition;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _rectTransform.position = _beginPosition;
            _dragShiftPosition = Vector2.zero;

            _isDrag = false;
        }

        public Vector2 GetStep()
        {
            return _dragShiftPosition;
        }
    }
}
