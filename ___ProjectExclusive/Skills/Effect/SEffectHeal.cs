﻿using System;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Heal Effect - N [Preset]",
        menuName = "Combat/Effects/Heal")]
    public class SEffectHeal : SEffectBase
    {
        public override EffectType GetEffectType() => EffectType.Support;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float heal = user.CombatStats.HealPower;
            heal *= effectModifier;
            UtilsSkill.DoHeal(heal,target.CombatStats);

        }
    }
}
 