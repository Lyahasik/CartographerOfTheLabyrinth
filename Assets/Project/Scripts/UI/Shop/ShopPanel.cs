using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Items;
using Gameplay.Player;
using Gameplay.Progress;
using Audio;

namespace UI.Shop
{
    public class ShopPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private PlayerInventory _playerInventory;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            PlayerInventory playerInventory)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _playerInventory = playerInventory;
        }
    
        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            AudioHandler.ActivateClip(_paperClipName);
        }

        public void BuySpeedBuff(int number)
        {
            _playerInventory.AddItem(ItemType.SpeedBuff, number);
            _processingProgress.SaveNotUsedItems(ItemType.SpeedBuff);
        }

        public void BuyVisibilityBuff(int number)
        {
            _playerInventory.AddItem(ItemType.VisibilityRangeBuff, number);
            _processingProgress.SaveNotUsedItems(ItemType.VisibilityRangeBuff);
        }

        public void FreeSpeedBuff()
        {
            Debug.Log("Ads speed buff");
            BuySpeedBuff(1);
        }

        public void FreeVisibilityBuff()
        {
            Debug.Log("Ads visibility buff");
            BuyVisibilityBuff(1);
        }
    }
}
