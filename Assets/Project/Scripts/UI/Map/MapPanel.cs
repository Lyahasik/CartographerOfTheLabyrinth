using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Audio;
using Publish;

namespace UI.Map
{
    public class MapPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private PublishHandler _publishHandler;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            PublishHandler publishHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _publishHandler = publishHandler;
        }

        private void Awake()
        {
            transform.SetSiblingIndex(1);
        }

        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
            _gameMashine.Enter(_container.Instantiate<PublishState>());
            _publishHandler.ViewFullscreenAds();
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            AudioHandler.ActivateClip(_paperClipName);
        }
    }
}
