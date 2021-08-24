using System;
using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Vitality Buff - N [ Buff Effect ]",
        menuName = "Combat/Effects/Buff/Vitality Buff")]
    public class SVitalityStatBuffEffect : SEffectBuffBase
    {
        [SerializeField] private EnumStats.Vitality buffType 
            = EnumStats.Vitality.DamageReduction;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.VariateBuffUser(arguments.UserStats,ref effectModifier);
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.VariateBuffTarget(target.CombatStats,ref effectModifier);
            DoBuff(GetBuff(target),effectModifier);
            UtilsStats.EnqueueStatsBuffEvent(target);
        }

        private void DoBuff(IBasicStats buffStats, float effectAmount)
        {
            switch (buffType)
            {
                case EnumStats.Vitality.MaxHealth:
                    buffStats.MaxHealth = buffStats.GetMaxHealth() + effectAmount;
                    break;
                case EnumStats.Vitality.MaxMortality:
                    buffStats.MaxMortalityPoints = buffStats.GetMaxMortalityPoints() + effectAmount;
                    break;
                case EnumStats.Vitality.DamageReduction:
                    buffStats.DamageReduction = buffStats.GetDamageReduction() + effectAmount;
                    break;
                case EnumStats.Vitality.DeBuffReduction:
                    buffStats.DeBuffReduction = buffStats.GetDeBuffReduction() + effectAmount;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }
        }

        protected override string GetPrefix()
        {
            return " Vitality Stat - " + buffType.ToString().ToUpper();
        }
    }
}
