using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team
{
    public interface ICombatTeamProvider
    {
        IReadOnlyCollection<ICombatEntityProvider> GetSelectedCharacters();
    }

    public interface IOppositionTeamStructureRead<out T>
    {
        T PlayerTeamType { get; }
        T EnemyTeamType { get; }
    }

    public interface ITeamFullRolesStructureRead<out T> : ITeamFlexRoleStructureRead<T>, ITeamOffStructureRead<T> 
    { }

    public interface ITeamFlexRoleStructureRead<out T> : ITeamRoleStructureRead<T>
    {
        T FlexType { get; }
    }

    public interface ITeamFlexPositionStructureRead<out T> : ITeamPositionStructureRead<T>
    {
        T FlexLineType { get; }
    }

    public interface ITeamRoleStructureRead<out T>
    {
        T VanguardType { get; }
        T AttackerType { get; }
        T SupportType { get; }
    }

    public interface ITeamPositionStructureRead<out T>
    {
        T FrontLineType { get; }
        T MidLineType { get; }
        T BackLineType { get; }
    }

    public interface IFullStanceStructureRead<out T> : IStanceStructureRead<T>
    {
        T DisruptionStance { get; }
    }

    public interface IStanceStructureRead<out T>
    {
        T AttackingStance { get; }
        T NeutralStance { get; }
        T DefendingStance { get; }
    }

    public interface ITeamAreaDataRead
    {
        EnumTeam.Role RoleType { get; }
        EnumTeam.Positioning PositioningType { get; }
    }


    public interface ITeamController
    {
        void InjectionOnRequestSequence(CombatEntity entity);
        /// <summary>
        /// Request if the controller is ready for [<seealso cref="PerformRequestAction"/>]
        /// </summary>
        IEnumerator<float> _ReadyToRequest(CombatEntity performer);
        void PerformRequestAction(CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target);
        bool HasForcedFinishControlling();
    }

    public interface ITeamEventListener : ICombatEventListener
    {
        void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance);
        void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst);
    }

    public interface IFullRoleAlimentRead<out T>
    {
        T MainRole { get; }
        T SecondaryRole { get; }
        T ThirdRole { get; }
    }
}
