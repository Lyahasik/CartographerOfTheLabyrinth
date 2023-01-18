using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace CartographerOfTheLabyrinth.Gameplay.Player
{
    public class PCMovementController : IInitializable, ITickable
    {
        private PlayerSettings _playerSettings;

        private PlayerMovement _playerMovement;
    
        private UnityEvent<Vector2> _onInputMove = new ();
        private UnityEvent<Vector2> _onInputTurn = new ();

        [Inject]
        public void Construct(PlayerSettings playerSettings, PlayerMovement playerMovement)
        {
            _playerSettings = playerSettings;
            _playerMovement = playerMovement;
        }

        public void Initialize()
        {
            _onInputMove.AddListener(_playerMovement.TakeStepMove);
            _onInputTurn.AddListener(_playerMovement.TakeStepTurn);
        }

        public void Tick()
        {
            ProcessingKeys();
            ProcessingMouse();
        }

        private void ProcessingKeys()
        {
            Vector2 vectorMove = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        
            _onInputMove?.Invoke(vectorMove);
        }

        private void ProcessingMouse()
        {
            Vector2 vectorMove = new Vector2(Input.GetAxis("Mouse X") * _playerSettings.MouseSensitivity, 0f);
        
            _onInputTurn?.Invoke(vectorMove);
        }
    }
}