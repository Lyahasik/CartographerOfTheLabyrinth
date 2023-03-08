using UnityEngine;
using Zenject;

using Environment;
using Gameplay.Progress;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour, IInitializable
    {
        private readonly int _walkingId = Animator.StringToHash("Walking");
        private const float _baseScale = 1f;

        private GameplaySettings _settings;
        private ProcessingProgress _processingProgress;
        private EnvironmentHandler _environmentHandler;
        
        private CharacterController _characterController;
        private Animator _animator;
        
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedTurn;
        private float _scaleSpeed = _baseScale;

        private Vector2Int _currentChunkId;

        private bool _isFreeze;
        private float _nextSaveTime;

        public bool IsFreeze
        {
            set => _isFreeze = value;
        }

        [Inject]
        public void Construct(GameplaySettings settings,
            ProcessingProgress processingProgress,
            EnvironmentHandler environmentHandler)
        {
            _settings = settings;
            _processingProgress = processingProgress;
            _environmentHandler = environmentHandler;
        }

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _animator = GetComponent<Animator>();
        }

        public void Initialize()
        {
            Vector3 position = transform.position;
            _processingProgress.TryLoadPlayerPosition(ref position);
            transform.position = position;
            
            _currentChunkId = _environmentHandler.TransformPositionByChunkId(transform.position.x, transform.position.z);
            _environmentHandler.UpdateChunks(in _currentChunkId);
        }

        private void Update()
        {
            if (_nextSaveTime > Time.time)
                return;
            
            _processingProgress.SavePlayerPosition(transform.position);
            _nextSaveTime = Time.time + _settings.DelaySave;
        }

        public void TakeStepMove(Vector2 stepMove)
        {
            if (_isFreeze)
                return;
            
            Vector3 step = new Vector3(stepMove.x, 0f, stepMove.y);
            if (step == Vector3.zero)
            {
                _animator.SetBool(_walkingId, false);
                return;
            }
            
            _animator.SetBool(_walkingId, true);
            
            step *= _speedMove * _scaleSpeed * Time.deltaTime;

            step = transform.TransformDirection(step);
        
            _characterController.Move(step);

            UpdateChunkId();
        }

        public void SetPosition(Vector3 newPosition)
        {
            _characterController.enabled = false;
        
            transform.position = newPosition + Vector3.up * _characterController.height * 0.5f;
            _processingProgress.SavePlayerPosition(transform.position);
        
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
            _animator.speed = _scaleSpeed;
        }

        public void DeactivateBoost()
        {
            _scaleSpeed = _baseScale;
            _animator.speed = _scaleSpeed;
        }
    }
}
