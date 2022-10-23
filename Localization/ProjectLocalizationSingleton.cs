using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace Localization
{
    public class ProjectLocalizationSingleton 
    {
        public static readonly ProjectLocalizationSingleton Instance = new ProjectLocalizationSingleton();
        public static readonly IReadOnlyList<string> LocaleIdentifiers;
        public static readonly IReadOnlyDictionary<string, int> LocaleIdentifiersDictionary;

        public const int EnglishIdentifierIndex = 0;
        public const int SpanishIdentifierIndex = EnglishIdentifierIndex +1;

        public const string EnglishLocaleIdentifier = "en";
        public const string SpanishLocaleIdentifier = "es";
        
        [ShowInInspector]
        public static string CurrentLanguage { get; private set; }

        static ProjectLocalizationSingleton()
        {
            LocaleIdentifiers = InitializeIdentifiersArray();

            var identifiersDictionary = new Dictionary<string, int>();
            LocaleIdentifiersDictionary = identifiersDictionary;
            AddInitializedIdentifiers();

            OnLocaleChange = DebugLocaleChange;

            IReadOnlyList<string> InitializeIdentifiersArray()
            {
                return new[]
                {
                    EnglishLocaleIdentifier, //english : 0
                    SpanishLocaleIdentifier //spanish : 1
                };
            }

            void AddInitializedIdentifiers()
            {
                for (int i = 0; i < LocaleIdentifiers.Count; i++)
                {
                    var identifierValue = LocaleIdentifiers[i];
                    identifiersDictionary.Add(identifierValue, i);
                }
            }
        }

        public delegate void OnLanguageChangeDelegate(string localeCode, int localeIndex);

        public static event OnLanguageChangeDelegate OnLocaleChange;

        public static void InvokeLocaleChange(string targetLocale)
        {
            CurrentLanguage = targetLocale;
            OnLocaleChange.Invoke(targetLocale,LocaleIdentifiersDictionary[targetLocale]);
        }

        private static void DebugLocaleChange(string targetLocale, int targetLocaleId)
        {
#if UNITY_EDITOR
            Debug.Log($"[ Project LOCALE ] Changed: {targetLocale} / ID: {targetLocaleId}");
#endif
        }
    }

    public class ProjectLocalizationSourcesSingleton
    {
        public static readonly ProjectLocalizationSourcesSingleton Instance = new ProjectLocalizationSourcesSingleton();

    }


    public class LocalizationsCombatSingletonWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private static ProjectLocalizationSingleton _singleton;

        [ShowInInspector] 
        private static ProjectLocalizationSourcesSingleton _sources;

        [MenuItem("Game/Localizations/Main", priority = -10)]
        private static void GetWindow()
        {
            var window = GetWindow<LocalizationsCombatSingletonWindow>();
            _singleton = ProjectLocalizationSingleton.Instance;
            _sources = ProjectLocalizationSourcesSingleton.Instance;
        }
    }
}
