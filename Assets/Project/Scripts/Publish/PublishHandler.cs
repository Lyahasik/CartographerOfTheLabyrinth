using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

using Audio;
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

        private MouseHandler _mouseHandler;

        public static event Action<string> OnLoadData; 

        private int _magnificationNumber = 120;
        private int _maxDelayRegularAdsTime = 300;
        private int _delayRegularAdsTime = 60;
        private float _nextRegularAdsTime;

        private bool _isChangedCursor;

        [Inject]
        public void Construct(MouseHandler mouseHandler)
        {
            _mouseHandler = mouseHandler;
        }

        public void Start()
        {
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
            _delayRegularAdsTime += _magnificationNumber;
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
            
            if (_nextRegularAdsTime > Time.time)
                return;
            
            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;

            if (_delayRegularAdsTime != _maxDelayRegularAdsTime)
                _delayRegularAdsTime = Mathf.Clamp(_delayRegularAdsTime + _magnificationNumber, 0, _maxDelayRegularAdsTime);

            PrepareAds();
            
            AdsFullExtern();
        }

        public void AdsActive(int indexAward)
        {
            PrepareAds();
            
            AdsActiveExtern(indexAward);
        }

        private void PrepareAds()
        {
            if (!_mouseHandler.IsActive)
            {
                _isChangedCursor = true;
                _mouseHandler.ActivateCursor();
            }
            
            AudioHandler.DeactivateAll();
            Time.timeScale = 0f;
        }

        public void CloseAds()
        {
            if (_isChangedCursor)
            {
                _isChangedCursor = false;
                _mouseHandler.DeactivateCursor();
            }
            
            Time.timeScale = 1f;
            AudioHandler.ActivateClip(_musicClipName);
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
