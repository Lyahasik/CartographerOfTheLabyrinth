﻿using UnityEngine;
using Zenject;

using Environment;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IInitializable
    {
        private const float _baseScale = 1f;

        private GameplaySettings _settings;
        private EnvironmentHandler _environmentHandler;
        
        private CharacterController _characterController;
        
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedTurn;
        private float _scaleSpeed = _baseScale;

        private Vector2Int _currentChunkId;

        private bool _isFreeze;

        public bool IsFreeze
        {
            set => _isFreeze = value;
        }

        [Inject]
        public void Construct(GameplaySettings settings, EnvironmentHandler environmentHandler)
        {
            _settings = settings;
            _environmentHandler = environmentHandler;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void Initialize()
        {
            _currentChunkId = _environmentHandler.TransformPositionByChunkId(transform.position.x, transform.position.z);
            _environmentHandler.UpdateChunks(in _currentChunkId);
        }

        public void TakeStepMove(Vector2 stepMove)
        {
            if (_isFreeze)
                return;
            
            Vector3 step = new Vector3(stepMove.x, 0f, stepMove.y);
            step *= _speedMove * _scaleSpeed * Time.deltaTime;

            step = transform.TransformDirection(step);
        
            _characterController.Move(step);

            UpdateChunkId();
        }

        public void SetPosition(Vector3 newPosition)
        {
            _characterController.enabled = false;
        
            transform.position = newPosition + Vector3.up * _characterController.height * 0.5f;
        
            _characterController.enabled = true;
        
            UpdateChunkId();
        }

        private void UpdateChunkId()
        {
            Vector2Int chunkId = _environmentHandler.TransformPositionByChunkId(transform.position.x, transform.position.z);

            if (_currentChunkId != chunkId)
            {
                _currentChunkId = chunkId;
            
                _environmentHandler.UpdateChunks(in _currentChunkId);
            }
        }

        public void TakeStepTurn(Vector2 stepTurn)
        {
            if (_isFreeze)
                return;
            
            transform.Rotate(Vector3.up, stepTurn.x * _speedTurn * Time.deltaTime, Space.World);
        }

        public void ActivateBoost()
        {
            _scaleSpeed = _settings.ScaleBoost;
        }

        public void DeactivateBoost()
        {
            _scaleSpeed = _baseScale;
        }
    }
}
