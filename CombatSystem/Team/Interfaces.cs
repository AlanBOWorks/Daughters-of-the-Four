using System.Collections.Generic;
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

    public interface IStanceDataRead
    {
        EnumTeam.StanceFull CurrentStance { get; }
    }

    public interface ITeamController
    {
        void Injection(CombatEntity entity);
        IEnumerator<float> _ReadyToRequest(CombatEntity performer);
        void RequestAction(CombatEntity performer, out CombatSkill usedSkill, out CombatEntity target);
        bool HasFinish();
    }
}
