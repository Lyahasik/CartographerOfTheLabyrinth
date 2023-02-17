using UnityEngine.EventSystems;

using Gameplay.Items;

namespace UI.Icons
{
    public class IconSpeedBuff : IconBuff, IPointerClickHandler
    {
        private void Awake()
        {
            _type = ItemType.SpeedBuff;
        }

        private void Update()
        {
            UpdateTimeAction(_buffsHandler.PercentageCompletionSpeedBuff());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buffsHandler.TryActivateSpeedBoostBuff();
        }
    }
}
