using Audio;
using UnityEngine;

namespace UI.Settings
{
    public class SoundsAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource[] _soundsSources;

        private void OnEnable()
        {
            AudioHandler.OnSetValueSounds += SetValue;
        }

        private void OnDisable()
        {
            AudioHandler.OnSetValueSounds -= SetValue;
        }

        private void SetValue(float value)
        {
            foreach (AudioSource audioSource in _soundsSources)
            {
                audioSource.volume = value;
            }
        }
    }
}
