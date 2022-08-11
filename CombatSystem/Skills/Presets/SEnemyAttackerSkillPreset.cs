using UnityEngine;

namespace CombatSystem.Skills.Presets
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Enemy Attacker Preset", order = 101)]
    public class SEnemyAttackerSkillPreset : SAttackerSkillPreset
    {
        protected override string GenerateAssetName()
        {
            return "[ENEMY] " + base.GenerateAssetName();
        }
    }

}
