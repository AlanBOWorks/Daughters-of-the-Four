using CombatEntity;
using CombatSkills;
using Stats;
using UnityEngine;

namespace CombatTeam
{
    public interface ICombatGroupsStructureRead<out T> : ICombatTeamsStructureRead<ITeamRoleStructureRead<T>> { }

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

    public interface IFullTargetingStructure<T> : IFullTargetingStructureRead<T>
    {
        new T SelfElement { get; set; }
        new T SelfTeamElement { get; set; }
        new T SelfAlliesElement { get; set; }
        new T TargetElement { get; set; }
        new T TargetTeamElement { get; set; }
        new T TargetAlliesElement { get; set; }
    }

    public interface IFullTargetingStructureRead<out T> : ITeamTargetingStructureRead<T>
    {
        T TargetElement { get; }
        T TargetTeamElement { get; }
        T TargetAlliesElement { get; }
    }
    public interface ITeamTargetingStructureRead<out T>
    {
        T SelfElement { get; }
        T SelfTeamElement { get; }
        T SelfAlliesElement { get; }
    }



    public interface ITeamProvider : ITeamRoleStructureRead<ICombatEntityProvider>
    {

    }

    public interface ITeamRoleStructureExtended<T> : ITeamRoleStructure<T>, ITeamRoleStructureReadExtended<T>
    {
        new T OthersRole { get; set; }
    }

    public interface ITeamRoleStructure<T> : ITeamRoleStructureRead<T>, ITeamRoleStructureInject<T>
    {
        new T Vanguard { get; set; }
        new T Attacker { get; set; }
        new T Support { get; set; }
    }

    public interface ITeamRoleStructureReadExtended<out T> : ITeamRoleStructureRead<T>
    {
        T OthersRole { get; }
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
        /// <summary>
        /// This is invoked only when the member is death; losing the mortality is not the only
        /// way to trigger this event:<br></br>
        /// <example>
        /// eg - it could be 0 Mortality with some HP; then losing the HP will mean death as well
        /// </example>
        /// </summary>
        void OnMemberDeath(THolder holder, CombatingEntity member);
    }

    public interface IVanguardSpecialization<TMaster, TElement> : ICondensedDominionStructure<TMaster,TElement>
    { }
    public interface IAttackersSpecialization<TMaster, TElement> : ICondensedOffensiveStat<TMaster,TElement> 
    { }
    public interface ISupportSpecialization<TMaster,TElement> : ICondensedSupportStat<TMaster, TElement> 
    { }
}
