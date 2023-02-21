using System;

namespace Audio
{
    public static class AudioHandler
    {
        public static event Action<float> OnSetValueMusic;
        public static event Action<float> OnSetValueSounds;

        public static void SetValueMusic(float value)
        {
            OnSetValueMusic?.Invoke(value);
        }

        public static void SetValueSounds(float value)
        {
            OnSetValueSounds?.Invoke(value);
        }
    }
}
