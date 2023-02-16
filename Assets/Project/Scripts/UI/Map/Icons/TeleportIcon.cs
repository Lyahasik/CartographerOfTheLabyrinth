using UnityEngine;
using UnityEngine.UI;
using Zenject;

using Environment.Level.Teleport;
using UI.Alerts;
using UnityEngine.EventSystems;

namespace UI.Map.Icons
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class TeleportIcon : MonoBehaviour, IPointerClickHandler
    {
        private UISettings _settings;
        private TeleportHandler _teleportHandler;
        private TeleportPanel _teleportPanel;
    
        private RectTransform _rectTransform;
        private Image _image;
        private Vector2 _initialSize;
        private Vector3 _initialPosition;
    
        private int _levelId;
    
        [Inject]
        public void Consrtuct(UISettings settings,
            TeleportHandler teleportHandler,
            TeleportPanel teleportPanel,
            int levelId,
            Vector2 position)
        {
            _settings = settings;
            _teleportHandler = teleportHandler;
            _teleportPanel = teleportPanel;
            
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

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!_teleportHandler.TeleportIsActive(_levelId))
                return;
            
            if (_teleportHandler.IsFreeTeleport)
            {
                _teleportHandler.Teleport(_levelId);
            }
            else
            {
                _teleportPanel.ActivatePaidTeleportWindow();
            }
        }
    }
}
