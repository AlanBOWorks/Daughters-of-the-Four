using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.CombatSkills;
using CombatSystem.Events;
using CombatTeam;
using MEC;
using UnityEngine;

namespace CombatSystem
{
    public class EntityRandomController : IEntitySkillRequestHandler
    {
        public EntityRandomController()
        {
            _possibleSkillToPick = new List<CombatingSkill>();
        }

        private readonly List<CombatingSkill> _possibleSkillToPick;
        
        public IEnumerator<float> HandleRequestAction(SkillValuesHolders values)
        {
            var selectedSkill = DoRequestWithRandom(values.Performer);
            values.Inject(selectedSkill);

            yield return Timing.WaitForOneFrame;
        }

        private const float SharedSkillChance = .2f;
        private SkillUsageValues DoRequestWithRandom(CombatingEntity currentEntity)
        {
            var skillsHolder = currentEntity.SkillsHolder;
            CombatingSkillsGroup skills = skillsHolder.GetCurrentSkills();
            
            var selectedSkill = ChooseSkill(skills) ?? skillsHolder.WaitSkill;

            List<CombatingEntity> possibleTargets = UtilsTarget.GetPossibleTargets(currentEntity, selectedSkill.GetTargetType());

            var selectedTarget = ChooseTarget(possibleTargets);

            return new SkillUsageValues(selectedSkill, selectedTarget);
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

            return selectedSkill;

            void PickFromList(List<CombatingSkill> fromSkills)
            {
                _possibleSkillToPick.Clear();

                if(fromSkills == null) return;

                _possibleSkillToPick.AddRange(fromSkills);
                while (_possibleSkillToPick.Count > 0)
                {
                    int randomPick = Random.Range(0, _possibleSkillToPick.Count);
                    var possibleSkill = _possibleSkillToPick[randomPick];
                    _possibleSkillToPick.RemoveAt(randomPick);
                    if (!possibleSkill.CanBeUsed())
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
