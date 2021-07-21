using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ___ProjectExclusive
{
    [CreateAssetMenu(fileName = "Game Theme [Singleton]",
        menuName = "Singleton/Game Theme")]
    public class SGameTheme : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField] private GameThemeColors themeColors = new GameThemeColors();
#endif
        public GameThemeColors ThemeColors => themeColors;

        [Button]
        public void InjectInSingleton()
        {
            GameThemeSingleton.Instance.Injection(this);
        }
    }

}
