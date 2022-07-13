using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Support Preset", order = -8)]
    public class SSupportSkillPreset : SSkillPreset, ISupportSkill
    {
        private const string AttackerAssetPrefix = "[Support Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
