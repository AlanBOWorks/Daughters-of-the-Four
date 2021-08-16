using System;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;

namespace ___ProjectExclusive
{
    [CreateAssetMenu(fileName = "Game Theme [Singleton]",
        menuName = "Singleton/Game Theme")]
    public class SGameTheme : ScriptableObject
    {

        [TitleGroup("Aesthetics")] 
        [SerializeField]
        private SkillsIconTheme skillIcons = new SkillsIconTheme();

        [TitleGroup("Aesthetics")]
        [SerializeField] private GameThemeColors themeColors = new GameThemeColors();


        public IStatDrivenEntity<Sprite> SkillIcons => skillIcons;
        public GameThemeColors ThemeColors => themeColors;

        [Button]
        public void InjectInSingleton()
        {
            GameThemeSingleton.Instance.Injection(this);
        }
    }

}
