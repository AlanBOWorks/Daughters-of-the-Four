using System.Collections.Generic;
using Characters;
using NUnit.Framework;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public interface ICharacterFaction<out T>
    {
        T PlayerFaction { get; }
        T EnemyFaction { get; }
    }

    public interface ICharacterAnimatorHandler
    {
        IEnumerator<float> _DoSkillAnimation(CombatSkill skill, List<CombatingEntity> onTargets);
    }
}
