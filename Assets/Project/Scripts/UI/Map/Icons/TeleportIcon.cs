using UnityEngine;
using UnityEngine.UI;
using Zenject;

using Environment.Level.Teleport;

namespace UI.Map.Icons
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class TeleportIcon : MonoBehaviour
    {
        private UISettings _settings;
        private TeleportHandler _teleportHandler;
    
        private RectTransform _rectTransform;
        private Image _image;
        private Vector2 _initialSize;
        private Vector3 _initialPosition;
    
        private int _levelId;
    
        [Inject]
        public void Consrtuct(UISettings settings, TeleportHandler teleportHandler, int levelId, Vector2 position)
        {
            _settings = settings;
            _teleportHandler = teleportHandler;
            _levelId = levelId;
            _initialPosition = position;
        }
    
        private void Awake()
        {
            _rectTransform = transform as RectTransform;
            _image = GetComponent<Image>();
        }
    
        private void OnEnable()
        {
            _initialSize = _rectTransform.sizeDelta;
        
            _rectTransform.localPosition = _initialPosition * _settings.PixelsPerUnit;
            _image.color = _teleportHandler.TeleportIsActive(_levelId) ? Color.blue : Color.gray;
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
