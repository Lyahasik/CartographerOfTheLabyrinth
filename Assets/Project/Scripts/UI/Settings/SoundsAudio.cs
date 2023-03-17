using UnityEngine;

using Audio;

namespace UI.Settings
{
    public class SoundsAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _soundsSources;

        private void OnEnable()
        {
            AudioHandler.OnSetValueSounds += SetValue;
            AudioHandler.OnActivateClip += TryActivateClip;
            AudioHandler.OnDeactivateClip += TryDeactivateClip;
        }

        private void OnDisable()
        {
            AudioHandler.OnSetValueSounds -= SetValue;
            AudioHandler.OnActivateClip -= TryActivateClip;
            AudioHandler.OnDeactivateClip -= TryDeactivateClip;
        }

        private void SetValue(float value)
        {
            foreach (AudioSource audioSource in _soundsSources)
            {
                audioSource.volume = value;
            }
        }

        private void TryActivateClip(string name)
        {
            foreach (AudioSource source in _soundsSources)
            {
                if (source.clip.name == name)
                {
                    if (!source.isPlaying)
                        source.Play();
                    
                    return;
                }
            }
        }

        private void TryDeactivateClip(string name)
        {
            foreach (AudioSource source in _soundsSources)
            {
                if (source.clip.name == name)
                {
                    source.Stop();
                    return;
                }
            }
        }
    }
}
