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
    }

    public interface ISkill
    {
        int SkillCost { get; }
        EnumsSkill.Archetype Archetype { get; }
        EnumsSkill.TargetType TargetType { get; }

        void DoSkill(in CombatEntity performer, in CombatEntity target, in CombatSkill holderReference);

        bool IgnoreSelf();
        IEnumerable<IEffect> GetEffects();
    }

    public interface IEffect
    {
        SEffect GetPreset();
        EnumsEffect.TargetType TargetType { get; }
        void DoEffect(in CombatEntity performer);
    }

    public interface ISkillUsageListener : ICombatEventListener
    {
        /// <summary>
        /// Tells what [<see cref="CombatSkill"/>] is used, but before acting out the [<seealso cref="IEffect"/>]s  it has.
        /// Recommendable using this for preparing/modifying the values of the skills or entities (reducing Actions, increasing
        /// cost, react to skills before effects, etc); <br></br>
        /// After [<seealso cref="OnEffectPerform"/>] will be invoked and [<seealso cref="IEffect"/>] will be invoked.
        /// </summary>
        void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target);
        /// <summary>
        /// Invoked before any [<seealso cref="IEffect"/>] and after all preparations were done;
        /// Its more reliable to read data from here.
        /// </summary>
        void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target);
        /// <summary>
        /// Will be invoked for each [<see cref="IEffect"/>] that the [<see cref="CombatSkill"/>] has;<br></br>
        /// For one call only use [<seealso cref="OnSkillPerform"/>] better;
        /// </summary>
        void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect);

        void OnSkillFinish(in CombatEntity performer);
    }
}
