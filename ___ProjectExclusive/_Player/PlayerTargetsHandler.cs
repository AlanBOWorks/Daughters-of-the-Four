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
            var predefinedUIElements 
                = PlayerEntitySingleton.PredefinedUIDictionary;
            foreach (CombatingEntity entity in possibleTargets)
            {
                playerElements[entity].GetTargetButton().Show();
                predefinedUIElements[entity].ShowTargetButton();
            }
        }

        private static void HideSkillTargets()
        {
            var playerElements 
                = PlayerEntitySingleton.CombatDictionary;
            var predefinedUIElements
                = PlayerEntitySingleton.PredefinedUIDictionary;
            foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> playerElement in playerElements)
            {
                var entity = playerElement.Key;
                playerElement.Value.GetTargetButton().Hide();
                predefinedUIElements[entity].HideTargetButton();

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
