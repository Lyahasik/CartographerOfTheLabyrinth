using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

using Gameplay.Items;
using Gameplay.Player;
using Audio;
using UI.Gameplay;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class LockedDoor : Door
    {
        private GameplayPanel _gameplayPanel;
            
        [SerializeField] private GameObject _key;

        [Inject]
        public void Construct(GameplayPanel gameplayPanel)
        {
            _gameplayPanel = gameplayPanel;
        }
        
        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            _localeEntryKey = "LockedDoorMessage";
            UpdateLocale();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                if (_playerInventory.ContainsItem(ItemType.DoorKey))
                {
                    _key.SetActive(true);
                    _doorsHandler.OpenDoor(transform.position);
                    _animator.SetTrigger(_openingId);
                    AudioHandler.ActivateClip(_doorOpeningClipName);
                    _playerInventory.UseItem(ItemType.DoorKey);
                    
                    _gameplayPanel.TryOpenRateWindow();
                }
                else
                {
                    _messagePanel.TemporarilyActivateMessage(_warningMessage);
                }
            }
        }
    }
}
