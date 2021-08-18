using ___ProjectExclusive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - ACTION Type - Combat Passive [Preset]",
        menuName = "Combat/Passive/Preset/Action Passive",order = -100)]
    public class SActionPassiveFilterPreset : SPassiveFilterPreset
    {
        private const string ActionPassivePrefix = "ACTION Type - ";

        [Button(ButtonSizes.Large)]

        protected override void UpdateAssetName()
        {
            name = ActionPassivePrefix + PassiveName.ToUpper() + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
