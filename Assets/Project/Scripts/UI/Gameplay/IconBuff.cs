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
    
        protected ItemType _type;

        public void Init(BuffsHandler buffsHandler)
        {
            _buffsHandler = buffsHandler;
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
