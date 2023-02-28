using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Buffs;
using Gameplay.Player;

namespace UI.Gameplay
{
    public class GameplayPanel : MonoBehaviour, IInitializable
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
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
            BuffsHandler buffsHandler,
            PlayerInventory playerInventory)
        {
            _container = container;
            _gameMashine = gameMashine;
            _buffsHandler = buffsHandler;
            _playerInventory = playerInventory;
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
        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}
