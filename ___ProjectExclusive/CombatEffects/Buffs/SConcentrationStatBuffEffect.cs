using System;
using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "Concentration Buff - N [ Buff Effect ]",
        menuName = "Combat/Effects/Buff/Concentration Buff")]
    public class SConcentrationStatBuffEffect : SEffectBuffBase
    {
        [SerializeField] private EnumStats.Concentration buffType 
            = EnumStats.Concentration.Critical;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.VariateBuffUser(arguments.UserStats, ref effectModifier);
            DoEffect(target, effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.VariateBuffTarget(target.CombatStats, ref effectModifier);
            DoBuff(GetBuff(target), effectModifier);
            UtilsStats.EnqueueStatsBuffEvent(target);
        }

        private void DoBuff(ICharacterBasicStats buffStats, float effectAmount)
        {
            switch (buffType)
            {
                case EnumStats.Concentration.Enlightenment:
                    buffStats.Enlightenment = buffStats.GetEnlightenment() + effectAmount;
                    break;
                case EnumStats.Concentration.Critical:
                    buffStats.CriticalChance = buffStats.GetCriticalChance() + effectAmount;
                    break;
                case EnumStats.Concentration.Speed:
                    buffStats.SpeedAmount = buffStats.GetSpeedAmount() + effectAmount;
                    break;
                default:
                    throw new ArgumentException($"The buff type [{buffType}] is not valid.",
                        new NotImplementedException($"Could be the type [{buffType}] is not implemented"));
            }
        }

        protected override string GetPrefix()
        {
            return " Concentration Stat - " + buffType.ToString().ToUpper();
        }
    }
}
