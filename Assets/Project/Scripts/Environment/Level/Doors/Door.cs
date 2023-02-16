using UnityEngine;
using Zenject;

using Gameplay.Player;
using UI.Alerts;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(Collider))]
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour
    {
        protected readonly int _openingId = Animator.StringToHash("Opening");

        protected DoorsHandler _doorsHandler;
        protected PlayerInventory _playerInventory;
        protected MessagePanel _messagePanel;
    
        protected Animator _animator;
    
        protected Level _level;

        [Inject]
        public void Construct(DoorsHandler doorsHandler, PlayerInventory playerInventory, MessagePanel messagePanel)
        {
            _doorsHandler = doorsHandler;
            _playerInventory = playerInventory;
            _messagePanel = messagePanel;
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