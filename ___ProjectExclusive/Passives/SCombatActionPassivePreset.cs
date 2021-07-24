using ___ProjectExclusive;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - Combat Action Passive - [Preset]",
        menuName = "Combat/Passive/Combat Action Passive Preset")]
    public class SCombatActionPassivePreset : SCombatPassivePreset
    {
        private const string ActionPassivePrefix = " - ACTION Type ";
        protected override void OnValidate()
        {
            name = PassiveName.ToUpper() + ActionPassivePrefix + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
