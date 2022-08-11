using UnityEngine;

namespace CombatSystem.Skills
{

    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Player Support Preset", order = 12)]
    public class SPlayerSupportSkillPreset : SSupportSkillPreset
    {
        protected override string GenerateAssetName()
        {
            return "[PLAYER] " + base.GenerateAssetName();
        }
    }
}
