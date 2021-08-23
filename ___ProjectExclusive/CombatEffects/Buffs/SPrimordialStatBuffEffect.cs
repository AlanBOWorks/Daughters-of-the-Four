using System;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Buff PRIMORDIAL - N [ Buff Effect ]",
        menuName = "Combat/Effects/Buff/_Primordial Buff", order = -100)]
    public class SPrimordialStatBuffEffect : SEffectBase
    {
        [SerializeField] 
        private EnumStats.Primordial buffStat = EnumStats.Primordial.Offensive;

        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float effectModifier = 1)
        {
            UtilsCombatStats.VariateBuffUser(arguments.UserStats,ref effectModifier);
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            var targetStats = target.CombatStats;
            UtilsCombatStats.VariateBuffTarget(targetStats,ref effectModifier);
            DoBuff(targetStats.GetMultiplierStats(),effectModifier);
            UtilsStats.EnqueueStatsBuffEvent(target);
        }

        private void DoBuff(IStatsPrimordial multiplierStats, float effectModifier)
        {
            switch (buffStat)
            {
                case EnumStats.Primordial.Offensive:
                    multiplierStats.OffensivePower += effectModifier;
                    break;
                case EnumStats.Primordial.Support:
                    multiplierStats.SupportPower += effectModifier;
                    break;
                case EnumStats.Primordial.Vitality:
                    multiplierStats.VitalityAmount += effectModifier;
                    break;
                case EnumStats.Primordial.Concentration:
                    multiplierStats.ConcentrationAmount += effectModifier;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(buffStat) + " isn't implemented; Either the argument" +
                                                          "wasn't inserted with the correct value or this method is unfinish");
            }
        }


        [Button(ButtonSizes.Large), GUIColor(.3f, .6f, 1f)]
        private void UpdateAssetName()
        {
            name = $"Buff _PRIMORDIAL - {buffStat.ToString().ToUpper()} [Buff Effect]";
            UtilsGame.UpdateAssetName(this);
        }

    }
}
