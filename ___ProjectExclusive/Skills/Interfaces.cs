using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Skills
{
    public interface ISkillPositions<out T> 
    {
        T AttackingSkills { get; }
        T NeutralSkills { get; }
        T DefendingSkills { get; }
    }

    public interface ISkillShared<out T> 
    {
        T UltimateSkill { get; }
        T CommonSkillFirst { get; }
        T CommonSkillSecondary { get; }
    }

    public interface IEquipSkill<T> : ISkillShared<T>
    {
        List<T> UniqueSkills { get; }
    }
}
