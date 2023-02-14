using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        protected readonly int _openingId = Animator.StringToHash("Opening");

        protected DoorsHandler _doorsHandler;
        protected PlayerInventory _playerInventory;
    
        protected Animator _animator;
    
        protected Level _level;

        [Inject]
        public void Construct(DoorsHandler doorsHandler, PlayerInventory playerInventory)
        {
            _doorsHandler = doorsHandler;
            _playerInventory = playerInventory;
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Init(Level level)
        {
            _level = level;
        }

        private void DestroyYourself()
        {
            _level.DestroyObject(transform.parent.gameObject, true);
        }
    }
}