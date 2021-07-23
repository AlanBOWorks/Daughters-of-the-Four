using System;
using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Offensive Buff - N [Preset]",
        menuName = "Combat/Effects/Offensive Buff")]
    public class SEffectBuffOffensive : SEffectBuffBase
    {
        [SerializeField] private UtilsOffensiveStats.BuffType buffType = UtilsOffensiveStats.BuffType.AttackPower;


        protected override void DoBuff(CombatingEntity user, ICharacterBasicStats buffStats, float effectModifier)
        {
            effectModifier *= user.CombatStats.BuffPower;
            switch (buffType)
            {
                case UtilsOffensiveStats.BuffType.AttackPower:
                    buffStats.AttackPower += effectModifier;
                    break;
                case UtilsOffensiveStats.BuffType.DeBuffPower:
                    buffStats.DeBuffPower += effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }

        }

        protected override string GetToolTip()
        {
            return UtilsOffensiveStats.GetTooltip(buffType);
        }
    }
}
