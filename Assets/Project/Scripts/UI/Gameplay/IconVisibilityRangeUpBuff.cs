using UnityEngine.EventSystems;

using Gameplay.Items;

namespace UI.Gameplay
{
    public class IconVisibilityRangeUpBuff : IconBuff, IPointerClickHandler
    {
        private void Awake()
        {
            _type = ItemType.VisibilityRangeBuff;
        }

        private void Update()
        {
#if UNITY_EDITOR
            return;
#endif
            UpdateTimeAction(_buffsHandler.PercentageCompletionVisibilityRangeBuff());
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buffsHandler.TryActivateVisibilityRangeUpBuff();
        }
    }
}
