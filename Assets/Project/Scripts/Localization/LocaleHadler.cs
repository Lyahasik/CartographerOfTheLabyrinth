using System;

namespace Localization
{
    public class LocaleHadler
    {
        public static event Action OnChange;

        public static void Change()
        {
            OnChange?.Invoke();
        }
    }
}
