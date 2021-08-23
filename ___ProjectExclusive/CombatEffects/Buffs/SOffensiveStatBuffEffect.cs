using System;
using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Offensive Buff - N [ Buff Effect ]",
        menuName = "Combat/Effects/Buff/Offensive Buff")]
    public class SOffensiveStatBuffEffect : SEffectBuffBase
    {
        [SerializeField] private EnumStats.Offensive buffType 
            = EnumStats.Offensive.Attack;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.VariateBuffUser(arguments.UserStats,ref effectModifier);
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.VariateBuffTarget(target.CombatStats,ref effectModifier);
            DoBuff(GetBuff(target), effectModifier);
            UtilsStats.EnqueueStatsBuffEvent(target);
        }

        protected void DoBuff(ICharacterBasicStats buffStats, float effectModifier)
        {
            switch (buffType)
            {
                case EnumStats.Offensive.Attack:
                    buffStats.AttackPower = buffStats.GetAttackPower() + effectModifier;
                    break;
                case EnumStats.Offensive.DeBuff:
                    buffStats.DeBuffPower = buffStats.GetDeBuffPower() + effectModifier;
                    break;
                case EnumStats.Offensive.StaticPower:
                    buffStats.StaticDamagePower = buffStats.GetStaticDamagePower() + effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }
        }


        protected override string GetPrefix()
        {
            return " Offensive Stat - " + buffType.ToString().ToUpper();
        }
    }
}
