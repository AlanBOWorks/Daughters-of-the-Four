using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Enemy Support Preset", order = 102)]
    public class SEnemySupportSkillPreset : SSupportSkillPreset
    {
        protected override string GenerateAssetName()
        {
            return "[ENEMY] " + base.GenerateAssetName();
        }
    }
}
