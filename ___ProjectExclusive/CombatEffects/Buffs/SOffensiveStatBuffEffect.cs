using System;
using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Offensive Buff - N [Preset]",
        menuName = "Combat/Effects/Offensive Buff")]
    public class SOffensiveStatBuffEffect : SEffectBuffBase
    {
        [SerializeField] private EnumStats.Offensive buffType 
            = EnumStats.Offensive.Attack;


        protected void DoBuff(CombatingEntity user, ICharacterBasicStats buffStats, float effectModifier)
        {
            effectModifier *= user.CombatStats.BuffPower;
            switch (buffType)
            {
                case EnumStats.Offensive.Attack:
                    buffStats.AttackPower += effectModifier;
                    break;
                case EnumStats.Offensive.DeBuff:
                    buffStats.DeBuffPower += effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }

        }

        private const string OffensivePrefix = " Offensive Stat - ";
        protected override string GetPrefix()
        {
            return OffensivePrefix + buffType.ToString().ToUpper();
        }

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            DoBuff(user,GetBuff(target),effectModifier);
        }
    }
}
