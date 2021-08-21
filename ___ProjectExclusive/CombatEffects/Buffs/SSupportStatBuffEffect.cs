using System;
using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Support Buff - N [Preset]",
        menuName = "Combat/Effects/Buff/Support Buff")]
    public class SSupportStatBuffEffect : SEffectBuffBase
    {
        [SerializeField]
        private EnumStats.Support buffType
            = EnumStats.Support.Heal;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.VariateBuffUser(arguments.UserStats,ref effectModifier);
            DoEffect(target, effectModifier);
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
                case EnumStats.Support.Heal:
                    buffStats.HealPower = buffStats.GetHealPower() + effectModifier;
                    break;
                case EnumStats.Support.Buff:
                    buffStats.BuffPower = buffStats.GetBuffPower() + effectModifier;
                    break;
                case EnumStats.Support.ReceiveBuffIndex:
                    buffStats.BuffReceivePower = buffStats.GetBuffReceivePower() + effectModifier;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }
        }



        private const string SupportPrefix = " Support Stat - ";
        protected override string GetPrefix()
        {
            return SupportPrefix + buffType.ToString().ToUpper();
        }
    }
}
