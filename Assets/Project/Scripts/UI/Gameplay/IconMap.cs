using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using Zenject;

using FiniteStateMachine;
using Gameplay.FogOfWar;

namespace UI.Gameplay
{
    public class IconMap : MonoBehaviour, IPointerClickHandler
    {
        private DiContainer _container;
        private GameMashine _gameMashine;

        [SerializeField] private TMP_Text _text;

        public void Init(DiContainer container, GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }

        private void OnEnable()
        {
            FogOfWar.OnProgressPercentage += UpdateProgress;
        }

        private void OnDisable()
        {
            FogOfWar.OnProgressPercentage -= UpdateProgress;
        }

        private void UpdateProgress(float value)
        {
            _text.text = value.ToString("0.0") + "%";
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _gameMashine.Enter(_container.Instantiate<MapState>());
        }
    }
}
