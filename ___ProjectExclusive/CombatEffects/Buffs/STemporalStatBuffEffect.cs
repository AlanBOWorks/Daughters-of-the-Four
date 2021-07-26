using System;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "N (Simple) - Temporal Stats Injection [Effect]",
        menuName = "Combat/Passive/Effect/Temporal Stats Injection")]
    public class STemporalStatBuffEffect : SEffectBuffBase
    {

        [SerializeField] 
        private EnumStats.Combat buffType = EnumStats.Combat.Actions; 


        public void DoEffect(ICombatTemporalStats stats, float effectModifier)
        {
            switch (buffType)
            {
                case EnumStats.Combat.Health:
                    stats.HealthPoints += effectModifier;
                    break;
                case EnumStats.Combat.Shield:
                    stats.ShieldAmount += effectModifier;
                    break;
                case EnumStats.Combat.Mortality:
                    stats.MortalityPoints += effectModifier;
                    break;
                case EnumStats.Combat.Harmony:
                    stats.HarmonyAmount += effectModifier;
                    break;
                case EnumStats.Combat.Initiative:
                    UtilsCombatStats.AddInitiative(stats, effectModifier); //TODO do the Utils for the rest stats
                    break;
                case EnumStats.Combat.Actions:
                    stats.ActionsPerInitiative += Mathf.RoundToInt(effectModifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid buff type [{buffType}]",
                        new NotImplementedException("It could be a type that wasn't implemented or" +
                                                    " injected in the wrong handler"));
            }
        }

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            ICharacterFullStats stats;
            if (isBurstType)
                stats = target.CombatStats.BurstStats;
            else
            {
                stats = target.CombatStats.BaseStats;
            }
            DoEffect(stats,effectModifier);
        }

        private const string TemporalStatsPrefix = " Temporal Stat - ";
        protected override string GetPrefix()
        {
            return TemporalStatsPrefix + buffType.ToString().ToUpper();
        }
    }
}
