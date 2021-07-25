using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{

    public class CombatSkills : ISkillPositions<List<CombatSkill>>,
        IStanceArchetype<List<CombatSkill>>
    {
        [ShowInInspector]
        public List<CombatSkill> AllSkills { get; private set; }
        [ShowInInspector]
        public IEquipSkill<CombatSkill> SharedSkills { get; private set; }
        public ISkillPositions<List<CombatSkill>> UniqueSkills => this;
        [ShowInInspector]
        public List<CombatSkill> AttackingSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> NeutralSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> DefendingSkills { get; }


        public List<CombatSkill> GetAttacking() => AttackingSkills;
        public List<CombatSkill> GetNeutral() => NeutralSkills;
        public List<CombatSkill> GetDefending() => DefendingSkills;

        public CombatSkills(CombatingEntity user,ISkillShared<SkillPreset> shared, ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            user.Injection(this);
            if (shared == null)
            {
                shared = UtilsSkill.GetBackUpSkills(user.AreasDataTracker.PositionInTeam);
            }

            int attackingCount;
            int neutralCount;
            int defendingCount;

            bool isUniqueNull = uniqueSkills == null;
            if (isUniqueNull)
            {
                attackingCount = 0;
                neutralCount = 0;
                defendingCount = 0;
            }
            else
            {
                attackingCount = GetSkillsCount(uniqueSkills.AttackingSkills);
                neutralCount = GetSkillsCount(uniqueSkills.NeutralSkills);
                defendingCount = GetSkillsCount(uniqueSkills.DefendingSkills);
            }

            

            int allCount = attackingCount + neutralCount + defendingCount;
            UtilsSkill.DoParse(shared,SumBySkill);

            AllSkills = new List<CombatSkill>(allCount);
            AttackingSkills = new List<CombatSkill>(attackingCount);
            NeutralSkills = new List<CombatSkill>(neutralCount);
            DefendingSkills = new List<CombatSkill>(defendingCount);

            AddSharedSkills(shared);
            if(!isUniqueNull)
                AddUniqueSkills(uniqueSkills);

            int GetSkillsCount(List<SkillPreset> skills)
            {
                return skills?.Count ?? 0;
            }

            void SumBySkill(SkillPreset check)
            {
                if (check != null && check.Preset != null)
                    allCount++;
            }
        }


        private void AddSharedSkills(ISkillShared<SkillPreset> shared)
        {
            if(shared == null)
                throw new ArgumentException("Shared skills are null; BackUp skills weren't invoked still");
            SharedSkills = new SharedCombatSkills(shared);

            UtilsSkill.DoParse(shared, DoInjection);
            void DoInjection(SkillPreset skill)
            {
                if (skill == null || skill.Preset == null) return;
                var combatSkill = new CombatSkill(skill);
                AllSkills.Add(combatSkill);
            }
        }

        private void AddUniqueSkills(ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            UtilsSkill.DoParse(uniqueSkills, this, InjectStanceSkills);

        }

        private void InjectStanceSkills(List<SkillPreset> injectedSkills, List<CombatSkill> onPositionCombatSkills)
        {
            if(injectedSkills == null) return;
            foreach (SkillPreset skill in injectedSkills)
            {
                var combatSkill = new CombatSkill(skill);
                onPositionCombatSkills.Add(combatSkill);
                AllSkills.Add(combatSkill);
            }
        }

    }
}
