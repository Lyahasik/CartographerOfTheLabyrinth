using System.Runtime.InteropServices;
using UnityEngine;

namespace Publish
{
    public class PublishHandler : MonoBehaviour
    {
        [DllImport("__Internal")]
        private static extern void AdsBlock();
        [DllImport("__Internal")]
        private static extern void CheckRateGame();
        [DllImport("__Internal")]
        private static extern void RateGame();
    
        [DllImport("__Internal")]
        private static extern void SetLeaderBoard(int value);
    
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
    }
}
