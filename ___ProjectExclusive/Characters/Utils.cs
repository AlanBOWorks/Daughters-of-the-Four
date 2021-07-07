using System;
using _CombatSystem;
using UnityEngine;

namespace Characters
{
    public static class CharacterUtils
    {
        public const int PredictedAmountOfCharactersInBattle = CharacterArchetypes.AmountOfArchetypes * 2;

        public static bool IsAPlayerEntity(CombatingEntity entity)
        {
            CombatingTeam playerCharacters = CombatSystemSingleton.Characters.PlayerFaction;
            return playerCharacters.Contains(entity);
        }

    }
}
