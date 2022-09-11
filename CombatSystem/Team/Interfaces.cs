using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;

namespace CombatSystem.Team
{
    public interface ICombatTeamProvider
    {
        IEnumerable<ICombatEntityProvider> GetSelectedCharacters();
        int MembersCount { get; }
    }

    public interface IOppositionTeamStructureRead<out T>
    {
        T PlayerTeamType { get; }
        T EnemyTeamType { get; }
    }

    public interface ITeamFlexStructure<T> : ITeamFlexStructureRead<T>, ITeamFlexStructureInject<T>
    {
        new T VanguardType { get; set; }
        new T AttackerType { get; set; }
        new T SupportType { get; set; }
        new T FlexType { get; set; }
    }
    public interface ITeamOffStructureRead<out T>
    {
        T SecondaryVanguardElement { get; }
        T SecondaryAttackerElement { get; }
        T SecondarySupportElement { get; }
        T SecondaryFlexElement { get; }

        T ThirdVanguardElement { get; }
        T ThirdAttackerElement { get; }
        T ThirdSupportElement { get; }
        T ThirdFlexElement { get; }
    }


    public interface ITeamFullStructureRead<out T> : ITeamFlexStructureRead<T>, ITeamOffStructureRead<T> 
    { }


    public interface ITeamFlexStructureInject<in T> : ITeamTrinityStructureInject<T>
    {
        T FlexType { set; }

    }
    public interface ITeamFlexStructureRead<out T> : ITeamTrinityStructureRead<T>
    {
        T FlexType { get; }
    }

    public interface ITeamFlexPositionStructureRead<out T> : ITeamPositionStructureRead<T>
    {
        T FlexLineType { get; }
    }

    public interface ITeamTrinityStructureRead<out T>
    {
        T VanguardType { get; }
        T AttackerType { get; }
        T SupportType { get; }
    }
    public interface ITeamTrinityStructureInject<in T>
    {
        T VanguardType { set; }
        T AttackerType { set; }
        T SupportType { set; }
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
        T SupportingStance { get; }
        T DefendingStance { get; }
    }

    public interface ITeamAreaDataRead
    {
        EnumTeam.Role RoleType { get; }
        EnumTeam.Positioning PositioningType { get; }
    }
    
    public interface ITeamEventListener : ICombatEventListener
    {
        void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance, bool isControlChange);
        void OnControlChange(CombatTeam team, float phasedControl);
    }

    public interface ITeamAlimentStructureRead<out T>
    {
        T MainRole { get; }
        T SecondaryRole { get; }
        T ThirdRole { get; }
    }
}
