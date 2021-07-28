using CombatEffects;
using Skills;
using UnityEngine;

namespace Passives
{
    // This is made for organizational purposes and have a clear object type for 'opening passives'
    [CreateAssetMenu(fileName = "N [PASSIVE Opening Preset]",
        menuName = "Combat/Passive/Preset/Opening Passive Preset")]
    public class SOpeningPassivesPreset : SEffectSetPreset
    {
        protected override string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + " - [PASSIVE opening Preset]";
        }
    }
}
