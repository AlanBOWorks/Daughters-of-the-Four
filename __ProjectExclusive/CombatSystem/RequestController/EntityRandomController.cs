using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using CombatTeam;
using UnityEngine;

namespace CombatSystem
{
    public class EntityRandomController : IEntitySkillRequestHandler
    {
        public EntityRandomController()
        {
            _possibleSkillToPick = new List<CombatingSkill>();
        }

        private CombatingEntity _currentEntity;
        private Queue<SkillUsageValues> _injectInQueue;
        private readonly List<CombatingSkill> _possibleSkillToPick;
        public void OnRequestAction(CombatingEntity currentEntity, Queue<SkillUsageValues> injectSkillInQueue)
        {
            _injectInQueue = injectSkillInQueue;
            _currentEntity = currentEntity;

            DoRequestWithRandom();
        }

        public void OnDoMoreActions()
        {
            DoRequestWithRandom();
        }

        public void OnFailRequest(CombatingEntity currentEntity, Queue<SkillUsageValues> injectSkillInQueue)
        {
            DoRequestWithRandom();
        }

        private const float SharedSkillChance = .2f;
        private void DoRequestWithRandom()
        {
            CombatingSkillsGroup skills = _currentEntity.SkillsHolder.GetCurrentSkills();
            
            var selectedSkill = ChooseSkill(skills);
            List<CombatingEntity> possibleTargets = UtilsTarget.GetPossibleTargets(_currentEntity, selectedSkill.GetTargetType());

            var selectedTarget = ChooseTarget(possibleTargets);

            var finalSelection = new SkillUsageValues(selectedSkill, selectedTarget);
            _injectInQueue.Enqueue(finalSelection);
        }

        private CombatingSkill ChooseSkill(CombatingSkillsGroup skills)
        {
            CombatingSkill selectedSkill = null;

            bool isSharedSelection = Random.value < SharedSkillChance;
            List<CombatingSkill> firstCheckPool;
            List<CombatingSkill> secondCheckPool;

            if (isSharedSelection)
            {
                firstCheckPool = skills.SharedSkillTypes;
                secondCheckPool = skills.MainSkillTypes;
            }
            else
            {
                firstCheckPool = skills.MainSkillTypes;
                secondCheckPool = skills.SharedSkillTypes;
            }

            PickFromList(firstCheckPool);
            if (selectedSkill == null)
                PickFromList(secondCheckPool);
            // TODO check if still null > Use Wait (Skill)

            return selectedSkill;

            void PickFromList(List<CombatingSkill> fromSkills)
            {
                _possibleSkillToPick.Clear();
                _possibleSkillToPick.AddRange(fromSkills);
                while (_possibleSkillToPick.Count > 0)
                {
                    int randomPick = Random.Range(0, _possibleSkillToPick.Count);
                    var possibleSkill = _possibleSkillToPick[randomPick];
                    _possibleSkillToPick.RemoveAt(randomPick);
                    if (possibleSkill.IsInCooldown())
                        continue;
                    selectedSkill = possibleSkill;
                    break;
                }
            }

        }

        private static CombatingEntity ChooseTarget(List<CombatingEntity> possibleTargets)
        {
            int randomSelection = Random.Range(0, possibleTargets.Count);
            return possibleTargets[randomSelection];
        }

    }
}
