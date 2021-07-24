using ___ProjectExclusive;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - Combat Reaction Passive - [Preset]",
        menuName = "Combat/Passive/Combat Reaction Passive Preset")]
    public class SCombatReactionPassivePreset : SCombatPassivePreset
    {
        private const string ActionPassivePrefix = " - REACTION Type ";
        protected override void OnValidate()
        {
            name = PassiveName.ToUpper() + ActionPassivePrefix + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
