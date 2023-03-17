using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

using Gameplay.Player;
using FiniteStateMachine;
using Gameplay.Education;
using Gameplay.Progress;
using Audio;

namespace Environment.Level.Teleport
{
    public class TeleportHandler
    {
        private const string _teleportClipName = "Teleport";
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private EducationHandler _educationHandler;
        private ProcessingProgress _processingProgress;
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
            EducationHandler educationHandler,
            ProcessingProgress processingProgress,
            PlayerMovement playerMovement,
            PlayerWatcher playerWatcher)
        {
            _container = container;
            _gameMashine = gameMashine;
            _educationHandler = educationHandler;
            _processingProgress = processingProgress;
            _playerMovement = playerMovement;
            _playerWatcher = playerWatcher;
        }

        public void CollectTeleports(List<TeleportData> teleportsData)
        {
            _teleportsData = new TeleportData[teleportsData.Count];

            foreach (TeleportData teleportData in teleportsData)
            {
                _teleportsData[teleportData.LevelId] = teleportData;

                if (_processingProgress.ActivateTeleports.Contains(teleportData.LevelId))
                    _teleportsData[teleportData.LevelId].IsActive = true;
            }
        }

        public void TeleportActivate(int levelId)
        {
            _educationHandler.ActivateLesson(LessonType.Lesson5);
            _teleportsData[levelId].IsActive = true;
            AudioHandler.ActivateClip(_teleportClipName);

            _processingProgress.ActivateTeleport(levelId);
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
            AudioHandler.ActivateClip(_teleportClipName);
        }
    }
}
