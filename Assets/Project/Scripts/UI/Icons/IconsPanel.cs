using UnityEngine;
using Zenject;

using Gameplay.Buffs;
using FiniteStateMachine;

namespace UI.Icons
{
    public class IconsPanel : MonoBehaviour, IInitializable
    {
        private DiContainer _container;
        private GameMashine _gameMashine;
        private BuffsHandler _buffsHandler;
        
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
            _iconMap.Init(_container, _gameMashine);
            _iconSpeedBuff.Init(_buffsHandler);
            _iconVisibilityRangeUpBuff.Init(_buffsHandler);
        }
    }
}
