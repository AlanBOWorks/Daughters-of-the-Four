using _Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class CharacterCombatAreasData : IStanceProvider
    {
        public CharacterArchetypes.TeamPosition PositionInTeam;
        public CharacterArchetypes.RoleArchetype Role;
        public CharacterArchetypes.FieldPosition CombatFieldPosition;
        public CharacterArchetypes.RangeType RangeType;
        private TeamCombatState _teamStateData;

        private EnumTeam.Stances _forcedState;
        public bool IsForceStance { get; private set; }


        public CharacterCombatAreasData(CharacterArchetypes.TeamPosition positionInTeam,
            CharacterArchetypes.RangeType rangeType,
            CharacterArchetypes.FieldPosition initialFieldPosition = CharacterArchetypes.FieldPosition.InTeam,
            EnumTeam.Stances initialStance = EnumTeam.Stances.Neutral)
        {
            PositionInTeam = positionInTeam;
            Role = (CharacterArchetypes.RoleArchetype) positionInTeam;
            CombatFieldPosition = initialFieldPosition;
            RangeType = rangeType;
            _forcedState = initialStance;
        }

        public void Injection(TeamCombatState teamState)
        {
            _teamStateData = teamState;
        }

        public void ForceState(EnumTeam.Stances targetStance)
        {
            _forcedState = targetStance;
            IsForceStance = true;
        }

        public void ForceStateFinish()
        {
            IsForceStance = false;
        }
        [ShowInInspector]
        public EnumTeam.Stances GetCurrentPositionState()
        {
            if (IsForceStance || _teamStateData == null)
                return _forcedState;
            return _teamStateData.CurrentStance;
        }

        public EnumTeam.Stances CurrentStance => GetCurrentPositionState();
    }
}
