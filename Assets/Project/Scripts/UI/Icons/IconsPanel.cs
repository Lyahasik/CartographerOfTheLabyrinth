using UnityEngine;
using Zenject;

using Gameplay.Buffs;

namespace UI.Icons
{
    public class IconsPanel : MonoBehaviour, IInitializable
    {
        private BuffsHandler _buffsHandler;
        
        [SerializeField] private IconSpeedBuff _iconSpeedBuff;

        [Inject]
        public void Construct(BuffsHandler buffsHandler)
        {
            _buffsHandler = buffsHandler;
        }

        public void Initialize()
        {
            _iconSpeedBuff.Init(_buffsHandler);
        }
    }
}
