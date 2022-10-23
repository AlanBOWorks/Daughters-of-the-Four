using System;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Localization
{
    public class ULocalizationChangeHandler : MonoBehaviour
    {
        [SerializeField] private LocalizationSettings localizationSettings;

        private void Awake()
        {
            localizationSettings.OnSelectedLocaleChanged += OnLocalizationChange;
        }

        private void Start()
        {
            OnLocalizationChange(localizationSettings.GetSelectedLocale());
        }

        private static void OnLocalizationChange(Locale locale)
        {
            var localeCode = locale.Identifier.Code;

            ProjectLocalizationSingleton.InvokeLocaleChange(localeCode);
        }

    }
}
