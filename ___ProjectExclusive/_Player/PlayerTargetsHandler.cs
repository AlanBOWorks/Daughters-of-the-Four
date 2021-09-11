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
            var user = TempoHandler.CurrentActingEntity;
            var skillHandler = CombatSystemSingleton.PerformSkillHandler;
            var possibleTargets
                = skillHandler.GetPossibleTargets(skill,user);
            var combatDictionary
                = PlayerEntitySingleton.CombatDictionary;
            
            foreach (CombatingEntity entity in possibleTargets)
            {
                combatDictionary[entity].GetTargetButton().Show();
                //predefinedUIElements[entity].ShowTargetButton();
            }
        }

        private static void HideSkillTargets()
        {
            var combatDictionary 
                = PlayerEntitySingleton.CombatDictionary;
            
            foreach (KeyValuePair<CombatingEntity, PlayerCombatUIElement> playerElement in combatDictionary)
            {
                var entity = playerElement.Key;
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
