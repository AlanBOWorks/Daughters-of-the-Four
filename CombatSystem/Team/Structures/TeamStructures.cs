using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public readonly struct FlexTeamStruct<T>
    {
        public FlexTeamStruct(T vanguard,T attacker,T support,T flex)
        {
            VanguardType = vanguard;
            AttackerType = attacker;
            SupportType = support;
            FlexType = flex;
        }

        [field: HorizontalGroup("FrontLine")]
        public T VanguardType { get; }

        [field: HorizontalGroup("FrontLine")]
        public T AttackerType { get; }

        [field: HorizontalGroup("BackLine")]
        public T SupportType { get; }

        [field: HorizontalGroup("BackLine")]
        public T FlexType { get; }

        public T GetByIndex(int roleIndex)
        {
            return roleIndex switch
            {
                EnumTeam.VanguardIndex => VanguardType,
                EnumTeam.SupportIndex => SupportType,
                EnumTeam.FlexIndex => FlexType,
                _ => AttackerType
            };
        }

        public IEnumerable<T> GetEnumerable()
        {
            yield return VanguardType;
            yield return AttackerType;
            yield return SupportType;
            yield return FlexType;
        }

        public IEnumerable<T> GenerateConcatenation(FlexTeamStruct<T> secondary)
        {
            yield return VanguardType;
            yield return secondary.VanguardType;

            yield return AttackerType;
            yield return secondary.AttackerType;

            yield return SupportType;
            yield return secondary.SupportType;

            yield return FlexType;
            yield return secondary.FlexType;
        }
    }

    public class TeamPosition<T> : ITeamPositionStructureRead<T>
    {
        public TeamPosition(ITeamPositionStructureRead<T> copyFrom)
        {
            FrontLineType = copyFrom.FrontLineType;
            MidLineType = copyFrom.MidLineType;
            BackLineType = copyFrom.BackLineType;
        }
        public T FrontLineType { get; set; }
        public T MidLineType { get; set; }
        public T BackLineType { get; set; }
    }

    [Serializable]
    public class TeamRolesStructure<T> : 
        ITeamFlexStructure<T>, ITeamFlexPositionStructureRead<T>, 
        IMainStatsRead<T>, IEffectTypeStructureRead<T>,
        IStanceStructureRead<T>,
        ISkillArchetypeStructureRead<T>
    {
        [SerializeField, HorizontalGroup("FrontLine")] protected T vanguardType;
        [SerializeField, HorizontalGroup("FrontLine")] protected T attackerType;
        [SerializeField, HorizontalGroup("BackLine")] protected T supportType;
        [SerializeField, HorizontalGroup("BackLine")] protected T flexType;

        public T OffensiveStatType => attackerType;
        public T SupportStatType => supportType;
        public T VitalityStatType => vanguardType;
        public T ConcentrationStatType => flexType;

        public T OffensiveEffectType => attackerType;
        public T SupportEffectType => supportType;
        public T VanguardEffectType => vanguardType;
        public T FlexibleEffectType => ConcentrationStatType;

        public T VanguardType
        {
            get => vanguardType;
            set => vanguardType = value;
        }

        public T AttackerType
        {
            get => attackerType;
            set => attackerType = value;
        }

        public T SupportType
        {
            get => supportType;
            set => supportType = value;
        }

        public T FlexType
        {
            get => flexType;
            set => flexType = value;
        }

        public T FrontLineType => vanguardType;
        public T MidLineType => attackerType;
        public T BackLineType => supportType;
        public T FlexLineType => flexType;

        public T AttackingStance => attackerType;
        public T SupportingStance => supportType;
        public T DefendingStance => vanguardType;


        public T SelfSkillType => vanguardType;
        public T OffensiveSkillType => attackerType;
        public T SupportSkillType => supportType;

        public IEnumerable<T> GetEnumerable()
        {
            yield return vanguardType;
            yield return attackerType;
            yield return supportType;
            yield return flexType;
        }

    }

    [Serializable]
    public class ClassTeamRolesStructure<T> : TeamRolesStructure<T> where T : new()
    {
        public ClassTeamRolesStructure()
        {
            attackerType = new T();
            supportType = new T();
            vanguardType = new T();
            flexType = new T();
        }
    }


    [Serializable]
    public class PreviewTeamRolesStructure<T> :
        ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T>,
        IMainStatsRead<T>, IEffectTypeStructureRead<T>,
        IStanceStructureRead<T>
    {

        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)]
        protected T vanguardType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)]
        protected T attackerType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)]
        protected T supportType;
        [SerializeField, PreviewField, GUIColor(0.3f, 0.3f, 0.3f)]
        protected T flexType;

        public T OffensiveStatType => attackerType;
        public T SupportStatType => supportType;
        public T VitalityStatType => vanguardType;
        public T ConcentrationStatType => flexType;

        public T OffensiveEffectType => attackerType;
        public T SupportEffectType => supportType;
        public T VanguardEffectType => vanguardType;
        public T FlexibleEffectType => flexType;

        public T VanguardType => vanguardType;
        public T AttackerType => attackerType;
        public T SupportType => supportType;
        public T FlexType => flexType;

        public T FrontLineType => vanguardType;
        public T MidLineType => attackerType;
        public T BackLineType => supportType;
        public T FlexLineType => flexType;

        public T AttackingStance => attackerType;
        public T SupportingStance => supportType;
        public T DefendingStance => vanguardType;
    }

    [Serializable]
    public class TeamAlimentStructure<T> : ITeamAlimentStructureRead<T>
    {
        [SerializeField] protected T mainRole;
        [SerializeField] protected T secondaryRole;
        [SerializeField] protected T thirdRole;

        public T MainRole => mainRole;
        public T SecondaryRole => secondaryRole;
        public T ThirdRole => thirdRole;
    }

    [Serializable]
    public class TeamAlimentStructureClass<T> : TeamAlimentStructure<T> where T : new()
    {
        public TeamAlimentStructureClass()
        {
            mainRole = new T();
            secondaryRole = new T();
            thirdRole = new T();
        }
    }

    [Serializable]
    public class TeamAlimentStructureMainOnlyClass<T> : ITeamAlimentStructureRead<T> where T : class
    {
        [SerializeField] protected T mainRole;

        public T MainRole => mainRole;
        public T SecondaryRole => null;
        public T ThirdRole => null;
    }

    [Serializable]
    public class StanceStructure<T> : IStanceStructureRead<T>, ITeamTrinityStructureRead<T>
    {
        [SerializeField] private T defendingType;
        [SerializeField] private T attackingType;
        [SerializeField] private T supportingType;

        public StanceStructure()
        { }
        public StanceStructure(T defendingType, T attackingType, T supportingType)
        {
            this.defendingType = defendingType;
            this.attackingType = attackingType;
            this.supportingType = supportingType;
        }

        public T this[EnumTeam.Stance stance]
        {
            get
            {
                return stance switch
                {
                    EnumTeam.Stance.Defending => defendingType,
                    EnumTeam.Stance.Attacking => attackingType,
                    EnumTeam.Stance.Supporting => supportingType,
                    _ => throw new ArgumentOutOfRangeException(nameof(stance), stance, null)
                };
            }
            set
            {
                switch (stance)
                {
                    case EnumTeam.Stance.Defending:
                        defendingType = value;
                        break;
                    case EnumTeam.Stance.Attacking:
                        attackingType = value;
                        break;
                    case EnumTeam.Stance.Supporting:
                        supportingType = value;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(stance), stance, null);
                }
            }
        }

        public T AttackingStance
        {
            get => attackingType;
            set => attackingType = value;
        }

        public T SupportingStance
        {
            get => supportingType;
            set => supportingType = value;
        }

        public T DefendingStance
        {
            get => defendingType;
            set => defendingType = value;
        }

        public T VanguardType => defendingType;
        public T AttackerType => attackingType;
        public T SupportType => supportingType;
    }
}
