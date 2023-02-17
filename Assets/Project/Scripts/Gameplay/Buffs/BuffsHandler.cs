using UnityEngine;
using Zenject;

using Gameplay.Items;
using Gameplay.Player;

namespace Gameplay.Buffs
{
    public class BuffsHandler : ITickable
    {
        private GameplaySettings _settings;
        private PlayerInventory _playerInventory;
        private PlayerMovement _playerMovement;
        private PlayerWatcher _playerWatcher;

        private bool _isActiveSpeedBuff;
        private float _startTimeSpeedBuff;
        private float _endTimeSpeedBuff;

        [Inject]
        public void Construct(GameplaySettings settings,
            PlayerInventory playerInventory, 
            PlayerMovement playerMovement, 
            PlayerWatcher playerWatcher)
        {
            _settings = settings;
            _playerInventory = playerInventory;
            _playerMovement = playerMovement;
            _playerWatcher = playerWatcher;
        }

        public void Tick()
        {
            ProcessInput();
            UpdateBuffs();
        }

        private void ProcessInput()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                TryActivateSpeedBoostBuff();
            if (Input.GetKeyDown(KeyCode.Alpha2))
                TryActivateVisibilityRangeUpBuff();
        }

        private void UpdateBuffs()
        {
            UpdateSpeedBuff();
        }

        private void UpdateSpeedBuff()
        {
            if (!_isActiveSpeedBuff
                || _endTimeSpeedBuff > Time.time)
                return;

            _playerMovement.DeactivateBoost();
            _isActiveSpeedBuff = false;
        }

        public void TryActivateSpeedBoostBuff()
        {
            if (!_playerInventory.ContainsItem(ItemType.SpeedBuff))
                return;

            if (!_isActiveSpeedBuff)
            {
                _playerMovement.ActivateBoost();
                _playerInventory.UseItem(ItemType.SpeedBuff);

                _isActiveSpeedBuff = true;
                _startTimeSpeedBuff = Time.time;
                _endTimeSpeedBuff = Time.time + _settings.TimeBoost;
            }
        }

        public float PercentageCompletionSpeedBuff()
        {
            if (!_isActiveSpeedBuff)
                return 0;

            float elapsedTime = Time.time - _startTimeSpeedBuff;

            return elapsedTime / _settings.TimeBoost;
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
