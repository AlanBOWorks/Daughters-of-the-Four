using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace _CombatSystem
{
    public class SkillCooldownHandler : ITempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            ReduceCooldown(entity.CombatSkills.AllSkills);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            CheckAndResetIdle(entity.CombatSkills.AllSkills);
        }

        private static void ReduceCooldown(List<CombatSkill> skills)
        {
            foreach (var skill in skills)
            {
                skill.OnCharacterAction();
            }
        }

        private static void CheckAndResetIdle(List<CombatSkill> skills)
        {
            foreach (var skill in skills)
            {
                skill.OnCharacterFinish();
            }
        }
    }
}
