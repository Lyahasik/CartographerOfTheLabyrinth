using UnityEngine;
using Zenject;

using FiniteStateMachine;
using Audio;
using Gameplay.Progress;
using Publish;
using UnityEngine.UI;

namespace UI.Settings
{
    public class SettingsPanel : MonoBehaviour
    {
        private const string _paperClipName = "Paper";
        
        private DiContainer _container;
        private GameMashine _gameMashine;
        private ProcessingProgress _processingProgress;
        private PublishHandler _publishHandler;

        [SerializeField] private Slider _sliderMusic;
        [SerializeField] private Slider _sliderSounds;

        [Inject]
        public void Construct(DiContainer container,
            GameMashine gameMashine,
            ProcessingProgress processingProgress,
            PublishHandler publishHandler)
        {
            _container = container;
            _gameMashine = gameMashine;
            _processingProgress = processingProgress;
            _publishHandler = publishHandler;
        }

        public void Deactivate()
        {
            bool isUpdated = _processingProgress.MusicValue != _sliderMusic.value
                             || _processingProgress.SoundsValue != _sliderSounds.value;

            _processingProgress.MusicValue = _sliderMusic.value;
            _processingProgress.SoundsValue = _sliderSounds.value;
            
            if (_publishHandler.AllowedShowFullscreen())
            {
                _gameMashine.Enter(_container.Instantiate<PublishState>());
                _publishHandler.ViewFullscreenAds(_container.Instantiate<PlayingState>());
            }
            else
            {
                _gameMashine.Enter(_container.Instantiate<PlayingState>());
            }

            if (isUpdated)
                _processingProgress.SaveProgressData();
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
            AudioHandler.ActivateClip(_paperClipName);

            _sliderMusic.value = _processingProgress.MusicValue;
            _sliderSounds.value = _processingProgress.SoundsValue;
        }

        public void SetValueMusic(float value)
        {
            AudioHandler.SetValueMusic(value);
        }

        public void SetValueSounds(float value)
        {
            AudioHandler.SetValueSounds(value);
        }
    }
}
