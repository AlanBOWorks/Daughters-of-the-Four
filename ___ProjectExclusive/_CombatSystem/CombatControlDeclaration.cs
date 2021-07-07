using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    
    public class CombatControlDeclaration : ICombatPreparationListener
    {


        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            InitializeEntities(playerEntities);
            InitializeEntities(enemyEntities);
            CombatSystemSingleton.TeamsDataHandler 
                = new CombatTeamsHandler(playerEntities, enemyEntities);

            void InitializeEntities(CombatingTeam entities)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    if (entity.PositionTracker != null)
                    {
                        entity.PositionTracker.CombatPosition =
                            CharacterArchetypes.PositionType.InTeam; //Resets if already have tracker
                    }
                    else
                    {
                        CharacterPosition position
                            = new CharacterPosition(CharacterArchetypes.GetTeamPosition(i));
                        entity.PositionTracker = position;
                    }
                }
            }
            
        }
    }

    /// <summary>
    ///  Keeps track of the [<seealso cref="TeamCombatData.ControlAmount"/>]
    /// of both [<seealso cref="ICharacterFaction{T}"/>]
    ///  groups and normalize the variations.
    /// </summary>
    public class CombatTeamsHandler : ICharacterFaction<TeamCombatData>
    {
        public CombatTeamsHandler(CombatingTeam playerEntities, CombatingTeam enemyEntities)
        {
            PlayerFaction = playerEntities.Data;
            EnemyFaction = enemyEntities.Data;
        }

        public TeamCombatData PlayerFaction { get; }
        public TeamCombatData EnemyFaction { get; }

        private bool IsPlayerGroup(CombatingTeam team)
        {
            return PlayerFaction.Team == team;
        }
        public void DoVariation(CombatingTeam team, float controlAddition)
        {
            HandleTeams(team, out var actor, out var receiver);
            DoVariation(actor, receiver, controlAddition);
        }

        public void DoVariation(CombatingTeam team, TeamCombatData.States targetState)
        {
            HandleTeams(team, out var actor, out var receiver);
            DoVariation(actor, receiver, targetState);
        }


        private void HandleTeams(CombatingTeam team, out TeamCombatData actor, out TeamCombatData receiver)
        {
            if (IsPlayerGroup(team))
            {
                actor = PlayerFaction;
                receiver = EnemyFaction;
            }
            else
            {
                actor = EnemyFaction;
                receiver = PlayerFaction;
            }
        }
        private static void DoVariation(TeamCombatData actor, TeamCombatData receiver, float controlGain)
        {
            DoVariation(ref actor.ControlAmount);
            receiver.ControlAmount = -actor.ControlAmount;

            void DoVariation(ref float controlValue)
            {
                controlValue += controlGain;
                if (controlValue > 1) controlValue = 1;
            }
        }

        private static void DoVariation(TeamCombatData actor, TeamCombatData receiver,
            TeamCombatData.States targetState)
        {
            TeamCombatData winner;
            TeamCombatData loser;
            switch (targetState)
            {
                case TeamCombatData.States.Attacking:
                    winner = actor;
                    loser = receiver;
                    HandleTeams();
                    break;
                case TeamCombatData.States.Defending:
                    winner = receiver;
                    loser = actor;
                    HandleTeams();
                    break;
                default:
                    actor.State = TeamCombatData.States.Neutral;
                    receiver.State = TeamCombatData.States.Neutral;
                    break;
            }

            void HandleTeams()
            {
                winner.State = TeamCombatData.States.Attacking;
                loser.State = TeamCombatData.States.Defending;
            }


        }
    }

    public class CharacterPosition
    {
        public CharacterArchetypes.TeamPosition PositionInTeam;
        public CharacterArchetypes.PositionType CombatPosition;

        public CharacterPosition(CharacterArchetypes.TeamPosition positionInTeam)
        {
            PositionInTeam = positionInTeam;
            CombatPosition = CharacterArchetypes.PositionType.InTeam;
        }
    }
}
