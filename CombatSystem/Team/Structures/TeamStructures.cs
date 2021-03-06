using System;
using System.Collections.Generic;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
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
        ITeamFlexStructureRead<T>, ITeamFlexPositionStructureRead<T>, 
        IMainStatsRead<T>, IEffectTypeStructureRead<T>,
        IStanceStructureRead<T>,
        ISkillArchetypeStructureRead<T>
    {
        [SerializeField] protected T vanguardType;
        [SerializeField] protected T attackerType;
        [SerializeField] protected T supportType;
        [SerializeField] protected T flexType;

        public T OffensiveStatType => attackerType;
        public T SupportStatType => supportType;
        public T VitalityStatType => vanguardType;
        public T ConcentrationStatType => flexType;

        public T OffensiveEffectType => attackerType;
        public T SupportEffectType => supportType;
        public T VanguardEffectType => vanguardType;
        public T FlexibleEffectType => ConcentrationStatType;

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
}
