using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Enemy Support Preset", order = 102)]
    public class SEnemySupportSkillPreset : SSupportSkillPreset
    {
        private const string AttackerAssetPrefix = "[Enemy Support Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
