using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Attacker Preset")]
    public class SAttackerSkillPreset : SSkillPreset, IAttackerSkill
    {
        private const string AttackerAssetPrefix =  "[Attacker Preset]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
