using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Attacker Preset", order = -9)]
    public class SAttackerSkillPreset : SSkillPreset, IAttackerSkill
    {
        private const string AttackerAssetPrefix =  "[Attacker Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
