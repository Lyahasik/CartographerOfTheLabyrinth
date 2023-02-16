using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Alerts
{
    public class MessagePanel : MonoBehaviour
    {
        private UISettings _settings;
    
        [SerializeField] private TMP_Text _text;

        private bool _isActive;

        [Inject]
        public void Construct(UISettings settings)
        {
            _settings = settings;
        }

        public void ActivateMessage(string text)
        {
            if (_isActive)
                return;
        
            gameObject.SetActive(true);
            _text.text = text;
            _isActive = true;
        }

        public void TemporarilyActivateMessage(string text)
        {
            if (_isActive)
                return;
        
            ActivateMessage(text);
            Invoke(nameof(DeactivateMessage), _settings.MessageTimeLife);
        }

        public void DeactivateMessage()
        {
            gameObject.SetActive(false);
            _isActive = false;
        }
    }
}
