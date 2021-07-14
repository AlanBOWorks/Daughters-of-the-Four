using System;
using System.Collections.Generic;
using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{
    public class SkillCooldownHandler : ITempoListener
    {
        public void OnInitiativeTrigger(CombatingEntity entity)
        {
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            ReduceCooldown(entity.AllSkills);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            CheckAndResetIdle(entity.AllSkills);
        }

        private static void ReduceCooldown(List<CombatSkill> skills)
        {
            Debug.Log("Reduce cooldowns");
            foreach (var skill in skills)
            {
                skill.OnCharacterAction();
            }
        }

        private static void CheckAndResetIdle(List<CombatSkill> skills)
        {
            Debug.Log("Cooldown Zeros?");
            foreach (var skill in skills)
            {
                skill.OnCharacterFinish();
            }
        }
    }
}
