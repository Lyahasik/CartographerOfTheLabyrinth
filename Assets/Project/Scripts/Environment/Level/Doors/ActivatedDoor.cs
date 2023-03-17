using System.Collections.Generic;
using UnityEngine;

using Gameplay.Player;
using Helpers;
using Audio;

namespace Environment.Level.Doors
{
    public class ActivatedDoor : Door
    {
        [SerializeField] private GameObject _topDoorSymbol;
        [SerializeField] private GameObject _rightDoorSymbol;
        [SerializeField] private GameObject _bottomDoorSymbol;
        [SerializeField] private GameObject _leftDoorSymbol;
        
        private DoorDirectionType _directionType;

        private void Start()
        {
            _localeEntryKey = "ActivatedDoorMessage";
            UpdateLocale();
            
            _directionType = DoorHelper.DetermineDirection(transform.forward);
            ActivateProgress();
        }

        private void ActivateProgress()
        {
            List<DoorDirectionType> directions = _doorsHandler.GetActiveDirections();

            foreach (DoorDirectionType direction in directions)
            {
                ActivationSymbol(direction);
            }
        }

        private void ActivationSymbol(DoorDirectionType direction)
        {
            switch (direction)
            {
                case DoorDirectionType.Top:
                    _topDoorSymbol.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case DoorDirectionType.Right:
                    _rightDoorSymbol.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case DoorDirectionType.Bottom:
                    _bottomDoorSymbol.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
                case DoorDirectionType.Left:
                    _leftDoorSymbol.GetComponent<MeshRenderer>().material.color = Color.white;
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<PlayerMovement>())
            {
                bool activation = _doorsHandler.MarkOpenDoor(transform.position, (int) _directionType);
                
                if (activation)
                    ActivationSymbol(_directionType);

                if (_doorsHandler.AllowActivateOpen())
                {
                    _animator.SetTrigger(_openingId);
                    AudioHandler.ActivateClip(_doorOpeningClipName);
                }
                else
                    _messagePanel.TemporarilyActivateMessage(_warningMessage);
            }
        }
    }
}
