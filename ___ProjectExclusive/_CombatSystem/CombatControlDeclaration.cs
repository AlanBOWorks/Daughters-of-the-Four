using System;
using System.Collections.Generic;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    
    public class CombatControlDeclaration : ICombatPreparationListener, ICombatStartListener
    {
        private CombatTeamsHandler _combatTeamsHandler;

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            InitializeEntities(playerEntities);
            InitializeEntities(enemyEntities);
            _combatTeamsHandler
                = new CombatTeamsHandler(playerEntities, enemyEntities);
            CombatSystemSingleton.TeamsDataHandler = _combatTeamsHandler;

            void InitializeEntities(CombatingTeam entities)
            {
                for (var i = 0; i < entities.Count; i++)
                {
                    var entity = entities[i];
                    entity.AreasDataTracker.PositionInTeam = CharacterArchetypes.GetTeamPosition(i);
                }
            }
            
        }

        public void OnCombatStart()
        {
            _combatTeamsHandler.OnCombatStart();
        }
    }

    /// <summary>
    ///  Keeps track of the [<seealso cref="TeamCombatData.ControlAmount"/>]
    /// of both [<seealso cref="ICharacterFaction{T}"/>]
    ///  groups and normalize the variations.
    /// </summary>
    public class CombatTeamsHandler : ICharacterFaction<TeamCombatData>, ICombatStartListener
    {
        public CombatTeamsHandler(CombatingTeam playerEntities, CombatingTeam enemyEntities)
        {
            PlayerFaction = playerEntities.Data;
            EnemyFaction = enemyEntities.Data;
            _listeners = new List<ITeamVariationListener>();
            _firstCall = true;
        }

        [ShowInInspector]
        private readonly List<ITeamVariationListener> _listeners;
        [ShowInInspector]
        private TeamCombatData.Stance _lastStance;

        private bool _firstCall;

        public void Subscribe(ITeamVariationListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnSubscribe(ITeamVariationListener listener)
        {
            _listeners.Remove(listener);
        }

        [ShowInInspector]
        public TeamCombatData PlayerFaction { get; }
        public TeamCombatData EnemyFaction { get; }

        private bool IsPlayerGroup(CombatingTeam team)
        {
            return PlayerFaction.Team == team;
        }

        [Button, HideInEditorMode]
        public void DoVariationPlayer(float controlAddition) => 
            DoVariation(PlayerFaction.Team, controlAddition);
        public void DoVariationPlayer(TeamCombatData.Stance targetStance) =>
            DoVariation(PlayerFaction.Team, targetStance);

        public void DoVariation(CombatingTeam team, float controlAddition)
        {
            HandleTeams(team, out var actor, out var receiver);
            DoVariation(actor, receiver, controlAddition);
            InvokeListeners();
        }

        public void DoVariation(CombatingTeam team, TeamCombatData.Stance targetStance)
        {
            HandleTeams(team, out var actor, out var receiver);
            DoVariation(actor, receiver, targetStance);
            InvokeListeners();
        }


        private void InvokeListeners()
        {
            float control = PlayerFaction.ControlAmount;
            TeamCombatData.Stance stance = PlayerFaction.stance;
            if (!_firstCall && stance == _lastStance)
            {
                foreach (ITeamVariationListener listener in _listeners)
                {
                    listener.OnPlayerControlVariation(control);
                }
            }
            else
            {
                _lastStance = stance;
                foreach (ITeamVariationListener listener in _listeners)
                {
                    listener.OnPlayerControlVariation(control, stance);
                }
            }

            
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
        private void DoVariation(TeamCombatData actor, TeamCombatData receiver, float controlGain)
        {
            DoVariation(ref actor.ControlAmount);
            receiver.ControlAmount = -actor.ControlAmount;
            DoVariationCheck(actor,receiver);

            void DoVariation(ref float controlValue)
            {
                controlValue += controlGain;
                if (controlValue > 1) controlValue = 1;
            }
        }

        public const float BreakStateThreshold = .25f;
        public const float NeutralBreakModifier = .65f;
        public const float NeutralReturnModifier = NeutralBreakModifier - BreakStateThreshold;
        private void DoVariationCheck(TeamCombatData check, TeamCombatData reaction)
        {
            float neutralModifier = (_lastStance == TeamCombatData.Stance.Neutral) ? 0 : 1;
            bool isAttacking = check.ControlAmount > NeutralBreakModifier - BreakStateThreshold 
                * neutralModifier;
            bool isDefending = check.ControlAmount < -NeutralBreakModifier + BreakStateThreshold 
                * neutralModifier;

            switch (check.stance)
            {
                case TeamCombatData.Stance.Attacking:
                    CheckForDefending();
                    CheckForNeutral();
                    break;
                case TeamCombatData.Stance.Neutral:
                    CheckForAttacking();
                    CheckForDefending();
                    break;
                case TeamCombatData.Stance.Defending:
                    CheckForNeutral();
                    CheckForAttacking();
                    break;
                default:
                    throw new NotImplementedException($"Team state not supported: {check.stance}");
            }


            void CheckForAttacking()
            {
                if (isAttacking)
                    DoVariation(check, reaction, TeamCombatData.Stance.Attacking);
            }

            void CheckForNeutral()
            {
                if (isAttacking || isDefending) return;
                    
                DoVariation(check, reaction, TeamCombatData.Stance.Neutral);
            }

            void CheckForDefending()
            {
                if (isDefending)
                    DoVariation(check, reaction, TeamCombatData.Stance.Defending);
            }
        }



        private static void DoVariation(TeamCombatData actor, TeamCombatData receiver,
            TeamCombatData.Stance targetStance)
        {
            TeamCombatData winner;
            TeamCombatData loser;
            switch (targetStance)
            {
                case TeamCombatData.Stance.Attacking:
                    winner = actor;
                    loser = receiver;
                    HandleTeams();
                    break;
                case TeamCombatData.Stance.Defending:
                    winner = receiver;
                    loser = actor;
                    HandleTeams();
                    break;
                default:
                    actor.stance = TeamCombatData.Stance.Neutral;
                    receiver.stance = TeamCombatData.Stance.Neutral;
                    break;
            }

            void HandleTeams()
            {
                winner.stance = TeamCombatData.Stance.Attacking;
                loser.stance = TeamCombatData.Stance.Defending;
            }


        }

        public void OnCombatStart()
        {
            InvokeListeners();
            _firstCall = false;
        }
    }

}
