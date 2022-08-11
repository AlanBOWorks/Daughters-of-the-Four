using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Enemy Vanguard Preset", order = 100)]
    public class SEnemyVanguardSkillPreset : SVanguardSkillPreset
    {
        protected override string GenerateAssetName()
        {
            return "[ENEMY] " + base.GenerateAssetName();
        }
    }
}
