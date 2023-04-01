using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Gameplay.Progress;
using Newtonsoft.Json;
using UnityEngine;

namespace Publish
{
    public class PublishHandler : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void AdsBlock();
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

        public static event Action<string> OnLoadData; 

        private int _magnificationNumber = 120;
        private int _maxDelayRegularAdsTime = 300;
        private int _delayRegularAdsTime = 60;
        private float _nextRegularAdsTime;

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
            if (_nextRegularAdsTime > Time.time)
                return;

            if (_delayRegularAdsTime != _maxDelayRegularAdsTime)
                _delayRegularAdsTime = Mathf.Clamp(_delayRegularAdsTime + _magnificationNumber, 0, _maxDelayRegularAdsTime);
        
#if !UNITY_EDITOR
        AdsBlock();
#endif

            _nextRegularAdsTime = Time.time + _delayRegularAdsTime;
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
            // ProcessingProgress.SettingsGameplay settingsGameplay = new ProcessingProgress.SettingsGameplay();
            // settingsGameplay.Lessons = new HashSet<int> {3};
            // string json = JsonConvert.SerializeObject(settingsGameplay, new JsonSerializerSettings());
            // LoadData(json);
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
