using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace UI.Map.Icons
{
    public class ArrowPlayer : MonoBehaviour
    {
        private MapSettings _settings;
        private PlayerMovement _player;

        private RectTransform _rectTransform;
        private Vector2 _initialSize;

        [Inject]
        public void Consrtuct(MapSettings settings, PlayerMovement playerMovement)
        {
            _settings = settings;
            _player = playerMovement;
        }

        private void Awake()
        {
            _rectTransform = transform as RectTransform;
        }

        private void OnEnable()
        {
            _initialSize = _rectTransform.sizeDelta;
        
            Vector3 playerPosition = new Vector3(_player.transform.position.x, _player.transform.position.z, 0f);
            _rectTransform.localPosition = playerPosition * _settings.PixelsPerUnit;

            Vector3 playerRotationEuler = new Vector3(0f, 0f, -_player.transform.rotation.eulerAngles.y);
            _rectTransform.localRotation = Quaternion.Euler(playerRotationEuler);
        }

        private void OnDisable()
        {
            _rectTransform.sizeDelta = _initialSize;
        }

        public void Resize(float differenceMultiplier)
        {
            _rectTransform.sizeDelta *= differenceMultiplier;
            _rectTransform.localPosition *= differenceMultiplier;
        }
    }
}
