using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Skills;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Enemies
{
    public abstract class SCombatEnemyController : ScriptableObject, ICombatEnemyController
    {
        public abstract void DoControlOn(CombatingEntity entity);
    }

    public class CombatEnemyControllerRandom : ICombatEnemyController
    {
        public static CombatEnemyControllerRandom GenericEnemyController = new CombatEnemyControllerRandom();

        [ShowInInspector]
        private readonly List<CombatSkill> _possibleSkills;

        public CombatEnemyControllerRandom()
        {
            _possibleSkills = new List<CombatSkill>(4); //4: ultimate + 2 commons + at least 1 unique
        }

        private const float UseWaitChance = .1f;
        public void DoControlOn(CombatingEntity entity)
        {
            ISkillShared<CombatSkill> skillShared = entity.SharedSkills;
            
            var ultimate = skillShared.UltimateSkill;
            if (ultimate != null && ultimate.Preset != null && !ultimate.IsInCooldown())
            {
                DoSkill(ultimate);
                return;
            }

            var waitSkill = skillShared.WaitSkill;

            if (Random.value < UseWaitChance)
            {
                DoSkill(waitSkill);
                return;
            }


            _possibleSkills.Clear();
            UtilsEnemyController.DoInjectionOfSkills(_possibleSkills,entity);

            DoRandomSelection(waitSkill);
        }



        private static void DoSkill(CombatSkill skill)
        {
            if(skill == null)
                throw new ArgumentException("Skill can't be empty", 
                    new NullReferenceException("The selector of skill had failed in choosing a Skill"));

#if UNITY_EDITOR
            Debug.Log($"[[ AI ]]] Using Skill: {skill.SkillName}");
#endif

            List<CombatingEntity> possibleTargets
                = PerformSkillHandler.SendHandlePossibleTargets(skill);
            int randomSelection = Random.Range(0, possibleTargets.Count);
            CombatingEntity selection = possibleTargets[randomSelection];
            PerformSkillHandler.SendDoSkill(selection);
        }

        private void DoRandomSelection(CombatSkill backupSkillOnZero)
        {
            if (_possibleSkills.Count <= 0)
            {
                DoSkill(backupSkillOnZero);
                return;
            }

            int randomSelection = Random.Range(0, _possibleSkills.Count);
            CombatSkill selectedSkill = _possibleSkills[randomSelection];
            DoSkill(selectedSkill);
        }
    }

    public static class UtilsEnemyController
    {
        public static void DoInjectionOfSkills(List<CombatSkill> injectInto, CombatingEntity user)
        {
            ISkillShared<CombatSkill> skillShared = user.SharedSkills;
            AddSharedType(skillShared.CommonSkillFirst);
            AddSharedType(skillShared.CommonSkillSecondary);

            List<CombatSkill> uniqueSkills = UtilsSkill.GetUniqueByStance(user);
            if (uniqueSkills == null) return;

            foreach (CombatSkill skill in uniqueSkills)
            {
                if (!skill.CanBeUse(user)) return;
                injectInto.Add(skill);
            }

            void AddSharedType(CombatSkill skill)
            {
                if (!skill.CanBeUse(user)) return;
                injectInto.Add(skill);
            }

        }

        public static void DoInjectionOfUltimate(List<CombatSkill> possibleSkills, CombatingEntity entity)
        {
            ISkillShared<CombatSkill> skillShared = entity.SharedSkills;
            var ultimate = skillShared.UltimateSkill;
            if (ultimate != null && !ultimate.IsInCooldown())
            {
                possibleSkills.Add(ultimate);
            }
        }

        public static void DoInjectionOfWait(List<CombatSkill> possibleSkills, CombatingEntity entity)
        {
            ISkillShared<CombatSkill> skillShared = entity.SharedSkills;
            possibleSkills.Add(skillShared.WaitSkill);
        }
    }

    public interface ICombatEnemyController
    {
        void DoControlOn(CombatingEntity entity);
    }
}
