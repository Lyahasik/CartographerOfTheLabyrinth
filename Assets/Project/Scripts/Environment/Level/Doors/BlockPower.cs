using Gameplay.Player;
using Helpers;
using UnityEngine;
using Zenject;

namespace Environment.Level.Doors
{
    [RequireComponent(typeof(MeshRenderer))]
    public class BlockPower : MonoBehaviour
    {
        private DoorsHandler _doorsHandler;

        private MeshRenderer _meshRenderer;
 
        private Level _level;

        private DoorDirectionType _direction;
        private bool _isActive;

        [Inject]
        public void Construct(DoorsHandler doorsHandler)
        {
            _doorsHandler = doorsHandler;
        }

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            _direction = DoorHelper.DetermineDirection(transform.forward);

            if (_doorsHandler.IsActivePower(transform.position))
                _meshRenderer.material.color = Color.green;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isActive)
                return;
        
            if (other.GetComponent<PlayerMovement>())
            {
                _meshRenderer.material.color = Color.green;
                _doorsHandler.ActivatePower(transform.position, (int) _direction);
            }
        }
    }
}
