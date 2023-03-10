using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

using Localization;

namespace UI.Settings
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class LocaleDropdown : MonoBehaviour
    {
        private TMP_Dropdown _dropdown;

        private void Awake()
        {
            _dropdown = GetComponent<TMP_Dropdown>();
        }

        IEnumerator Start()
        {
            yield return LocalizationSettings.InitializationOperation;

            var options = new List<TMP_Dropdown.OptionData>();
            int selected = 0;
            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[i];
                if (LocalizationSettings.SelectedLocale == locale)
                    selected = i;
                options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
            }
            _dropdown.options = options;

            _dropdown.value = selected;
            _dropdown.onValueChanged.AddListener(LocaleSelected);
        }

        static void LocaleSelected(int index)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
            LocaleHadler.Change();
        }
    }
}
