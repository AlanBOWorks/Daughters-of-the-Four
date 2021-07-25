using ___ProjectExclusive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - ACTION Type - Combat Passive [Preset]",
        menuName = "Combat/Passive/Preset/Action Passive Preset")]
    public class SCombatActionPassivePreset : SCombatPassivePreset
    {
        private const string ActionPassivePrefix = " - ACTION Type ";

        [Button(ButtonSizes.Large)]

        protected override void UpdateAssetName()
        {
            name = PassiveName.ToUpper() + ActionPassivePrefix + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
