using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Skills
{
    public interface ISkillInteractionStructureRead<out T>
    {
        T SingleType { get; }
        T TargetLine { get; }
        T TargetTeam { get; }
    }

    public interface ISkillArchetypeStructureRead<out T>
    {
        T SelfSkillType { get; }
        T OffensiveSkillType { get; }
        T SupportSkillType { get; }
    }


    public interface ICombatSkill : IFullSkill
    {
        IFullSkill Preset { get; }
        void IncreaseCost();
        void ResetCost();
    }

    public interface IFullSkill : ISkill
    {
        string GetSkillName();
        Sprite GetSkillIcon();

        IEnumerable<PerformEffectValues> GetEffectsFeedBacks();
    }

    public interface ISkill
    {
        int SkillCost { get; }
        EnumsSkill.TeamTargeting TeamTargeting { get; }

        IEffect GetMainEffectArchetype();
        IEnumerable<PerformEffectValues> GetEffects();

        bool IgnoreSelf();

        float LuckModifier { get; }
    }


    public interface ISkillUsageListener : ICombatEventListener
    {
        /// <summary>
        /// Tells what [<see cref="CombatSkill"/>] is used, but before acting out the [<seealso cref="IEffectPreset"/>]s  it has.
        /// Recommendable using this for preparing/modifying the values of the skills or entities (reducing Actions, increasing
        /// cost, react to skills before effects, etc); <br></br>
        /// </summary>
        void OnCombatSkillSubmit(in SkillUsageValues values);
        /// <summary>
        /// Invoked before any [<seealso cref="IEffectPreset"/>] and after all preparations were done;
        /// Its more reliable to read data from here.
        /// </summary>
        void OnCombatSkillPerform(in SkillUsageValues values);
        

        void OnCombatSkillFinish(CombatEntity performer);
    }

    public interface IEffectUsageListener : ICombatEventListener
    {

        void OnCombatPrimaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values);
        /// <summary>
        /// Will be invoked for each [<see cref="IEffectPreset"/>] that the [<see cref="CombatSkill"/>] has;<br></br>
        /// For one call only use [<seealso cref="ISkillUsageListener.OnCombatSkillPerform"/>] better;
        /// </summary>
        void OnCombatSecondaryEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values);

        void OnCombatVanguardEffectPerform(EntityPairInteraction entities, in SubmitEffectValues values);


    }

}
