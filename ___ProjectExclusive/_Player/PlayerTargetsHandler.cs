using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace _Player
{
    public class PlayerTargetsHandler : IPlayerSkillListener
    {
        private static void ShowSkillTargets(CombatSkill skill)
        {
            var skillHandler = CombatSystemSingleton.PerformSkillHandler;
            var possibleTargets
                = skillHandler.HandlePossibleTargets(skill);

            var playerElements
                = PlayerEntitySingleton.CombatDictionary;
            foreach (CombatingEntity entity in possibleTargets)
            {
                playerElements[entity].GetTargetButton().Show();
            }
        }

        private static void HideSkillTargets()
        {
            var playerElements 
                = PlayerEntitySingleton.CombatDictionary;
            foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> playerElement in playerElements)
            {
                playerElement.Value.GetTargetButton().Hide();
            }
        }

        public void OnSkillSelect(CombatSkill selectedSkill)
        {
            HideSkillTargets();
            ShowSkillTargets(selectedSkill);
        }

        public void OnSkillDeselect(CombatSkill deselectSkill)
        {
            HideSkillTargets();
        }

        public void OnSubmitSkill(CombatSkill submitSkill)
        {
            HideSkillTargets();
        }
    }
}
