using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay.Progress;

namespace Environment.Level.Doors
{
    public class DoorsHandler
    {
        private ProcessingProgress _processingProgress;

        private DoorData[] _doors;
        private ActivateDoorData[] _activateDoors;
        private PowerPointData[] _powerPoints;

        private bool _isActivateDoorsOpen;

        [Inject]
        public void Construct(ProcessingProgress processingProgress)
        {
            _processingProgress = processingProgress;
        }

        public bool IsDoorNeedPut(Vector3 position)
        {
            if (_doors == null)
                LoadDoors();

            int hash = position.GetHashCode();

            foreach (DoorData lockedDoorData in _doors)
            {
                if (lockedDoorData.Hash == hash)
                    return lockedDoorData.IsOpened != 1;
            }

            WriteDoor(hash);
            
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

        public void OpenDoor(Vector3 position)
        {
            int hash = position.GetHashCode();

            for (int i = 0; i < _doors.Length; i++)
            {
                if (hash == _doors[i].Hash)
                    _doors[i].IsOpened = 1;
            }
            
            _processingProgress.Doors = _doors;
        }

        private void WriteDoor(int hash)
        {
            for (int i = 0; i < _doors.Length; i++)
            {
                if (_doors[i].Hash != 0)
                    continue;

                _doors[i].Hash = hash;
                _doors[i].IsOpened = 0;

                break;
            }

            _processingProgress.Doors = _doors;
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

        public bool AllowActivateOpen()
        {
            return _isActivateDoorsOpen;
        }

        private void LoadDoors()
        {
            _doors = _processingProgress.Doors;
        }

        private void LoadActivateDoors()
        {
            _activateDoors = _processingProgress.ActivateDoors;

            UpdateActivateDoorOpen();
        }
        
        private void LoadPowerPoints()
        {
            _powerPoints = _processingProgress.PowerPoints;
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

        public void ActivatePower(Vector3 position, int direction)
        {
            if (_powerPoints == null)
            {
                LoadPowerPoints();
            }
            
            int hash = position.GetHashCode();
            
            for (int i = 0; i < _powerPoints.Length; i++)
            {
                if (_powerPoints[i].Hash == hash)
                    return;

                if (_powerPoints[i].Hash == 0)
                {
                    _powerPoints[i].Hash = hash;
                    _powerPoints[i].Direction = direction;
                    _powerPoints[i].IsActive = 1;
                    
                    _processingProgress.PowerPoints = _powerPoints;
                    
                    return;
                }
            }
        }

        public bool IsActivePower(Vector3 position)
        {
            if (_powerPoints == null)
            {
                LoadPowerPoints();
            }
            
            int hash = position.GetHashCode();

            foreach (PowerPointData powerPointData in _powerPoints)
            {
                if (powerPointData.Hash == hash)
                    return true;
            }
            
            return false;
        }

        public int GetActivePowerInDirection(DoorDirectionType directionType)
        {
            if (_powerPoints == null)
            {
                LoadPowerPoints();
            }

            int countActivePower = 0;

            foreach (PowerPointData powerPointData in _powerPoints)
            {
                if (powerPointData.Direction == (int) directionType
                    && powerPointData.IsActive == 1)
                {
                    countActivePower++;
                }
            }

            return countActivePower;
        }
    }
}
