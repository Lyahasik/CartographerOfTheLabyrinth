using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

using FiniteStateMachine;
using Gameplay.Buffs;
using Gameplay.Player;
using Gameplay.Progress;
using Publish;

namespace UI.Gameplay
{
    public class GameplayPanel : MonoBehaviour
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private BuffsHandler _buffsHandler;
        private PlayerInventory _playerInventory;
        private PublishHandler _publishHandler;
        private MouseHandler _mouseHandler;
        
        [SerializeField] private IconSettings _iconSettings;
        [SerializeField] private IconMap _iconMap;
        
        [Header("Items")]
        [SerializeField] private IconShop _iconShop;
        [SerializeField] private IconSpeedBuff _iconSpeedBuff;
        [SerializeField] private IconVisibilityRangeUpBuff _iconVisibilityRangeUpBuff;

        [Header("Publish")]
        [SerializeField] private GameObject _rateWindow;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            BuffsHandler buffsHandler,
            PlayerInventory playerInventory,
            PublishHandler publishHandler,
            MouseHandler mouseHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _buffsHandler = buffsHandler;
            _playerInventory = playerInventory;
            _publishHandler = publishHandler;
            _mouseHandler = mouseHandler;
        }
        
        public IEnumerator StartLocalization()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_processingProgress.LocaleId];
        }

        public void Init()
        {
            _iconSettings.Init(_container, _gameMashine);
            _iconMap.Init(_container, _gameMashine);
            
            _iconShop.Init(_container, _gameMashine);
            _iconSpeedBuff.Init(_buffsHandler, _playerInventory);
            _iconVisibilityRangeUpBuff.Init(_buffsHandler, _playerInventory);
        }

        public void TryOpenRateWindow()
        {
            StartCoroutine(DelayedOpenRateWindow());
        }

        private IEnumerator DelayedOpenRateWindow()
        {
            yield return new WaitForSeconds(3f);

            _publishHandler.StartCheckRateGame();
        }

        public void OpenRateWindow()
        {
            _rateWindow.SetActive(true);
            _mouseHandler.ActivateCursor();
        }

        public void CloseRateWindow()
        {
            _rateWindow.SetActive(false);
            _mouseHandler.DeactivateCursor();
        }

        public void RateGame()
        {
            _publishHandler.StartRateGame();
            _rateWindow.SetActive(false);
        }
    }
}
