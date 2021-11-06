using System.Collections.Generic;
using CombatEffects;
using UnityEngine;

namespace CombatSkills
{
    public interface ISkill : ISkillDescription, ISkillStats,ISkillEffectParameters
    { }

    public interface ISkillPlayerFeedback
    {
        string GetSkillName();
        Sprite GetIcon();
    }

    public interface ISkillDescription : ISkillPlayerFeedback
    {
        /// <summary>
        /// A Key type for checking special cases (if null it should return the first element of [<see cref="GetEffects"/>]]
        /// </summary>
        ISkillComponent GetDescriptiveEffect();
    }

    public interface ISkillStats
    {
        EnumSkills.TargetType GetTargetType();
        int GetUseCost();
        bool CanCrit();
        float GetCritVariation();
    }
    public interface ISkillEffectParameters
    {
        bool IsMainEffectAfterListEffects { get; }
        EffectParameter GetMainEffect();
        List<EffectParameter> GetEffects();
    }

    public interface ISkillTypes<T> : ISkillTypesRead<T>, ISkillTypesInject<T>
    {
        new T SelfType { get; set; }
        new T OffensiveType { get; set; }
        new T SupportType { get; set; }
    }

    public interface ISkillTypesRead<out T>
    {
        T SelfType { get; }
        T OffensiveType { get; }
        T SupportType { get; }
    }

    public interface ISkillTypesInject<in T>
    {
        T SelfType { set; }
        T OffensiveType { set; }
        T SupportType { set; }
    }

    public interface ISkillGroupTypesRead<out T>
    {
        T SharedSkillTypes { get; }
        T MainSkillTypes { get; }
    }

    public interface ICondensedDominionStructure<TMaster, TElement> : 
        IDominionStructureInject<TElement>, IDominionStructureRead<TElement>
    {
        TMaster Dominion { get; set; }
        new TElement Guard { get; set; }
        new TElement Control { get; set; }
        new TElement Provoke { get; set; }
        new TElement Stance { get; set; }
    }

    public interface IDominionStructureRead<out T>
    {
        T Guard { get; }
        T Control { get; }
        T Provoke { get; }
        T Stance { get; }
    }
    public interface IDominionStructureInject<in T>
    {
        T Guard { set; }
        T Control { set; }
        T Provoke { set; }
        T Stance { set; }
    }
}
