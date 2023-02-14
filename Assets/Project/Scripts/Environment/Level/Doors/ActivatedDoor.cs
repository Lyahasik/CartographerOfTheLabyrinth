using System.Collections.Generic;
using Gameplay.Player;
using UnityEngine;

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
            DetermineDirection();
            ActivateProgress();
        }

        private void DetermineDirection()
        {
            if (transform.forward == Vector3.forward)
            {
                _directionType = DoorDirectionType.Top;
            }
            else if (transform.forward == Vector3.right)
            {
                _directionType = DoorDirectionType.Right;
            }
            else if (transform.forward == Vector3.back)
            {
                _directionType = DoorDirectionType.Bottom;
            }
            else if (transform.forward == Vector3.left)
            {
                _directionType = DoorDirectionType.Left;
            }
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
                    _topDoorSymbol.SetActive(true);
                    break;
                case DoorDirectionType.Right:
                    _rightDoorSymbol.SetActive(true);
                    break;
                case DoorDirectionType.Bottom:
                    _bottomDoorSymbol.SetActive(true);
                    break;
                case DoorDirectionType.Left:
                    _leftDoorSymbol.SetActive(true);
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
                    _animator.SetTrigger(_openingId);
            }
        }
    }
}
