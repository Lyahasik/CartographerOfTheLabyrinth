using UnityEngine;
using Zenject;

using Gameplay.Items;
using Gameplay.Player;
using Audio;

namespace Gameplay.Buffs
{
    public class BuffsHandler : ITickable
    {
        private const string _visibilityRadngeUpClipName = "VisibilityRangeUp";
        private const string _speedUpClipName = "SpeedUp";
        
        private GameplaySettings _settings;
        private PlayerInventory _playerInventory;
        private PlayerMovement _playerMovement;
        private PlayerWatcher _playerWatcher;

        private bool _isActiveSpeedBuff;
        private float _elapsedTimeSpeedBuff;

        private bool _isActiveVisibilityRangeBuff;
        private float _elapsedTimeVisibilityRangeBuff;

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
            if (Input.GetKeyDown(KeyCode.E))
                TryActivateVisibilityRangeUpBuff();
        }

        private void UpdateBuffs()
        {
            UpdateSpeedBuff();
            UpdateVisibilityRangeBuff();
        }

        private void UpdateSpeedBuff()
        {
            if (!_isActiveSpeedBuff
                || _elapsedTimeSpeedBuff < _settings.TimeBoost)
                return;

            _playerMovement.DeactivateBoost();
            _isActiveSpeedBuff = false;
        }

        private void UpdateVisibilityRangeBuff()
        {
            if (!_isActiveVisibilityRangeBuff
                || _elapsedTimeVisibilityRangeBuff < _settings.TimeFollowOffsetUp)
                return;

            _playerWatcher.DeactivateRangeUp();
            _playerMovement.Blob.DownScale();
            _isActiveVisibilityRangeBuff = false;
        }

        public float PercentageCompletionSpeedBuff()
        {
            if (!_isActiveSpeedBuff)
                return 0;

            _elapsedTimeSpeedBuff += Time.deltaTime;

            return _elapsedTimeSpeedBuff / _settings.TimeBoost;
        }

        public float PercentageCompletionVisibilityRangeBuff()
        {
            if (!_isActiveVisibilityRangeBuff)
                return 0;

            _elapsedTimeVisibilityRangeBuff += Time.deltaTime;

            return _elapsedTimeVisibilityRangeBuff / _settings.TimeFollowOffsetUp;
        }

        public void TryActivateVisibilityRangeUpBuff()
        {
            if (!_playerInventory.ContainsItem(ItemType.VisibilityRangeBuff))
                return;

            if (!_isActiveVisibilityRangeBuff)
            {
                _playerWatcher.ActivateRangeUp();
                _playerMovement.Blob.UpScale();
                _playerInventory.UseItem(ItemType.VisibilityRangeBuff);
                AudioHandler.ActivateClip(_visibilityRadngeUpClipName);

                _isActiveVisibilityRangeBuff = true;
                _elapsedTimeVisibilityRangeBuff = 0f;
            }
        }
        
        public void TryActivateSpeedBoostBuff()
        {
            if (!_playerInventory.ContainsItem(ItemType.SpeedBuff))
                return;

            if (!_isActiveSpeedBuff)
            {
                _playerMovement.ActivateBoost();
                _playerInventory.UseItem(ItemType.SpeedBuff);
                AudioHandler.ActivateClip(_speedUpClipName);

                _isActiveSpeedBuff = true;
                _elapsedTimeSpeedBuff = 0f;
            }
        }
    }
}
