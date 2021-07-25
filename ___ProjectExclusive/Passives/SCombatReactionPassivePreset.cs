using ___ProjectExclusive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - Combat Reaction Passive - [Preset]",
        menuName = "Combat/Passive/Combat Reaction Passive Preset")]
    public class SCombatReactionPassivePreset : SCombatPassivePreset
    {
        private const string ActionPassivePrefix = " - REACTION Type ";
        [Button(ButtonSizes.Large)]
        protected override void UpdateAssetName()
        {
            name = PassiveName.ToUpper() + ActionPassivePrefix + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
