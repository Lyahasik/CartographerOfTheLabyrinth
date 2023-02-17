using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Gameplay.Items;
using Gameplay.Player;
using Gameplay.Buffs;
using UnityEngine.EventSystems;

namespace UI.Icons
{
    public class IconSpeedBuff : MonoBehaviour, IPointerClickHandler
    {
        private BuffsHandler _buffsHandler;
        
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private Image _timerImage;

        private ItemType _type;

        private void Awake()
        {
            _type = ItemType.SpeedBuff;
        }

        public void Init(BuffsHandler buffsHandler)
        {
            _buffsHandler = buffsHandler;
        }

        private void Update()
        {
            UpdateTimeAction();
        }

        private void UpdateTimeAction()
        {
            float percentageCompletion = _buffsHandler.PercentageCompletionSpeedBuff();

            if (percentageCompletion == 0)
                return;

            _timerImage.fillAmount = 1f - percentageCompletion;
        }

        private void OnEnable()
        {
            PlayerInventory.OnSetNumberItem += SetNumber;
        }

        private void OnDisable()
        {
            PlayerInventory.OnSetNumberItem -= SetNumber;
        }

        public void SetNumber(ItemType type, int number)
        {
            if (_type != type)
                return;

            _numberText.text = number.ToString();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _buffsHandler.TryActivateSpeedBoostBuff();
        }
    }
}
