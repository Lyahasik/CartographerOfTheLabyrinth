using Audio;
using TMPro;
using UnityEngine;
using Zenject;

using FiniteStateMachine;

namespace UI.Settings
{
    public class SettingsPanel : MonoBehaviour
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
