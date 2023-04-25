using UnityEngine.EventSystems;

using Gameplay.Items;

namespace UI.Gameplay
{
    public class IconSpeedBuff : IconBuff, IPointerClickHandler
    {
        private void Awake()
        {
            _type = ItemType.SpeedBuff;
        }

        private void Update()
        {
#if UNITY_EDITOR
            return;
#endif
            UpdateTimeAction(_buffsHandler.PercentageCompletionSpeedBuff());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buffsHandler.TryActivateSpeedBoostBuff();
        }
    }
}
