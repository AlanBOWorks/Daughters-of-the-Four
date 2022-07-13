using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Enemy Vanguard Preset", order = 100)]
    public class SEnemyVanguardSkillPreset : SVanguardSkillPreset
    {
    private const string VanguardAssetPrefix = " [Enemy Vanguard Skill]";
    protected override string GetAssetPrefix() => VanguardAssetPrefix;
    }
}
