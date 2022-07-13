using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Player Vanguard Preset", order = 10)]
    public class SPlayerVanguardSkillPreset : SVanguardSkillPreset
    {
        private const string VanguardAssetPrefix = " [Player Vanguard Skill]";
        protected override string GetAssetPrefix() => VanguardAssetPrefix;
    }
}
