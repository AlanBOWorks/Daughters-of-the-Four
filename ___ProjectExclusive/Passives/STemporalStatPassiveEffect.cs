using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "N (Simple) - Temporal Stats Injection [Effect]",
        menuName = "Combat/Passive/Effect/Temporal Stats Injection")]
    public class STemporalStatPassiveEffect : SPassiveEffectInjection
    {
        [TitleGroup("Params")]
        [InfoBox("If false this is a Base Buff Type", InfoMessageType.Warning)]
        [SerializeField] 
        private bool isBurstType = false;

        [SerializeField] 
        private EnumStats.Combat buffType = EnumStats.Combat.Actions; 

        private const string TemporalStatsPrefix = " - Temporal Stat - ";

        [Button(ButtonSizes.Large)]
        private void UpdateName()
        {
            string isBurst = (isBurstType) ? "BURST " : "BUFF ";
            name =  isBurst + buffType + TemporalStatsPrefix + InjectionNamePrefix;
            UtilsGame.UpdateAssetName(this);
        }

        public override void InjectPassive(CombatingEntity target, float modifier = 1)
        {
            ICharacterFullStats stats;
            if (isBurstType)
                stats = target.CombatStats.BurstStats;
            else
            {
                stats = target.CombatStats.BaseStats;
            }

            switch (buffType)
            {
                case EnumStats.Combat.Health:
                    stats.HealthPoints += modifier;
                    break;
                case EnumStats.Combat.Shield:
                    stats.ShieldAmount += modifier;
                    break;
                case EnumStats.Combat.Mortality:
                    stats.MortalityPoints += modifier;
                    break;
                case EnumStats.Combat.Harmony:
                    stats.HarmonyAmount += modifier;
                    break;
                case EnumStats.Combat.Initiative:
                    UtilsCombatStats.AddInitiative(stats,modifier); //TODO do the Utils for the rest stats
                    break;
                case EnumStats.Combat.Actions:
                    stats.ActionsPerInitiative += Mathf.RoundToInt(modifier);
                    break;
                default:
                    throw new ArgumentOutOfRangeException($"Invalid buff type [{buffType}]",
                        new NotImplementedException("It could be a type that wasn't implemented or" +
                                                    " injected in the wrong handler")); }
        }
    }
}
