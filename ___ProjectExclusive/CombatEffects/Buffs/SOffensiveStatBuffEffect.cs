using System;
using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Offensive Buff - N [Preset]",
        menuName = "Combat/Effects/Buff/Offensive Buff")]
    public class SOffensiveStatBuffEffect : SEffectBuffBase
    {
        [SerializeField] private EnumStats.Offensive buffType 
            = EnumStats.Offensive.Attack;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            effectModifier *= user.CombatStats.BuffPower;
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            DoBuff(GetBuff(target), effectModifier);

        }

        protected void DoBuff(ICharacterBasicStats buffStats, float effectModifier)
        {
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
    }
}
