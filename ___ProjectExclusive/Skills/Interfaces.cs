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

    public interface IEquipSkill<T>
    {
        T UltimateSkill { get; }
        T CommonSkillFirst { get; }
        T CommonSkillSecondary { get; }
        List<T> UniqueSkills { get; }
    }
}
