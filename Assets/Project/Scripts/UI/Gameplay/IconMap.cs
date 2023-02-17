using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

using FiniteStateMachine;

namespace UI.Gameplay
{
    public class IconMap : MonoBehaviour, IPointerClickHandler
    {
        private DiContainer _container;
        private GameMashine _gameMashine;

        public void Init(DiContainer container, GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }
    
        public void OnPointerClick(PointerEventData eventData)
        {
            _gameMashine.Enter(_container.Instantiate<MapState>());
        }
    }
}
