using System;
using System.Collections.Generic;
using _Team;
using Characters;
using Sirenix.OdinInspector;

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

        public CombatSkills(CombatingEntity user,
            ISkillShared<SkillPreset> shared, ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            user.Injection(this);
            if (shared == null)
            {
                shared = UtilsSkill.GetOnNullSkills(user.AreasDataTracker.PositionInTeam);
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
            UtilsSkill.DoSafeParse(shared,SumBySkill);

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
                if(check.Preset != null)
                    allCount++;
            }
        }


        private void AddSharedSkills(ISkillShared<SkillPreset> shared)
        {
            if(shared == null)
                throw new ArgumentException("Shared skills are null; BackUp skills weren't invoked still");
            SharedSkills = new SharedCombatSkills(shared);

            UtilsSkill.DoSafeParse(shared, AddShared);
        }

        private void AddUniqueSkills(ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            UtilsSkill.DoParse(uniqueSkills, this, InjectStanceSkills);
        }

        private void AddShared(SkillPreset skill)
        {
            if (skill == null)
                return;
            if (skill.Preset == null)
                return;
            var combatSkill = new CombatSkill(skill);
            AllSkills.Add(combatSkill);
        }

        public void AddSingle(SkillPreset skill)
        {
            if (skill == null)
                throw new NullReferenceException("Skill preset is Null (Value wasn't Serialized)");
            if(skill.Preset == null) 
                throw new NullReferenceException("Skill Preset is  null (Forgotten Serialized reference)");
            AddSkill(skill);
        }

        /// <summary>
        /// Search if the [<seealso cref="skill"/>] is wasn't added and if so Adds it to the
        /// <see cref="AllSkills"/>; Else ignores it.
        /// </summary>
        /// <param name="skill"></param>
        public void AddNotRepeat(SkillPreset skill)
        {
            bool canAdd = true;
            foreach (CombatSkill combatSkill in AllSkills)
            {
                if (combatSkill.Preset != skill.Preset) continue;
                canAdd = false;
                break;
            }
            if(canAdd) AddSkill(skill);
        }

        private void AddSkill(SkillPreset skill)
        {
            var combatSkill = new CombatSkill(skill);
            AllSkills.Add(combatSkill);
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
