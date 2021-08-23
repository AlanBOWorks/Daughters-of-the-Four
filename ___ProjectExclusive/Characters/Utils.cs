﻿using System;
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
        public static void InvokeAreaChange(CombatingEntity entity)
        {
            entity.Events.InvokeAreaChange();
            CombatSystemSingleton.GlobalCharacterChangesEvent.InvokeTemporalStatChange(entity);
        }

        public static EnumTeam.Stances ParseStance(float stanceEquivalent)
        {
            EnumTeam.Stances stance;
            if (stanceEquivalent == 0) stance = EnumTeam.Stances.Neutral;
            else if (stanceEquivalent > 0) stance = EnumTeam.Stances.Attacking;
            else stance = EnumTeam.Stances.Defending;

            return stance;
        }

        public static void ToggleStance(CombatingEntity entity, EnumTeam.Stances targetStance)
        {
            var areaData = entity.AreasDataTracker;
            if (areaData.IsForceStance)
            {
                areaData.ForceStateFinish();
            }
            else
            {
                areaData.ForceState(targetStance);
            }

            InvokeAreaChange(entity);
        }

        public static void TeamToggleStance(CombatingEntity teamHolder, EnumTeam.Stances targetStance)
        {
            var team = teamHolder.CharacterGroup.Team;
            var teamData = team.State;
            if (teamData.IsForcedStance)
            {
                teamData.FinishForceStance();
            }
            else
            {
                teamData.DoForceStance(targetStance);
            }

            foreach (CombatingEntity entity in team)
            {
                entity.Events.InvokeAreaChange();
            }
            CombatSystemSingleton.GlobalCharacterChangesEvent.InvokeTemporalStatChange(teamHolder);

        }

        public static void TogglePosition(CombatingEntity entity, CharacterArchetypes.FieldPosition targetPosition)
        {
            var areaTracker = entity.AreasDataTracker;
            var currentPosition = areaTracker.CombatFieldPosition;
            var finalPosition 
                = currentPosition != CharacterArchetypes.FieldPosition.InTeam 
                ? CharacterArchetypes.FieldPosition.InTeam 
                : targetPosition;


            areaTracker.CombatFieldPosition = finalPosition;

            InvokeAreaChange(entity);
        }
    }
}
