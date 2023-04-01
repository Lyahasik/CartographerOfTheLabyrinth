using UnityEngine;
using Zenject;

using Environment;
using Gameplay.Progress;
using Audio;
using Gameplay.FogOfWar;

namespace Gameplay.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    public class PlayerMovement : MonoBehaviour
    {
        private const float _baseScale = 1f;
        private const string _stepClipName = "Step";
        
        private readonly int _walkingId = Animator.StringToHash("Walking");

        private GameplaySettings _settings;
        private ProcessingProgress _processingProgress;
        private EnvironmentHandler _environmentHandler;
        
        private CharacterController _characterController;
        private Animator _animator;
        
        [SerializeField] private Blob _blob;
        
        [SerializeField] private float _speedMove;
        [SerializeField] private float _speedTurn;
        private float _scaleSpeed = _baseScale;

        private Vector2Int _currentChunkId;

        private bool _isFreeze = true;
        private bool _isUpdated;
        private float _nextSaveTime;

        public Blob Blob => _blob;

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

        // public void Initialize()
        // {
        //     Vector3 position = transform.position;
        //     _processingProgress.TryLoadPlayerPosition(ref position);
        //     transform.position = position;
        //     
        //     _currentChunkId = _environmentHandler.TransformPositionByChunkId(transform.position.x, transform.position.z);
        //     _environmentHandler.UpdateChunks(in _currentChunkId);
        // }

        private void Update()
        {
            if (!_isUpdated
                || _nextSaveTime > Time.time)
                return;
            
            _processingProgress.AxesPosition = transform.position;
            _nextSaveTime = Time.time + _settings.DelaySave;
            _isUpdated = false;
        }

        public void TakeStepMove(Vector2 stepMove)
        {
            if (_isFreeze)
                return;
            
            Vector3 step = new Vector3(stepMove.x, 0f, stepMove.y);
            if (step == Vector3.zero)
            {
                _animator.SetBool(_walkingId, false);
                AudioHandler.DeactivateClip(_stepClipName);
                return;
            }
            
            _isUpdated = true;
            _animator.SetBool(_walkingId, true);
            AudioHandler.ActivateClip(_stepClipName);
            
            step *= _speedMove * _scaleSpeed * Time.deltaTime;

            step = transform.TransformDirection(step);
        
            _characterController.Move(step);

            UpdateChunkId();
        }

        public void SetPosition(Vector3 newPosition)
        {
            _isFreeze = false;
            _isUpdated = true;
            _characterController.enabled = false;

            if (newPosition != Vector3.zero)
                transform.position = newPosition;

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
