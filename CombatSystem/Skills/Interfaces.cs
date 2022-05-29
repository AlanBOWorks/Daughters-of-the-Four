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
        T SelfType { get; }
        T OffensiveType { get; }
        T SupportType { get; }
    }


    public interface IFullSkill : ISkill
    {
        string GetSkillName();
        Sprite GetSkillIcon();

        IEnumerable<PerformEffectValues> GetEffects();
    }

    public interface ISkill
    {
        int SkillCost { get; }
        EnumsSkill.Archetype Archetype { get; }
        EnumsSkill.TargetType TargetType { get; }

        IEffect GetMainEffectArchetype();

        bool IgnoreSelf();
    }


    public interface ISkillUsageListener : ICombatEventListener
    {
        /// <summary>
        /// Tells what [<see cref="CombatSkill"/>] is used, but before acting out the [<seealso cref="IEffectPreset"/>]s  it has.
        /// Recommendable using this for preparing/modifying the values of the skills or entities (reducing Actions, increasing
        /// cost, react to skills before effects, etc); <br></br>
        /// After [<seealso cref="OnCombatEffectPerform"/>] will be invoked and [<seealso cref="IEffectPreset"/>] will be invoked.
        /// </summary>
        void OnCombatSkillSubmit(in SkillUsageValues values);
        /// <summary>
        /// Invoked before any [<seealso cref="IEffectPreset"/>] and after all preparations were done;
        /// Its more reliable to read data from here.
        /// </summary>
        void OnCombatSkillPerform(in SkillUsageValues values);
        /// <summary>
        /// Will be invoked for each [<see cref="IEffectPreset"/>] that the [<see cref="CombatSkill"/>] has;<br></br>
        /// For one call only use [<seealso cref="OnCombatSkillPerform"/>] better;
        /// </summary>
        void OnCombatEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values);

        void OnCombatSkillFinish(CombatEntity performer);
    }

}
