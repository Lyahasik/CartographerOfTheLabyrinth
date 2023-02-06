using UnityEngine;
using Zenject;

using Gameplay.Items;
using Gameplay.Player;

namespace Gameplay.Buffs
{
    public class BuffsHandler : ITickable
    {
        private PlayerInventory _playerInventory;
        private PlayerMovement _playerMovement;
        private PlayerWatcher _playerWatcher;

        [Inject]
        public void Construct(PlayerInventory playerInventory, 
            PlayerMovement playerMovement, 
            PlayerWatcher playerWatcher)
        {
            _playerInventory = playerInventory;
            _playerMovement = playerMovement;
            _playerWatcher = playerWatcher;
        }

        public void Tick()
        {
            ProcessInput();
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                TryActivateSpeedBoostBuff();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                TryActivateVisibilityRangeUpBuff();
        }

        public void TryActivateSpeedBoostBuff()
        {
            if (!_playerInventory.ContainsItem(ItemType.SpeedBuff))
                return;
        
            if (_playerMovement.TryActivateBoost())
                _playerInventory.UseItem(ItemType.SpeedBuff);
        }

        public void TryActivateVisibilityRangeUpBuff()
        {
            if (!_playerInventory.ContainsItem(ItemType.VisibilityRangeBuff))
                return;
        
            if (_playerWatcher.TryActivateRangeUp())
                _playerInventory.UseItem(ItemType.VisibilityRangeBuff);
        }
    }
}
