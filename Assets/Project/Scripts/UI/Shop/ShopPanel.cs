using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Items;
using Gameplay.Player;

namespace UI.Shop
{
    public class ShopPanel : MonoBehaviour
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private PlayerInventory _playerInventory;

        [Inject]
        public void Construct(DiContainer container, GameMashine gameMashine, PlayerInventory playerInventory)
        {
            _container = container;
            _gameMashine = gameMashine;
            _playerInventory = playerInventory;
        }
    
        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }

        public void BuySpeedBuff(int number)
        {
            _playerInventory.AddItem(ItemType.SpeedBuff, number);
        }

        public void BuyVisibilityBuff(int number)
        {
            _playerInventory.AddItem(ItemType.VisibilityRangeBuff, number);
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
