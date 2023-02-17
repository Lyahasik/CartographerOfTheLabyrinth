using UnityEngine.EventSystems;

using Gameplay.Items;

namespace UI.Icons
{
    public class IconVisibilityRangeUpBuff : IconBuff, IPointerClickHandler
    {
        private void Awake()
        {
            _type = ItemType.VisibilityRangeBuff;
        }

        private void Update()
        {
            UpdateTimeAction(_buffsHandler.PercentageCompletionVisibilityRangeBuff());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buffsHandler.TryActivateVisibilityRangeUpBuff();
        }
    }
}
