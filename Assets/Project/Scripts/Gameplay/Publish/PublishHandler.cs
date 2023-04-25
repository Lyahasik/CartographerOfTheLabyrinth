using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

using FiniteStateMachine;
using UI.Gameplay;

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

        private const float _delayStartRegularAds = 3f;

        private DiContainer _container;
        private GameMashine _gameMashine;
        private GameplayPanel _gameplayPanel;

        public static event Action<string> OnLoadData; 

        private int _magnificationNumber = 120;
        private int _maxDelayRegularAdsTime = 300;
        private int _delayRegularAdsTime = 60;
        private float _nextRegularAdsTime;
        
        private int _delayFullscreenAdsTime = 60;
        private float _nextFullscreenAdsTime;

        private GameState _nextGameState;
        private bool _isActive;
        private bool _isRegular;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            GameplayPanel gameplayPanel)
        {
            _gameMashine = gameMashine;
            _container = container;
            _gameplayPanel = gameplayPanel;
        }

        public void Start()
        {
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
        }
    
        public void Update()
        {
            PrepareRegularAds();
        }

        private void PrepareRegularAds()
        {
#if UNITY_EDITOR
        return;
#endif
            
            if (_nextRegularAdsTime > Time.time
                || _nextFullscreenAdsTime > Time.time)
                return;
            
            _gameplayPanel.ViewWarningStartAds();

            _nextGameState = _gameMashine.CurrentState;
            _gameMashine.Enter(_container.Instantiate<PublishState>());
            
            if (_delayRegularAdsTime != _maxDelayRegularAdsTime)
                _delayRegularAdsTime = Mathf.Clamp(_delayRegularAdsTime + _magnificationNumber, 0, _maxDelayRegularAdsTime);
            
            _nextRegularAdsTime = Time.time + _delayStartRegularAds;
            
            Invoke(nameof(StartRegularAds), _delayStartRegularAds);
        }

        private void StartRegularAds()
        {
            _isRegular = true;
            
            PrepareAds();
            
            AdsFullExtern();
        }

        public void ViewFullscreenAds(GameState nextGameState)
        {
#if UNITY_EDITOR
        return;
#endif

            if (_nextFullscreenAdsTime > Time.time)
            {
                _gameMashine.Enter(nextGameState);
                return;
            }

            _nextGameState = nextGameState;

            PrepareAds();
            
            AdsFullExtern();
        }

        public void ViewVideoAds(GameState nextGameState, int indexAward)
        {
            _gameMashine.Enter(_container.Instantiate<PublishState>());
            _nextGameState = nextGameState;
            
            PrepareAds();
            
            AdsActiveExtern(indexAward);
        }

        private void PrepareAds()
        {
            _isActive = true;
            Time.timeScale = 0f;
        }

        public void CloseAds()
        {
            if (!_isActive)
                return;
            
            if (!_isRegular)
                _gameMashine.Enter(_container.Instantiate<PlayingState>());
            else
                _isRegular = false;
            
            Time.timeScale = 1f;
            _isActive = false;
            
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
            _nextFullscreenAdsTime = Time.time + _delayFullscreenAdsTime;
        }

        public void NextState()
        {
            _gameMashine.Enter(_nextGameState);
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
