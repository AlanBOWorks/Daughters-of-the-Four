using System.Collections.Generic;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Skills
{
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
    }

    public interface IEffect
    {
        void DoEffect(in CombatEntity performer, in CombatEntity target);
    }

    public interface ISkillUsageListener
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

        void OnSkillFinish();
    }
}
