using UnityEngine;

namespace CombatSystem.Skills
{

    [CreateAssetMenu(fileName = "N" + AttackerAssetPrefix,
        menuName = "Combat/Skill/Player Support Preset", order = 12)]
    public class SPlayerSupportSkillPreset : SSupportSkillPreset
    {
        private const string AttackerAssetPrefix = "[Player Support Skill]";
        protected override string GetAssetPrefix() => AttackerAssetPrefix;
    }
}
