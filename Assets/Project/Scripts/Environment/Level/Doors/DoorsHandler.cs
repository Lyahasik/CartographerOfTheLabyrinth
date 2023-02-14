using System.Collections.Generic;
using Gameplay.Progress;
using UnityEngine;
using Zenject;

namespace Environment.Level.Doors
{
    public class DoorsHandler
    {
        private ProcessingProgress _processingProgress;

        private LockedDoorData[] _lockedDoors;
        private ActivateDoorData[] _activateDoors;

        private bool _isActivateDoorsOpen;

        [Inject]
        public void Construct(ProcessingProgress processingProgress)
        {
            _processingProgress = processingProgress;
        }

        public bool IsLockedDoorNeedPut(Vector3 position)
        {
            if (_lockedDoors == null)
                LoadLockedDoors();

            int hash = position.GetHashCode();

            foreach (LockedDoorData lockedDoorData in _lockedDoors)
            {
                if (lockedDoorData.Hash == hash)
                    return lockedDoorData.IsOpened != 1;
            }

            WriteLockedDoor(hash);
            
            return true;
        }

        public bool IsActivateDoorNeedPut()
        {
            if (_activateDoors == null)
                LoadActivateDoors();
            
            return !_isActivateDoorsOpen;
        }

        public bool MarkOpenDoor(Vector3 position, int direction)
        {
            int hash = position.GetHashCode();

            foreach (ActivateDoorData doorData in _activateDoors)
            {
                if (hash == doorData.Hash)
                    return false;
            }

            WriteActivateDoor(hash, direction);
            return true;
        }

        public void OpenLockedDoor(Vector3 position)
        {
            int hash = position.GetHashCode();

            for (int i = 0; i < _lockedDoors.Length; i++)
            {
                if (hash == _lockedDoors[i].Hash)
                    _lockedDoors[i].IsOpened = 1;
            }
            
            _processingProgress.LockedDoors = _lockedDoors;
        }

        private void WriteActivateDoor(int hash, int direction)
        {
            for (int i = 0; i < _activateDoors.Length; i++)
            {
                if (_activateDoors[i].Hash != 0)
                    continue;

                _activateDoors[i].Hash = hash;
                _activateDoors[i].Direction = direction;
                _activateDoors[i].IsOpened = 1;

                break;
            }

            UpdateActivateDoorOpen();

            _processingProgress.ActivateDoors = _activateDoors;
        }

        private void WriteLockedDoor(int hash)
        {
            for (int i = 0; i < _lockedDoors.Length; i++)
            {
                if (_lockedDoors[i].Hash != 0)
                    continue;

                _lockedDoors[i].Hash = hash;
                _lockedDoors[i].IsOpened = 0;

                break;
            }

            _processingProgress.LockedDoors = _lockedDoors;
        }

        public bool AllowActivateOpen()
        {
            return _isActivateDoorsOpen;
        }

        private void LoadLockedDoors()
        {
            _lockedDoors = _processingProgress.LockedDoors;
        }

        private void LoadActivateDoors()
        {
            _activateDoors = _processingProgress.ActivateDoors;

            UpdateActivateDoorOpen();
        }

        private void UpdateActivateDoorOpen()
        {
            foreach (ActivateDoorData doorData in _activateDoors)
            {
                if (doorData.IsOpened == 0)
                {
                    _isActivateDoorsOpen = false;
                    return;
                }
            }

            _isActivateDoorsOpen = true;
        }

        public List<DoorDirectionType> GetActiveDirections()
        {
            List<DoorDirectionType> directions = new List<DoorDirectionType>();
            
            foreach (ActivateDoorData doorData in _activateDoors)
            {
                if (doorData.IsOpened == 1)
                    directions.Add((DoorDirectionType) doorData.Direction);
            }

            return directions;
        }
    }
}
