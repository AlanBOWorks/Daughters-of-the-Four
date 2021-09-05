using System;
using ___ProjectExclusive;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Passives
{
    public static class HarmonyHolderConstructor 
    {

        public static IHarmonyHolderBase GenerateHarmonyHolder(CombatingEntity entity,
            SerializedHashsetStats addTo,
            EnumCharacter.RoleArchetype role)
        {
            switch (role)
            {
                case EnumCharacter.RoleArchetype.Vanguard:
                    var vanguardStats = new VanguardHarmony(entity);
                    addTo.Add(vanguardStats);
                    return vanguardStats;
                case EnumCharacter.RoleArchetype.Attacker:
                    var attackerStats = new AttackerHarmony(entity);
                    addTo.Add(attackerStats);
                    return attackerStats;
                case EnumCharacter.RoleArchetype.Support:
                    var supportStats = new SupportHarmony(entity);
                    addTo.Add(supportStats);
                    return supportStats;
                default:
                    throw new ArgumentOutOfRangeException(nameof(role), role, null);
            }
        }

        

        private class VanguardHarmony : IHarmonyHolderBase
        {
            public VanguardHarmony(CombatingEntity entity)
            {
                _harmonyStatsHolder = entity.CombatStats;
            }
            
            private readonly CombatStatsHolder _harmonyStatsHolder;

            [ShowInInspector]
            public float HarmonyLossOnDeath => .3f;

            // Positive Harmony Buff
            [ShowInInspector]
            public float ActionsPerInitiative => 1 * UtilsHarmony.CalculatePositiveModifier(_harmonyStatsHolder);

            // Negative Harmony Buffs 
            [ShowInInspector]
            public float MaxHealth => .2f * UtilsHarmony.NegativePositiveModifier(_harmonyStatsHolder);
            [ShowInInspector]
            public float DamageReduction => .2f * UtilsHarmony.NegativePositiveModifier(_harmonyStatsHolder);

            // Not used
            public float AttackPower => 0;
            public float DeBuffPower => 0;
            public float StaticDamagePower => 0;
            public float HealPower => 0;
            public float BuffPower => 0;
            public float BuffReceivePower => 0;
            public float DisruptionResistance => 0;
            public float CriticalChance => 0;
            public float SpeedAmount => 0;
            public float MaxMortalityPoints => 0;
            public float DeBuffReduction => 0;
            public float HarmonyAmount => 0;
            public float InitiativePercentage => 0;
        }
        private class AttackerHarmony : IHarmonyHolderBase
        {
            public AttackerHarmony(CombatingEntity entity)
            {
                _harmonyStatsHolder = entity.CombatStats;
            }

            private readonly CombatStatsHolder _harmonyStatsHolder;

            [ShowInInspector]
            public float HarmonyLossOnDeath => .4f;

            // Positive Harmony Buff
            [ShowInInspector]
            public float ActionsPerInitiative => 1 * UtilsHarmony.CalculatePositiveModifier(_harmonyStatsHolder);

            [ShowInInspector]
            public float DeBuffPower => .2f * UtilsHarmony.CalculatePositiveModifier(_harmonyStatsHolder);

            // Negative Harmony Buffs
            [ShowInInspector]
            public float AttackPower => .2f * UtilsHarmony.NegativePositiveModifier(_harmonyStatsHolder);
            [ShowInInspector]
            public float StaticDamagePower => .2f * UtilsHarmony.NegativePositiveModifier(_harmonyStatsHolder);

            // Not Used
            public float HealPower => 0;
            public float BuffPower => 0;
            public float BuffReceivePower => 0;
            public float MaxHealth => 0;
            public float MaxMortalityPoints => 0;
            public float DamageReduction => 0;
            public float DeBuffReduction => 0;
            public float DisruptionResistance => 0;
            public float CriticalChance => 0;
            public float SpeedAmount => 0;
            public float HarmonyAmount => 0;
            public float InitiativePercentage => 0;

        }

        private class SupportHarmony : IHarmonyHolderBase
        {
            public SupportHarmony(CombatingEntity entity)
            {
                _harmonyStatsHolder = entity.CombatStats;
            }

            private readonly CombatStatsHolder _harmonyStatsHolder;

            [ShowInInspector]
            public float HarmonyLossOnDeath => .4f;

            // Positive Harmony Buff
            [ShowInInspector]
            public float InitiativePercentage => .1f * UtilsHarmony.CalculatePositiveModifier(_harmonyStatsHolder);
            [ShowInInspector]
            public float BuffPower => .2f * UtilsHarmony.CalculatePositiveModifier(_harmonyStatsHolder);

            // Negative Harmony Buffs
            [ShowInInspector]
            public float HealPower => 1 * UtilsHarmony.NegativePositiveModifier(_harmonyStatsHolder);

            // Not Used
            public float AttackPower => 0;
            public float DeBuffPower => 0;
            public float StaticDamagePower => 0;
            public float MaxHealth => 0;
            public float MaxMortalityPoints => 0;
            public float DamageReduction => 0;
            public float DeBuffReduction => 0;
            public float DisruptionResistance => 0;
            public float CriticalChance => 0;
            public float SpeedAmount => 0;

            public float ActionsPerInitiative => 0;
            public float HarmonyAmount => 0;
            public float BuffReceivePower => 0;

        }
    }



    public static class UtilsHarmony
    {
        public const float PositiveCheck = .5f;
        public const float NegativeCheck = -.3f;
        public const float DangerThreshold = -.6f;
        /// <summary>
        /// Amount of [<see cref="ITemporalStatsData{float}.HarmonyAmount"/>] that the [<seealso cref="CombatingEntity"/>]
        /// loses when being damaged if it's in [<see cref="DangerThreshold"/>]
        /// </summary>
        public const float InDangerHarmonyLossOnHit = .02f;

        public static bool IsInDanger(CombatStatsHolder holder) 
            => holder.HarmonyAmount < DangerThreshold;

        public static float CalculatePositiveModifier(CombatStatsHolder holder)
        {
            return holder.HarmonyAmount < PositiveCheck ? 0 : 1;
        }

        public static float NegativePositiveModifier(CombatStatsHolder holder)
        {
            return holder.HarmonyAmount > NegativeCheck ? 0 : 1;
        }

    }
}
