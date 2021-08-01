using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatAreasData
    {
        public CharacterArchetypes.TeamPosition PositionInTeam;
        public CharacterArchetypes.RoleArchetype Role;
        public CharacterArchetypes.FieldPosition CombatFieldPosition;
        public CharacterArchetypes.RangeType RangeType;
        private TeamCombatState _teamState;

        private TeamCombatState.Stance _forcedTeamState;
        public bool IsForceStance { get; private set; }


        public CombatAreasData(CharacterArchetypes.TeamPosition positionInTeam,
            CharacterArchetypes.RangeType rangeType,
            CharacterArchetypes.FieldPosition initialFieldPosition = CharacterArchetypes.FieldPosition.InTeam,
            TeamCombatState.Stance initialStance = TeamCombatState.Stance.Neutral)
        {
            PositionInTeam = positionInTeam;
            Role = (CharacterArchetypes.RoleArchetype) positionInTeam;
            CombatFieldPosition = initialFieldPosition;
            RangeType = rangeType;
            _forcedTeamState = initialStance;
        }

        public void Injection(TeamCombatState teamState)
        {
            _teamState = teamState;
        }

        public void ForceState(TeamCombatState.Stance targetStance)
        {
            _forcedTeamState = targetStance;
            IsForceStance = true;
        }

        public void ForceStateFinish()
        {
            IsForceStance = false;
        }
        [ShowInInspector]
        public TeamCombatState.Stance GetCurrentPositionState()
        {
            if (IsForceStance || _teamState == null)
                return _forcedTeamState;
            return _teamState.stance;
        }

    }
}
