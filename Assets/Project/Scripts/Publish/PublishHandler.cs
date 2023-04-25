using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

using FiniteStateMachine;
using UI;

namespace Publish
{
    public class PublishHandler : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void AdsFullExtern();
        [DllImport("__Internal")]
        private static extern void AdsActiveExtern(int indexAward);
        [DllImport("__Internal")]
        private static extern void BuyGoodsExtern(string idGoods);
        
        [DllImport("__Internal")]
        private static extern void LoadDataExtern();
        [DllImport("__Internal")]
        private static extern void SaveDataExtern(string data);
        [DllImport("__Internal")]
        private static extern void CheckRateGame();
        [DllImport("__Internal")]
        private static extern void RateGame();
    
        [DllImport("__Internal")]
        private static extern void SetLeaderBoard(int value);

        public static event Action<int> OnActivateAward; 
        public static event Action<string> OnGetGoods; 

        private const string _musicClipName = "Music";

        private DiContainer _container;
        private MouseHandler _mouseHandler;
        private GameMashine _gameMashine;

        public static event Action<string> OnLoadData; 

        private int _magnificationNumber = 15;
        private int _maxDelayRegularAdsTime = 90;
        private int _delayRegularAdsTime = 60;
        private float _nextRegularAdsTime;
        
        private int _delayFullscreenAdsTime = 60;
        private float _nextFullscreenAdsTime;

        private bool _isChangedCursor;

        [Inject]
        public void Construct(DiContainer container,
            MouseHandler mouseHandler,
            GameMashine gameMashine)
        {
            _mouseHandler = mouseHandler;
            _gameMashine = gameMashine;
            _container = container;
        }

        public void Start()
        {
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
        }
    
        public void Update()
        {
            RegularAds();
        }

        private void RegularAds()
        {
#if UNITY_EDITOR
        return;
#endif
            
            // if (_nextRegularAdsTime > Time.time
            //     || _nextFullscreenAdsTime > Time.time)
            //     return;
            //
            // _gameMashine.Enter(_container.Instantiate<PublishState>());
            //
            // if (_delayRegularAdsTime != _maxDelayRegularAdsTime)
            //     _delayRegularAdsTime = Mathf.Clamp(_delayRegularAdsTime + _magnificationNumber, 0, _maxDelayRegularAdsTime);
            //
            // PrepareAds();
            //
            // AdsFullExtern();
        }

        public void ViewFullscreenAds()
        {
#if UNITY_EDITOR
        return;
#endif

            if (_nextFullscreenAdsTime > Time.time)
            {
                _gameMashine.Enter(_container.Instantiate<PlayingState>());
                return;
            }

            PrepareAds();
            
            AdsFullExtern();
        }

        public void ViewVideoAds(int indexAward)
        {
            _gameMashine.Enter(_container.Instantiate<PublishState>());
            
            PrepareAds();
            
            AdsActiveExtern(indexAward);
        }

        private void PrepareAds()
        {
            Time.timeScale = 0f;
        }

        public void CloseAds()
        {
            _gameMashine.ResetState();
            
            Time.timeScale = 1f;
            
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
            _nextFullscreenAdsTime = Time.time + _delayFullscreenAdsTime;
        }

        public void GetAward(int index)
        {
            OnActivateAward?.Invoke(index);
        }

        public void BayGoods(string idGoods)
        {
            BuyGoodsExtern(idGoods);
        }

        public void GetGoods(string idGoods)
        {
            OnGetGoods?.Invoke(idGoods);
        }

        public void StartCheckRateGame()
        {
            CheckRateGame();
        }

        public void StartRateGame()
        {
            RateGame();
        }

        public void UpdateLeaderboard(int value)
        {
            SetLeaderBoard(value);
        }

        public void StartLoadData()
        {
            LoadDataExtern();
        }

        public void LoadData(string json)
        {
            OnLoadData?.Invoke(json);
        }

        public void SaveData(string data)
        {
            SaveDataExtern(data);
        }
    }
}
