using System;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using Stats;
using UnityEngine;

namespace _Team
{
    public class TeamCombatStatsHolder : ITeamCombatControlStats, 
        IStanceData<IBasicStatsData<float>>
    {
        public TeamCombatStatsHolder(CombatTeamControl control)
        {
            _teamPositionalStats = new PositionalStats(control);
            _controlStats = new TeamControlStats(control);
            ReviveTime = DefaultReviveTime;
        }
        public TeamCombatStatsHolder(CombatTeamControl control, ITeamCombatControlHolder stats)
        : this(control)
        {
            InjectPreset(stats);
        }

        [ShowInInspector] 
        private readonly PositionalStats _teamPositionalStats;
        private readonly TeamControlStats _controlStats;

        public float ReviveTime;
        public int BurstControlLength;
        public int BurstCounterAmount;
        public const float DefaultReviveTime = 0.5f;




        public void InjectPreset(ITeamCombatControlHolder holder)
        {
            InjectNewStats(holder);
       
            ReviveTime = holder.GetReviveTime();

            BurstControlLength = holder.GetBurstControlLength();
            BurstCounterAmount = holder.GetBurstCounterAmount();
        }

        private void InjectNewStats(IStanceData<IBasicStats<float>> stats)
        {
            var attackingStats = stats.AttackingStance;
            var neutralStats = stats.NeutralStance;
            var defendingStats = stats.DefendingStance;
            
            if(attackingStats != null)
                UtilsStats.Add(_teamPositionalStats.AttackingStance, attackingStats);
            if (neutralStats != null)
                UtilsStats.Add(_teamPositionalStats.NeutralStance, neutralStats);
            if (defendingStats != null)
                UtilsStats.Add(_teamPositionalStats.DefendingStance, defendingStats);
        }

        public void InjectOn(CombatingTeam team)
        {
            foreach (CombatingEntity entity in team)
            {
                InjectOn(entity.CombatStats);
            }

            void InjectOn(CombatStatsHolder stats)
            {
                var baseStats = stats.GetFormulatedStats().GetBase();
                baseStats.Add(_teamPositionalStats);
                baseStats.Add(_controlStats);
            }
        }

        public IBasicStatsData<float> AttackingStance => _teamPositionalStats.AttackingStance;
        public IBasicStatsData<float> NeutralStance => _teamPositionalStats.NeutralStance;
        public IBasicStatsData<float> DefendingStance => _teamPositionalStats.DefendingStance;

        public IBasicStatsData<float> GetCurrentStanceValue() 
            => _teamPositionalStats.GetCurrentStanceValue();

        private class TeamControlStats : IBasicStatsData<float>
        {
            public TeamControlStats(CombatTeamControl control)
            {
                _teamControl = control;
            }

            private readonly CombatTeamControl _teamControl;

            private const float LowerTierCheck = .5f;
            private const float HighTierCheck = .8f;

            private float GetLowTierModifier()
            {
                return _teamControl.GetControlAmount() < LowerTierCheck ? 0 : 1;
            }
            private float GetHighTierModifier()
            {
                return _teamControl.GetControlAmount() < HighTierCheck ? 0 : 1;
            }

            // Low Tier Check
            [ShowInInspector]
            public float DisruptionResistance => 10 * GetLowTierModifier();
            [ShowInInspector]
            public float InitiativePercentage => .1f * GetLowTierModifier();

            // High Tier Check
            [ShowInInspector]
            public float ActionsPerInitiative => 1 * GetHighTierModifier();
            [ShowInInspector]
            public float HarmonyAmount => 1 * GetHighTierModifier();



            // Not used
            public float AttackPower => 0;
            public float DeBuffPower => 0;
            public float StaticDamagePower => 0;
            public float HealPower => 0;
            public float BuffPower => 0;
            public float BuffReceivePower => 0;
            public float MaxHealth => 0;
            public float MaxMortalityPoints => 0;
            public float DamageReduction => 0;
            public float DeBuffReduction => 0;
            public float CriticalChance => 0;
            public float SpeedAmount => 0;
        }
    }
}
