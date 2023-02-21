using Audio;
using UnityEngine;

namespace UI.Settings
{
    public class MusicAudio : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicSource;

        private void OnEnable()
        {
            AudioHandler.OnSetValueMusic += SetValue;
        }

        private void OnDisable()
        {
            AudioHandler.OnSetValueMusic -= SetValue;
        }

        private void SetValue(float value)
        {
            _musicSource.volume = value;
        }
    }
}
