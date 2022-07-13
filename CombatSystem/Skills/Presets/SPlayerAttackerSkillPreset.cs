using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Player Attacker Preset", order = 11)]
    public class SPlayerAttackerSkillPreset : SAttackerSkillPreset
    {
        private const string AttackerAssetPrefix = " [Player Attacker Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
