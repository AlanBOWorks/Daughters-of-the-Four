using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Player Attacker Preset", order = 11)]
    public class SPlayerAttackerSkillPreset : SAttackerSkillPreset
    { 
        protected override string GenerateAssetName()
        {
            return "[PLAYER] " + base.GenerateAssetName();
        }
    }
}
