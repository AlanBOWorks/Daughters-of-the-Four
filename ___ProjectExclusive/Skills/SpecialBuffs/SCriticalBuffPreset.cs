using CombatEffects;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "N (T) - [Critical BUFF Skill Preset]",
        menuName = "Combat/Buffs/Critical Buff")]
    public class SCriticalBuffPreset : SDelayBuffPreset
    {
        private const string BuffPresetPrefix = " - [Critical BUFF Preset]";
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + " - " + tickType + BuffPresetPrefix;
        }
    }
}
