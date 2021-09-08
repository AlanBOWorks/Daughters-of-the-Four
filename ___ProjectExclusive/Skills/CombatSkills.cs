using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Characters;
using Sirenix.OdinInspector;

namespace Skills
{

    public class CombatSkills : ISkillPositions<List<CombatSkill>>,
        IStanceData<List<CombatSkill>>, ISpecialSkills<CombatSkill>
    {
        private readonly CombatingEntity _user;

        [ShowInInspector]
        public List<CombatSkill> AllSkills { get; private set; }

        [ShowInInspector] 
        private readonly CombatSharedSkills _sharedSkills;
        public ISharedSkillsSet<CombatSkill> SharedSkills => _sharedSkills;
        public ISkillPositions<List<CombatSkill>> UniqueSkills => this;
        [ShowInInspector]
        public List<CombatSkill> AttackingSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> NeutralSkills { get; }
        [ShowInInspector]
        public List<CombatSkill> DefendingSkills { get; }

        public List<CombatSkill> AttackingStance => AttackingSkills;
        public List<CombatSkill> NeutralStance => NeutralSkills;
        public List<CombatSkill> DefendingStance => DefendingSkills;


        public CombatSkill UltimateSkill => SharedSkills.UltimateSkill;
        public CombatSkill WaitSkill => SharedSkills.WaitSkill;

        public ISharedSkills<CombatSkill> GetCurrentSharedSkills()
            => UtilsSkill.GetElement(SharedSkills, _user);


        public CombatSkills(CombatingEntity user, ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            _user = user;
            user.Injection(this);
            

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

            AllSkills = new List<CombatSkill>(allCount);
            AttackingSkills = new List<CombatSkill>(attackingCount);
            NeutralSkills = new List<CombatSkill>(neutralCount);
            DefendingSkills = new List<CombatSkill>(defendingCount);
            _sharedSkills = new CombatSharedSkills();

            if(!isUniqueNull)
                AddUniqueSkills(uniqueSkills);

            AddWait();


            int GetSkillsCount(List<SkillPreset> skills)
            {
                return skills?.Count ?? 0;
            }

            void AddWait()
            {
                SkillPreset waitPreset = CombatSystemSingleton.ParamsVariable.GetWaitSkillPreset();
                CombatSkill waitSkill = new CombatSkill(waitPreset);
                AllSkills.Add(waitSkill);
                _sharedSkills.WaitSkill = waitSkill;
            }
        }

        public void Initialization()
        {
            EnumCharacter.RoleArchetype entityRole = _user.Role;
            EnumCharacter.RangeType entityRange = _user.AreasDataTracker.RangeType;

            var sharedSkills = UtilsSkill.GetSharedSkillPreset(entityRole, entityRange);
            InjectGenerateSharedSkills(sharedSkills);
        }

        private void InjectGenerateSharedSkills(ISharedSkillsInPosition<SkillPreset> shared)
        {
            if(shared == null)
                throw new ArgumentException("Shared skills are null; BackUp skills weren't invoked still");
            _sharedSkills.GenerateShared(shared,AllSkills);
        }
        public void AddGenerateUltimate(SkillPreset ultimate)
        {
            var ultimateSkill = new CombatSkill(ultimate,true);
            AllSkills.Add(ultimateSkill);
        }

        private void AddUniqueSkills(ISkillPositions<List<SkillPreset>> uniqueSkills)
        {
            UtilsSkill.DoParse(uniqueSkills, this, InjectStanceSkills);
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

        public void ReduceCooldown()
        {
            foreach (var skill in AllSkills)
            {
                skill.OnCharacterAction();
            }
        }

        public void CheckAndResetIdle()
        {
            foreach (var skill in AllSkills)
            {
                skill.OnCharacterFinish();
            }
        }

        private class CombatSharedSkills : ISharedSkillsSet<CombatSkill>
        {
            public ISharedSkills<CombatSkill> AttackingSkills { get; private set; }
            public ISharedSkills<CombatSkill> NeutralSkills { get; private set; }
            public ISharedSkills<CombatSkill> DefendingSkills { get; private set; }
            public CombatSkill UltimateSkill { get; set; } = null;
            public CombatSkill WaitSkill { get; set; } = null;

            public void GenerateShared(ISharedSkillsInPosition<SkillPreset> sharedSkills, List<CombatSkill> addTo)
            {
                AttackingSkills = new SharedCombatSkills(sharedSkills.AttackingSkills);
                NeutralSkills = new SharedCombatSkills(sharedSkills.NeutralSkills);
                DefendingSkills = new SharedCombatSkills(sharedSkills.DefendingSkills);

                UtilsSkill.AddTo(this,addTo);
            }

            private class SharedCombatSkills : ISharedSkills<CombatSkill>
            {
                public SharedCombatSkills(ISharedSkills<SkillPreset> preset)
                {
                    CommonSkillFirst = new CombatSkill(preset.CommonSkillFirst);
                    CommonSkillSecondary = new CombatSkill(preset.CommonSkillSecondary);
                }

                public CombatSkill CommonSkillFirst { get; }
                public CombatSkill CommonSkillSecondary { get; }
            }
        }
    }
}
