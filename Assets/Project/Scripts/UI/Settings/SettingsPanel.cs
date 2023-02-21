using Audio;
using TMPro;
using UnityEngine;
using Zenject;

using FiniteStateMachine;

namespace UI.Settings
{
    public class SettingsPanel : MonoBehaviour
    {
        //TODO локализовать
        private const string _musicTextValue = "Музыка"; 
        private const string _soundsTextValue = "Звуки"; 
        
        private DiContainer _container;
        private GameMashine _gameMashine;

        [SerializeField] private TMP_Text _musicText;
        [SerializeField] private TMP_Text _soundsText;

        [Inject]
        public void Construct(DiContainer container, GameMashine gameMashine)
        {
            _container = container;
            _gameMashine = gameMashine;
        }

        private void Start()
        {
            _musicText.text = _musicTextValue;
            _soundsText.text = _soundsTextValue;
        }

        public void Deactivate()
        {
            _gameMashine.Enter(_container.Instantiate<PlayingState>());
        }
        
        public void Activate(bool value)
        {
            gameObject.SetActive(value);
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
