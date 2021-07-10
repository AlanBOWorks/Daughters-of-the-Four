using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Skills;
using UnityEngine;

namespace _Player
{
    public class PlayerTargetsHandler
    {
        public void ShowSkillTargets(CombatSkill skill)
        {
            HideSkillTargets();
            var skillHandler = CombatSystemSingleton.actionSkillHandler;
            var possibleTargets
                = skillHandler.GetPossibleTargets(skill);

            var playerElements
                = PlayerEntitySingleton.CombatDictionary;
            foreach (CombatingEntity entity in possibleTargets)
            {
                playerElements[entity].GetTargetButton().Show();
            }
        }

        public void HideSkillTargets()
        {
            var playerElements 
                = PlayerEntitySingleton.CombatDictionary;
            foreach (KeyValuePair<CombatingEntity, PlayerCombatElement> playerElement in playerElements)
            {
                playerElement.Value.GetTargetButton().Hide();
            }
        }
    }
}
