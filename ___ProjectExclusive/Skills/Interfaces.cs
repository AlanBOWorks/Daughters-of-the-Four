using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Skills
{
    public interface ISharedSkillsSet<out T> : ISharedSkillsInPosition<T>, ISpecialSkills<T>
    {}

    public interface ISharedSkillsInPosition<out T> : ISkillPositions<ISharedSkills<T>> 
    { }

    public interface ISkillPositions<out T> 
    {
        T AttackingSkills { get; }
        T NeutralSkills { get; }
        T DefendingSkills { get; }
    }

    public interface ISharedSpecialSkills<out T> : ISpecialSkills<T>, ISharedSkills<T> 
    { }
    
    public interface ISpecialSkills<out T> : IUltimateSkill<T>, IWaitSkill<T>
    { }

    public interface IUltimateSkill<out T>
    {
        T UltimateSkill { get; }
    }
    public interface IWaitSkill<out T>
    {
        T WaitSkill { get; }

    }

    public interface ISharedSkills<out T>
    {
        T CommonSkillFirst { get; }
        T CommonSkillSecondary { get; }
    }



    public interface ISkillBackUp<out T>
    {
        ICharacterArchetypesData<T> OnDeathSkill();
    }


}
