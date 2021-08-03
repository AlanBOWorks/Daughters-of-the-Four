using System;
using _CombatSystem;
using _Team;
using Skills;
using UnityEngine;

namespace Characters
{
    public static class UtilsCharacter
    {
        public const int PredictedAmountOfCharactersInBattle = CharacterArchetypes.AmountOfArchetypesAmount * 2;

        public static bool IsAPlayerEntity(CombatingEntity entity)
        {
            CombatingTeam playerCharacters = CombatSystemSingleton.Characters.PlayerFaction;
            return playerCharacters.Contains(entity);
        }
    }


    public static class UtilsArea
    {
        public static TeamCombatState.Stance ParseStance(float stanceEquivalent)
        {
            TeamCombatState.Stance stance;
            if (stanceEquivalent == 0) stance = TeamCombatState.Stance.Neutral;
            else if (stanceEquivalent > 0) stance = TeamCombatState.Stance.Attacking;
            else stance = TeamCombatState.Stance.Defending;

            return stance;
        }

        public static void ToggleStance(CombatingEntity entity, TeamCombatState.Stance targetStance)
        {
            var areaData = entity.AreasDataTracker;
            if (areaData.IsForceStance)
            {
                Debug.Log("Restore stance");
                areaData.ForceStateFinish();
            }
            else
            {
                Debug.Log($"{entity.CharacterName} >> Change stance: {targetStance}");
                areaData.ForceState(targetStance);
            }
        }
    }
}
