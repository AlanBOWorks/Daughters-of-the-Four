﻿using ___ProjectExclusive;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N - REACTION Type - Combat Passive [Preset]",
        menuName = "Combat/Passive/Preset/Reaction Passive Preset")]
    public class SReactionPassiveFilterPreset : SPassiveFilterPreset
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