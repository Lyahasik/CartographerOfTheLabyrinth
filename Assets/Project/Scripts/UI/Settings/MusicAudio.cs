using UnityEngine;

using Audio;

namespace UI.Settings
{
    public class MusicAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;

        private void OnEnable()
        {
            AudioHandler.OnSetValueMusic += SetValue;
            AudioHandler.OnActivateClip += TryActivateClip;
            AudioHandler.OnDeactivateClip += TryDeactivateClip;
            AudioHandler.OnDeactivateAll += DeactivateAll;
        }

        private void OnDisable()
        {
            AudioHandler.OnSetValueMusic -= SetValue;
            AudioHandler.OnActivateClip -= TryActivateClip;
            AudioHandler.OnDeactivateClip -= TryDeactivateClip;
            AudioHandler.OnDeactivateAll -= DeactivateAll;
        }

        private void SetValue(float value)
        {
            _musicSource.volume = value;
        }

        private void TryActivateClip(string name)
        {
            if (_musicSource.clip.name == name)
            {
                _musicSource.Play();
            }
        }

        private void TryDeactivateClip(string name)
        {
            if (_musicSource.clip.name == name)
            {
                _musicSource.Pause();
            }
        }
        
        private void DeactivateAll()
        {
            _musicSource.Pause();
        }
    }
}
