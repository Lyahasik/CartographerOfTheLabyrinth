using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Items;
using Gameplay.Player;
using Gameplay.Progress;
using Audio;
using Publish;

namespace UI.Shop
{
    public class ShopPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        private const int _indexAdsSpeedBuff = 0;
        private const int _indexAdsVisibilityBuff = 1;
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private PlayerInventory _playerInventory;
        private PublishHandler _publishHandler;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            PlayerInventory playerInventory,
            PublishHandler publishHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _playerInventory = playerInventory;
            _publishHandler = publishHandler;
        }

        private void OnEnable()
        {
            PublishHandler.OnActivateAward += BuyOneSpeedBuff;
            PublishHandler.OnActivateAward += BuyOneVisibilityBuff;
            PublishHandler.OnGetGoods += BuyGoods;
        }

        private void OnDisable()
        {
            PublishHandler.OnActivateAward -= BuyOneSpeedBuff;
            PublishHandler.OnActivateAward -= BuyOneVisibilityBuff;
            PublishHandler.OnGetGoods -= BuyGoods;
        }

        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PublishState>());
            _publishHandler.ViewFullscreenAds(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            AudioHandler.ActivateClip(_paperClipName);
        }

        public void BuyBuff(string idGoods)
        {
            _publishHandler.BayGoods(idGoods);
        }

        private void BuyGoods(string idGoods)
        {
            switch (idGoods)
            {
                case "speedBuffThree":
                    BuySpeedBuff(3);
                    break;
                case "speedBuffTen":
                    BuySpeedBuff(10);
                    break;
                case "visibilityBuffThree":
                    BuyVisibilityBuff(3);
                    break;
                case "visibilityBuffTen":
                    BuyVisibilityBuff(10);
                    break;
            }
        }

        public void BuySpeedBuff(int number)
        {
            _playerInventory.AddItem(ItemType.SpeedBuff, number);
            _processingProgress.SaveProgressData();
        }

        public void BuyVisibilityBuff(int number)
        {
            _playerInventory.AddItem(ItemType.VisibilityRangeBuff, number);
            _processingProgress.SaveProgressData();
        }

        public void BuyOneSpeedBuff(int index)
        {
            if (index == _indexAdsSpeedBuff)
                BuySpeedBuff(1);
        }

        public void BuyOneVisibilityBuff(int index)
        {
            if (index == _indexAdsVisibilityBuff)
                BuyVisibilityBuff(1);
        }

        public void FreeSpeedBuff()
        {
            _publishHandler.ViewVideoAds(_container.Instantiate<ShopState>(), _indexAdsSpeedBuff);
        }

        public void FreeVisibilityBuff()
        {
            _publishHandler.ViewVideoAds(_container.Instantiate<ShopState>(), _indexAdsVisibilityBuff);
        }
    }
}
