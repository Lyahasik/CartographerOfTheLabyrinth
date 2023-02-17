using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Gameplay.Buffs;

namespace UI.Gameplay
{
    public class GameplayPanel : MonoBehaviour, IInitializable
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private BuffsHandler _buffsHandler;
        
        [SerializeField] private IconSettings _iconSettings;
        [SerializeField] private IconMap _iconMap;
        [SerializeField] private IconSpeedBuff _iconSpeedBuff;
        [SerializeField] private IconVisibilityRangeUpBuff _iconVisibilityRangeUpBuff;

        [Inject]
        public void Construct(DiContainer container, GameMashine gameMashine, BuffsHandler buffsHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _buffsHandler = buffsHandler;
        }

        public void Initialize()
        {
            _iconSettings.Init(_container, _gameMashine);
            _iconMap.Init(_container, _gameMashine);
            _iconSpeedBuff.Init(_buffsHandler);
            _iconVisibilityRangeUpBuff.Init(_buffsHandler);
        }
    }
}
