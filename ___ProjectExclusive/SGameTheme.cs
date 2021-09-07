using System;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
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
        [SerializeField]
        private RoleIconsTheme roleIconsTheme = new RoleIconsTheme();

        [TitleGroup("Aesthetics")]
        [SerializeField] private GameThemeColors themeColors = new GameThemeColors();

        
        public SerializableFullDrivenData<Sprite> SkillIcons => skillIcons;
        public ICharacterArchetypesData<Sprite> RoleIcons => roleIconsTheme;

        public GameThemeColors ThemeColors => themeColors;

        [Button]
        public void InjectInSingleton()
        {
            GameThemeSingleton.Instance.Injection(this);
        }

        [Serializable]
        private class RoleIconsTheme : ICharacterArchetypesData<Sprite>
        {
            [SerializeField] private Sprite vanguardSprite;
            [SerializeField] private Sprite attackerSprite;
            [SerializeField] private Sprite supportSprite;
            public Sprite Vanguard => vanguardSprite;
            public Sprite Attacker => attackerSprite;
            public Sprite Support => supportSprite;
        }
    }

}
