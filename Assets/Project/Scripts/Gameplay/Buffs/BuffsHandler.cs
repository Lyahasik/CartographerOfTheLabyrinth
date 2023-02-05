using Gameplay.Items;
using Gameplay.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Buffs
{
    public class BuffsHandler : ITickable
    {
        private PlayerInventory _playerInventory;
        private PlayerMovement _playerMovement;

        [Inject]
        public void Construct(PlayerInventory playerInventory, PlayerMovement playerMovement)
        {
            _playerInventory = playerInventory;
            _playerMovement = playerMovement;
        }

        public void Tick()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                TryActivateBuff(ItemType.SpeedBuff);
        }

        public void TryActivateBuff(ItemType type)
        {
            if (!_playerInventory.ContainsItem(type))
                return;
        
            if (_playerMovement.TryActivateBoost())
                _playerInventory.UseItem(type);
        }
    }
}
