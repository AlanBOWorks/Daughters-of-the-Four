using System;
using CombatSystem.Skills;
using CombatSystem.Team;
using ExplorationSystem.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem
{
    public class UDualSkillCraftHandler : MonoBehaviour
    {
        [SerializeField] 
        private UDualSkillCombinationElement resultSkillHolder;
        [ShowInInspector,HideInEditorMode]
        private CraftedDualSkill _combinationSkill;

        private void Awake()
        {
            _combinationSkill = new CraftedDualSkill();
        }

        private void OnDisable()
        {
            ResetCombinationValues();
        }

        private void ResetCombinationValues()
        {
            _currentUser = null;
            _combinationSkill = null;

            _currentMainSkill.Reset();
            _currentSecondarySkill.Reset();
        }

        private void LazyInstantiation()
        {
            if(_combinationSkill != null) return;

            _combinationSkill = new CraftedDualSkill();
        }

        private PlayerRunTimeEntity _currentUser;
        private SkillValues _currentMainSkill;
        private SkillValues _currentSecondarySkill;

        public void InjectSkill(IFullSkill skill, EnumTeam.Stance stance, bool isMainSkill)
        {
            LazyInstantiation();
            if(isMainSkill)
                HandleSkillValues(ref _currentMainSkill, skill, stance);
            else
                HandleSkillValues(ref _currentSecondarySkill, skill, stance);


            if (_currentMainSkill.IsInvalid() || _currentSecondarySkill.IsInvalid()) return;

            DoInjection();
            HandleResultHolder();
        }

        private void HandleSkillValues(ref SkillValues values, IFullSkill skill, EnumTeam.Stance stance)
        {
            if (skill == null) 
                throw new ArgumentNullException(nameof(skill), "Skill Can't be Null");
            values.Skill = skill;
            values.SkillStanceGroup = stance;
        }

        private void DoInjection()
        {
            if(_currentMainSkill.Skill == _currentSecondarySkill.Skill) 
                throw new InvalidOperationException("Combining same skills is not allow.");

            _combinationSkill.DoInjection(_currentMainSkill.Skill,_currentSecondarySkill.Skill);
            resultSkillHolder.DoInjection(_combinationSkill);
        }

        private void HandleResultHolder()
        {
            resultSkillHolder.ClearAndPrint();
        }

        public void InjectEntity(PlayerRunTimeEntity skillHolder)
        {
            _currentUser = skillHolder;
        }

        public void ConfirmCombination()
        {
            if(_currentMainSkill.IsInvalid() || _currentSecondarySkill.IsInvalid())
                throw new InvalidOperationException("One or more skills were [Null] during combination.");

            RemoveSkill(_currentMainSkill);
            RemoveSkill(_currentSecondarySkill);

            var craftedSkillsHolder = _currentUser.GetCraftsSkillsHolder();
            var craftedSkillsCollection = UtilsTeam.GetElement(_currentMainSkill.SkillStanceGroup, craftedSkillsHolder);
            craftedSkillsCollection.Add(_combinationSkill);


            ResetCombinationValues();


            void RemoveSkill(SkillValues values)
            {
                var skill = values.Skill;

                var skillsHolder = _currentUser.GetBasicsSkillsHolder();
                var skillsCollection = UtilsTeam.GetElement(values.SkillStanceGroup, skillsHolder);
                bool validOperation = skillsCollection.Remove(skill);
                if(!validOperation) throw new InvalidOperationException("The target skill's List didn't contain the" +
                                                                        "skill while removing; Registration of the skill was" +
                                                                        "invalid during the Injection");
            }
        }

        private struct SkillValues
        {
            public IFullSkill Skill;
            public EnumTeam.Stance SkillStanceGroup;

            public bool IsInvalid() => Skill == null;

            public void Reset()
            {
                Skill = null;
            }
        }
    }
}
