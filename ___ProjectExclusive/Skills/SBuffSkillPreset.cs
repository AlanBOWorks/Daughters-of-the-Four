
using CombatEffects;
using UnityEngine;

namespace Skills
{
    // this is for type check if a skill is primarily a Buff type (and serialization purposes)
    [CreateAssetMenu(fileName = "N (T) - BUFF Skill L - [Preset]",
        menuName = "Combat/Buffs/Buff Preset")]
    public class SBuffSkillPreset : SSkillPresetBase
    {
        private const string BuffPresetPrefix = " - [Buff SKILL Preset]";
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + BuffPresetPrefix;
        }
    }
}
