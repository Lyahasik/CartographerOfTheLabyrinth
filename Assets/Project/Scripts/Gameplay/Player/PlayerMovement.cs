using Environment;
using UnityEngine;
using Zenject;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour, IInitializable
    {
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedTurn;

        private CharacterController _characterController;
        private EnvironmentHandler _environmentHandler;

        private Vector2Int _currentChunkId;

        private bool _isFreeze;

        public bool IsFreeze
        {
            set => _isFreeze = value;
        }

        [Inject]
        public void Construct(EnvironmentHandler environmentHandler)
        {
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
            step *= _speedMove * Time.deltaTime;

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
    }
}
