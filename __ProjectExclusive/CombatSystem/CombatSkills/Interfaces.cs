using CombatEffects;
using UnityEngine;

namespace CombatSkills
{
    public interface ISkill
    {
        string GetSkillName();
        Sprite GetIcon();
        EnumSkills.TargetType GetTargetType();
        int GetCooldownAmount();
        bool CanCrit();
        float GetCritVariation();
        IEffect GetDescriptiveEffect();
        EffectParameter[] GetEffects();
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
