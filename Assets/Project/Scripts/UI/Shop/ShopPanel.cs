using UnityEngine;
using Zenject;

using FiniteStateMachine;

namespace UI.Shop
{
    public class ShopPanel : MonoBehaviour
    {
        private DiContainer _container;
        private GameMashine _gameMashine;

        [Inject]
        public void Construct(DiContainer container, GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }
    
        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
