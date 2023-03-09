using UnityEngine;
using Zenject;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

using Gameplay.Player;
using UI.Alerts;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        private LocalizedString _localizedString;
        
        protected readonly int _openingId = Animator.StringToHash("Opening");

        protected DoorsHandler _doorsHandler;
        protected PlayerInventory _playerInventory;
        protected MessagePanel _messagePanel;
    
        protected Animator _animator;
    
        protected Level _level;

        protected string _localeEntryKey;
        protected string _warningMessage;

        [Inject]
        public void Construct(DoorsHandler doorsHandler, PlayerInventory playerInventory, MessagePanel messagePanel)
        {
            _doorsHandler = doorsHandler;
            _playerInventory = playerInventory;
            _messagePanel = messagePanel;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            LocaleHadler.OnChange += UpdateLocale;
        }

        private void OnDisable()
        {
            LocaleHadler.OnChange -= UpdateLocale;
        }

        protected void UpdateLocale()
        {
            _warningMessage = LocalizationSettings
                .StringDatabase
                .GetLocalizedString("StringTable", _localeEntryKey);
        }

        public void Init(Level level)
        {
            _level = level;
        }

        private void DestroyYourself()
        {
            _level.DestroyObject(transform.parent.gameObject, true);
        }
    }
}