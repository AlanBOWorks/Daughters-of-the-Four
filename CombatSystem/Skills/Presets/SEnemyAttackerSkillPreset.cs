using UnityEngine;

namespace CombatSystem.Skills.Presets
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Enemy Attacker Preset", order = 101)]
    public class SEnemyAttackerSkillPreset : SAttackerSkillPreset
    {
        private const string AttackerAssetPrefix = " [Enemy Attacker Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
