using CombatEntity;
using UnityEngine;

namespace CombatTeam
{
    public interface ICombatTeamsStructure<T> : ICombatTeamsStructureRead<T>
    {
        void SetPlayerTeam(T playerTeam);
        void SetEnemyTeam(T enemyTeam);
    }
    public interface ICombatTeamsStructureRead<out T>
    {
        T GetPlayerTeam();
        T GetEnemyTeam();
    }

    public interface ITeamProvider : ITeamRoleStructureRead<ICombatEntityProvider>
    {

    }

    public interface ITeamRoleStructure<T> : ITeamRoleStructureRead<T>, ITeamRoleStructureInject<T>
    {
        new T Vanguard { get; set; }
        new T Attacker { get; set; }
        new T Support { get; set; }
    }

    public interface ITeamRoleStructureRead<out T>
    {
        T Vanguard { get; }
        T Attacker { get; }
        T Support { get; }
    }
    public interface ITeamRoleStructureInject<in T>
    {
        T Vanguard { set; }
        T Attacker { set; }
        T Support { set; }
    }

    public interface ITeamStanceStructure<T> : ITeamStanceStructureRead<T>
    {
        new T OnAttackStance { get; set; }
        new T OnNeutralStance { get; set; }
        new T OnDefenseStance { get; set; }
    }

    public interface ITeamStanceStructureRead<out T>
    {
        T OnAttackStance { get; }
        T OnNeutralStance { get; }
        T OnDefenseStance { get; }
    }


    public interface ITeamStateChangeListener
    {
        void OnStanceChange(EnumTeam.TeamStance switchStance);
        void OnMemberDeath(CombatingEntity member);
    }

    public interface ITeamStateChangeListener<in THolder>
    {
        void OnStanceChange(THolder holder, EnumTeam.TeamStance switchStance);
        void OnMemberDeath(THolder holder, CombatingEntity member);
    }
}
