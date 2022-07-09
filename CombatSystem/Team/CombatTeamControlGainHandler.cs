using System;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamControlGainHandler : ISkillUsageListener
    {
        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            DoGain(values.Performer);
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
        }

        private const float PerActionControlGain = .1f;

        private static void DoGain(CombatEntity performer)
        {
            var team = performer.Team;
            var teamValues = team.DataValues;

            var teamStance = teamValues.CurrentStance;
            var role = performer.RoleType;
            if(!IsInStanceGain(role,teamStance)) return;

            float gainControlModifier = UtilsStatsFormula.CalculateControlGain(performer.Stats);
            float controlGained = PerActionControlGain * gainControlModifier;

            UtilsCombatTeam.GainControl(team, controlGained);
        }

        private static bool IsInStanceGain(EnumTeam.Role entityRole, EnumTeam.StanceFull stance)
        {
            switch (entityRole)
            {
                case EnumTeam.Role.Vanguard:
                    return stance == EnumTeam.StanceFull.Defending;
                case EnumTeam.Role.Attacker:
                    return stance == EnumTeam.StanceFull.Attacking;
                case EnumTeam.Role.Support:
                    return stance == EnumTeam.StanceFull.Supporting;
                case EnumTeam.Role.Flex:
                    return true;
                default:
                    return false;
            }
        }
    }
}
