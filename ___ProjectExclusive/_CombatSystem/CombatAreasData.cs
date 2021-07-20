using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatAreasData
    {
        public CharacterArchetypes.TeamPosition PositionInTeam;
        public CharacterArchetypes.FieldPosition combatFieldPosition;
        public CharacterArchetypes.RangeType RangeType;
        [ShowInInspector]
        internal PositionState positionState;
        private TeamCombatData _teamData;


        public TeamCombatData.Stance PositionStance
        {
            get => positionState.stance;
            set => positionState.stance = value;
        }

        public bool IsForceStance
        {
            get => positionState.IsForcedState;
            set => positionState.IsForcedState = value;
        }

        public CombatAreasData(CharacterArchetypes.TeamPosition positionInTeam,
            CharacterArchetypes.RangeType rangeType,
            CharacterArchetypes.FieldPosition initialFieldPosition = CharacterArchetypes.FieldPosition.InTeam,
            TeamCombatData.Stance initialStance = TeamCombatData.Stance.Neutral)
        {
            PositionInTeam = positionInTeam;
            combatFieldPosition = initialFieldPosition;
            RangeType = rangeType;
            positionState = new PositionState(initialStance,false);
        }

        public void Injection(TeamCombatData teamData)
        {
            _teamData = teamData;
        }

        public void ForceState(TeamCombatData.Stance targetStance)
        {
            positionState.stance = targetStance;
            positionState.IsForcedState = true;
        }

        public void ForceStateFinish()
        {
            positionState.IsForcedState = false;
            positionState.stance = _teamData.stance;
        }

        public TeamCombatData.Stance GetCurrentPositionState()
        {
            if (positionState.IsForcedState || _teamData == null)
                return positionState.stance;
            return _teamData.stance;
        }

        internal struct PositionState
        {
            public TeamCombatData.Stance stance;
            public bool IsForcedState;

            public PositionState(TeamCombatData.Stance target, bool isForcedState)
            {
                stance = target;
                IsForcedState = isForcedState;
            }

        }
    }
}
