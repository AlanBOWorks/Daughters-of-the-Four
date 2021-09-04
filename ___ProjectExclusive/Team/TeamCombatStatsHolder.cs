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

        [ShowInInspector]
        public readonly PositionalStats PositionalStats;
       

        public float ReviveTime;
        public int BurstControlLength;
        public int BurstCounterAmount;

        

        public TeamCombatStatsHolder(TeamCombatControlHandler controlHandler)
        {
            PositionalStats = new PositionalStats(controlHandler);

            ReviveTime = DefaultReviveTime;
        }

        public const float DefaultReviveTime = 0.5f;
        public TeamCombatStatsHolder(TeamCombatControlHandler controlHandler, ITeamCombatControlHolder stats)
        {
            PositionalStats = new PositionalStats(controlHandler);

            InjectPreset(stats);
        }


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
                UtilsStats.Add(PositionalStats.AttackingStance, attackingStats);
            if (neutralStats != null)
                UtilsStats.Add(PositionalStats.NeutralStance, neutralStats);
            if (defendingStats != null)
                UtilsStats.Add(PositionalStats.DefendingStance, defendingStats);
        }


        public IBasicStatsData<float> AttackingStance => PositionalStats.AttackingStance;
        public IBasicStatsData<float> NeutralStance => PositionalStats.NeutralStance;
        public IBasicStatsData<float> DefendingStance => PositionalStats.DefendingStance;

        public IBasicStatsData<float> GetCurrentStanceValue() 
            => PositionalStats.GetCurrentStanceValue();
    }
}
