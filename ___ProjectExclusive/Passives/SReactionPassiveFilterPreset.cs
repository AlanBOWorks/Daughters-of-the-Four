using ___ProjectExclusive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - REACTION Type - Combat Passive [Preset]",
        menuName = "Combat/Passive/Preset/Reaction Passive",order = -100)]
    public class SReactionPassiveFilterPreset : SPassiveFilterPreset
    {
        private const string ActionPassivePrefix = "REACTION Type - ";
        [Button(ButtonSizes.Large)]
        protected override void UpdateAssetName()
        {
            name = ActionPassivePrefix + PassiveName.ToUpper() + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }
}
