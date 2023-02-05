using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay.Player;

namespace Environment.Level.Teleport
{
    public class TeleportHandler : ITickable
    {
        private PlayerWatcher _playerWatcher;
        private PlayerMovement _playerMovement;
    
        private TeleportData[] _teleportsData;

        public TeleportData[] TeleportsData => _teleportsData;

        [Inject]
        public void Construct(PlayerMovement playerMovement, PlayerWatcher playerWatcher)
        {
            _playerMovement = playerMovement;
            _playerWatcher = playerWatcher;
        }

        public void Tick()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Teleport(0);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Teleport(1);
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                Teleport(2);
            }
        }

        public void CollectTeleports(List<TeleportData> teleportsData)
        {
            _teleportsData = new TeleportData[teleportsData.Count];

            foreach (TeleportData teleportData in teleportsData)
            {
                _teleportsData[teleportData.LevelId] = teleportData;
            }
        }

        public void TeleportActivate(int levelId)
        {
            _teleportsData[levelId].IsActive = true;
        }

        public bool TeleportIsActive(int levelId)
        {
            return _teleportsData[levelId].IsActive;
        }

        public void Teleport(int levelId)
        {
            if (!_teleportsData[levelId].IsActive)
                return;
        
            Vector3 teleportPoint = new Vector3(_teleportsData[levelId].Position.x, 0f, _teleportsData[levelId].Position.y);
        
            _playerMovement.SetPosition(teleportPoint);
            _playerWatcher.ResetPosition();
        }
    }
}
