using UnityEngine;
using Zenject;

using Gameplay.Player;

namespace UI.Movement
{
    public class MovementPanel : MonoBehaviour
    {
        [SerializeField] private Joystick.Joystick _joystickMove;
        [SerializeField] private Joystick.Joystick _joystickTurn;

        private PlayerMovement _playerMovement;

        [Inject]
        public void Construct(PlayerMovement playerMovement)
        {
            _playerMovement = playerMovement;
        }

        public void Start()
        {
            _joystickMove.OnDrag.AddListener(_playerMovement.TakeStepMove);
            _joystickTurn.OnDrag.AddListener(_playerMovement.TakeStepTurn);
        }
    }
}
