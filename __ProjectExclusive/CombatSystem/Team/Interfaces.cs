using CombatEntity;
using UnityEngine;

namespace CombatTeam
{
    public interface ITeamProvider : ITeamStructureRead<ICombatEntityProvider>
    {

    }

    public interface ITeamStructure<T> : ITeamStructureRead<T>, ITeamStructureInject<T>
    {
        new T Vanguard { get; set; }
        new T Attacker { get; set; }
        new T Support { get; set; }
    }

    public interface ITeamStructureRead<out T>
    {
        T Vanguard { get; }
        T Attacker { get; }
        T Support { get; }
    }
    public interface ITeamStructureInject<in T>
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
