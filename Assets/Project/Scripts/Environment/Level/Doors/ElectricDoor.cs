using UnityEngine;

using Gameplay.Player;
using Helpers;
using Audio;

namespace Environment.Level.Doors
{
    public class ElectricDoor : Door
    {
        [SerializeField] private PowerSymbol[] _symbolsPower;
        
        private DoorDirectionType _directionType;
        private int _numberActivePower;

        private void Start()
        {
            _localeEntryKey = "ElectricDoorMessage";
            UpdateLocale();
            
            _directionType = DoorHelper.DetermineDirection(transform.forward);
            ActivateProgress();
        }

        private void ActivateProgress()
        {
            _numberActivePower = _doorsHandler.GetActivePowerInDirection(_directionType);
            
            ActivationSymbols();
        }

        private void ActivationSymbols()
        {
            for (int i = 0; i < _symbolsPower.Length; i++)
            {
                if (i >= _numberActivePower)
                    return;
                
                _symbolsPower[i].Activate();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                if (_numberActivePower == _symbolsPower.Length)
                {
                    _doorsHandler.OpenDoor(transform.position);
                    _animator.SetTrigger(_openingId);
                    AudioHandler.ActivateClip(_doorOpeningClipName);
                }
                else
                {
                    _messagePanel.TemporarilyActivateMessage(_warningMessage);
                }
            }
        }
    }
}
