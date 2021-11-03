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
        int GetCooldownAmount();
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
}
