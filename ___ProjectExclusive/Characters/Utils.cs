using System;
using _CombatSystem;
using Skills;
using UnityEngine;

namespace Characters
{
    public static class UtilsCharacter
    {
        public const int PredictedAmountOfCharactersInBattle = CharacterArchetypes.AmountOfArchetypes * 2;

        public static bool IsAPlayerEntity(CombatingEntity entity)
        {
            CombatingTeam playerCharacters = CombatSystemSingleton.Characters.PlayerFaction;
            return playerCharacters.Contains(entity);
        }
    }


    public static class UtilsArea
    {
        public static TeamCombatData.Stance ParseStance(float stanceEquivalent)
        {
            TeamCombatData.Stance stance;
            if (stanceEquivalent == 0) stance = TeamCombatData.Stance.Neutral;
            else if (stanceEquivalent > 0) stance = TeamCombatData.Stance.Attacking;
            else stance = TeamCombatData.Stance.Defending;

            return stance;
        }

        public static void ToggleStance(CombatingEntity entity, TeamCombatData.Stance targetStance)
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
