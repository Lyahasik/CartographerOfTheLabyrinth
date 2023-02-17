using System.Collections;
using Cinemachine;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class PlayerWatcher : MonoBehaviour
    {
        private GameplaySettings _settings;
        private PlayerMovement _playerMovement;

        private CinemachineVirtualCamera _cinemachine;
        private CinemachineTransposer _cinemachineTransposer;
        
        private Vector3 _baseFollowOffset;
        private Vector3 _currentFollowOffset;
        private Vector3 _targetFollowOffset;
        private float _distanceBetweenOffsets;

        [Inject]
        public void Construct(GameplaySettings settings, PlayerMovement playerMovement)
        {
            _settings = settings;
            _playerMovement = playerMovement;
        }
    
        public void Start()
        {
            _cinemachine = GetComponent<CinemachineVirtualCamera>();
            _cinemachineTransposer = _cinemachine.GetCinemachineComponent<CinemachineTransposer>();

            _cinemachine.Follow = _playerMovement.transform;
            _cinemachine.LookAt = _playerMovement.transform;

            _baseFollowOffset = _cinemachineTransposer.m_FollowOffset;
            _targetFollowOffset = _baseFollowOffset;
        }

        private void Update()
        {
            ChangeFollowOffset();
        }

        private void ChangeFollowOffset()
        {
            if (_cinemachineTransposer.m_FollowOffset == _targetFollowOffset)
                return;
            
            float currentDistance = Vector3.Distance(_currentFollowOffset, _cinemachineTransposer.m_FollowOffset);
            float t = currentDistance / _distanceBetweenOffsets + _settings.SpeedChange;
            
            _cinemachineTransposer.m_FollowOffset =
                Vector3.Lerp(_currentFollowOffset, _targetFollowOffset, t);
        }

        public void ResetPosition()
        {
            _cinemachine.ForceCameraPosition(_playerMovement.transform.position, _playerMovement.transform.rotation);
        }

        public void ActivateRangeUp()
        {
            _currentFollowOffset = _baseFollowOffset;
            _targetFollowOffset = _settings.FollowOffsetUp;
            _distanceBetweenOffsets = Vector3.Distance(_currentFollowOffset, _targetFollowOffset);
        }

        public void DeactivateRangeUp()
        {
            _currentFollowOffset = _settings.FollowOffsetUp;
            _targetFollowOffset = _baseFollowOffset;
        }
    }
}
