using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Player Vanguard Preset", order = 10)]
    public class SPlayerVanguardSkillPreset : SVanguardSkillPreset
    {
        protected override string GenerateAssetName()
        {
            return "[PLAYER] " + base.GenerateAssetName();
        }
    }
}
