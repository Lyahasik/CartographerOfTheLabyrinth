using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Audio;

namespace UI.Map
{
    public class MapPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        
        private DiContainer _container;
        private GameMashine _gameMashine;

        [Inject]
        public void Construct(DiContainer container, GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }

        private void Start()
        {
            transform.SetSiblingIndex(0);
        }

        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            AudioHandler.ActivateClip(_paperClipName);
        }
    }
}
