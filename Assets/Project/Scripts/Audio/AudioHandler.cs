using System;

namespace Audio
{
    public static class AudioHandler
    {
        public static event Action<float> OnSetValueMusic;
        public static event Action<float> OnSetValueSounds;
        
        public static event Action<string> OnActivateClip;
        public static event Action<string> OnDeactivateClip;
        public static event Action OnDeactivateAll;

        public static void SetValueMusic(float value)
        {
            OnSetValueMusic?.Invoke(value);
        }

        public static void SetValueSounds(float value)
        {
            OnSetValueSounds?.Invoke(value);
        }

        public static void ActivateClip(string name)
        {
            OnActivateClip?.Invoke(name);
        }

        public static void DeactivateClip(string name)
        {
            OnDeactivateClip?.Invoke(name);
        }

        public static void DeactivateAll()
        {
            OnDeactivateAll?.Invoke();
        }
    }
}
