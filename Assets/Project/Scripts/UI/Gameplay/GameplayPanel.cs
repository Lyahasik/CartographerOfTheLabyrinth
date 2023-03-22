using System.Collections;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

using FiniteStateMachine;
using Gameplay.Buffs;
using Gameplay.Player;
using Gameplay.Progress;

namespace UI.Gameplay
{
    public class GameplayPanel : MonoBehaviour, IInitializable
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private BuffsHandler _buffsHandler;
        private PlayerInventory _playerInventory;
        
        [SerializeField] private IconSettings _iconSettings;
        [SerializeField] private IconMap _iconMap;
        
        [Header("Items")]
        [SerializeField] private IconShop _iconShop;
        [SerializeField] private IconSpeedBuff _iconSpeedBuff;
        [SerializeField] private IconVisibilityRangeUpBuff _iconVisibilityRangeUpBuff;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            BuffsHandler buffsHandler,
            PlayerInventory playerInventory)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _buffsHandler = buffsHandler;
            _playerInventory = playerInventory;
        }
        
        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_processingProgress.LocaleId];
        }

        public void Initialize()
        {
            _iconSettings.Init(_container, _gameMashine);
            _iconMap.Init(_container, _gameMashine);
            
            _iconShop.Init(_container, _gameMashine);
            _iconSpeedBuff.Init(_buffsHandler, _playerInventory);
            _iconVisibilityRangeUpBuff.Init(_buffsHandler, _playerInventory);
        }

        //TODO удалить

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ResetProgress();
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
