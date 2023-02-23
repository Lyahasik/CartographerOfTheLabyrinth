using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Gameplay.Buffs;
using Gameplay.Items;
using Gameplay.Player;

namespace UI.Gameplay
{
    public abstract class IconBuff : MonoBehaviour
    {
        [SerializeField] private TMP_Text _numberText;
        [SerializeField] private Image _timerImage;
    
        protected BuffsHandler _buffsHandler;
        protected PlayerInventory _playerInventory;
    
        protected ItemType _type;

        public void Init(BuffsHandler buffsHandler, PlayerInventory playerInventory)
        {
            _buffsHandler = buffsHandler;
            _playerInventory = playerInventory;
            
            SetNumber(_type, _playerInventory.GetNumberItem(_type));
        }
    
        protected void UpdateTimeAction(float percentageCompletion)
        {
            if (percentageCompletion == 0)
                return;

            _timerImage.fillAmount = 1f - percentageCompletion;
        }
    
        private void OnEnable()
        {
            PlayerInventory.OnSetNumberItem += SetNumber;
            
            if (_playerInventory != null)
                SetNumber(_type, _playerInventory.GetNumberItem(_type));
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
    }
}
