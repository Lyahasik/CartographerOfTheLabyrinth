using UnityEngine;
using Zenject;

using Gameplay.Buffs;

namespace UI.Icons
{
    public class IconsPanel : MonoBehaviour, IInitializable
    {
        private BuffsHandler _buffsHandler;
        
        [SerializeField] private IconSpeedBuff _iconSpeedBuff;
        [SerializeField] private IconVisibilityRangeUpBuff _iconVisibilityRangeUpBuff;

        [Inject]
        public void Construct(BuffsHandler buffsHandler)
        {
            _buffsHandler = buffsHandler;
        }

        public void Initialize()
        {
            _iconSpeedBuff.Init(_buffsHandler);
            _iconVisibilityRangeUpBuff.Init(_buffsHandler);
        }
    }
}
