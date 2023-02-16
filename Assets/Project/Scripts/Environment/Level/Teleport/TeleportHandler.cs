using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay.Player;
using FiniteStateMachine;

namespace Environment.Level.Teleport
{
    public class TeleportHandler
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private PlayerWatcher _playerWatcher;
        private PlayerMovement _playerMovement;
    
        private TeleportData[] _teleportsData;
        private bool _isFreeTeleport;

        public TeleportData[] TeleportsData => _teleportsData;

        public bool IsFreeTeleport
        {
            get => _isFreeTeleport;
            set => _isFreeTeleport = value;
        }

        public event Action<int> OnActivate; 

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            PlayerMovement playerMovement,
            PlayerWatcher playerWatcher)
        {
            _container = container;
            _gameMashine = gameMashine;
            _playerMovement = playerMovement;
            _playerWatcher = playerWatcher;
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
            _isFreeTeleport = true;
            OnActivate?.Invoke(levelId);
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
            
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
    }
}
