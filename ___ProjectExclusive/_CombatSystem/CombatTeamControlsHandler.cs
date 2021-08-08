using System;
using System.Collections.Generic;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    /// <summary>
    /// This instance is mean to exit in the [<see cref="SystemInvoker"/>] while it creates
    /// instances of [<see cref="CombatTeamControlsHandler"/>] on each combat iteration (the handlers are
    /// volatiles).
    /// </summary>
    public class CombatControlDeclaration : ICombatPreparationListener, ICombatStartListener
    {
        private CombatTeamControlsHandler _combatControlsHandler;

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            InitializeEntities(playerEntities);
            InitializeEntities(enemyEntities);
            _combatControlsHandler
                = new CombatTeamControlsHandler(playerEntities, enemyEntities);
            CombatSystemSingleton.CombatTeamControlHandler = _combatControlsHandler;

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
            _combatControlsHandler.OnCombatStart();
        }
    }

    /// <summary>
    ///  Keeps track of the [<seealso cref="TeamCombatState.ControlAmount"/>]
    /// of both [<seealso cref="ICharacterFaction{T}"/>]
    ///  groups and normalize the variations.
    /// </summary>
    public class CombatTeamControlsHandler : ICharacterFaction<TeamCombatState>, ICombatStartListener,
        IRoundListenerVoid
    {
        public CombatTeamControlsHandler(CombatingTeam playerEntities, CombatingTeam enemyEntities)
        {
            PlayerFaction = playerEntities.State;
            EnemyFaction = enemyEntities.State;
            _listeners = new List<ITeamVariationListener>();
            _firstCall = true;
        }

        [ShowInInspector]
        private readonly List<ITeamVariationListener> _listeners;
        [ShowInInspector]
        private TeamCombatState.Stances _lastPlayerStance;

        private bool _firstCall;
        private int _burstRoundCount;
        public bool IsBurstState() => _burstRoundCount > 0;

        public void Subscribe(ITeamVariationListener listener)
        {
            _listeners.Add(listener);
        }

        public void UnSubscribe(ITeamVariationListener listener)
        {
            _listeners.Remove(listener);
        }

        [ShowInInspector]
        public TeamCombatState PlayerFaction { get; }
        public TeamCombatState EnemyFaction { get; }

        private bool IsPlayerGroup(CombatingTeam team)
        {
            return PlayerFaction.Team == team;
        }

        [Button, HideInEditorMode]
        private void DoVariationPlayer(float controlAddition) => 
            DoVariation(PlayerFaction.Team, controlAddition);
       
        public void DoVariation(CombatingTeam team, float controlAddition)
        {
            HandleTeams(team, out var actingTeam, out var receiver);
            DoVariation(actingTeam, receiver, controlAddition);
            InvokeListeners();
        }

        public void DoBurstControl(CombatingTeam team, float burstControl)
        {
            if (IsBurstState()) return;
            HandleTeams(team, out var actingTeam, out var receiver);

            actingTeam.IsBurstStance = true;
            receiver.DoBurstControl(-burstControl); // Is negative because the Control is checked in negatives
            _burstRoundCount = team.StatsHolder.BurstCounterAmount;

            DoVariationCheck(actingTeam, receiver);
            InvokeListeners();
        }
        public void DoCounterBurstControl(CombatingTeam team, float counterBurst)
        {
            if (!IsBurstState()) return;
            HandleTeams(team, out var actingTeam, out var receiver);
            actingTeam.DoBurstVariation(counterBurst);

            _burstRoundCount = team.StatsHolder.BurstCounterAmount;
            _burstRoundCount -= _burstRoundCount;
            if (_burstRoundCount <= 0)
                FinishBurstControl();

            DoVariationCheck(actingTeam,receiver);
            InvokeListeners();
        }

        private void FinishBurstControl()
        {
            PlayerFaction.FinishBurstControl();
            EnemyFaction.FinishBurstControl();
            InvokeListeners();
        }


        private void InvokeListeners()
        {
            float control = PlayerFaction.GetControlAmount()
                - EnemyFaction.GetControlAmount();
            control = Mathf.Clamp(control, -1, 1);

                TeamCombatState.Stances stance = PlayerFaction.CurrentStance;
            if (!_firstCall && stance == _lastPlayerStance)
            {
                foreach (ITeamVariationListener listener in _listeners)
                {
                    listener.OnPlayerControlVariation(control);
                }
            }
            else
            {
                _lastPlayerStance = stance;
                foreach (ITeamVariationListener listener in _listeners)
                {
                    listener.OnPlayerControlVariation(control, stance);
                }
            }
        }

        private void HandleTeams(CombatingTeam team, out TeamCombatState actor, out TeamCombatState receiver)
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
        private void DoVariation(TeamCombatState actingTeam, TeamCombatState receiver, float controlGain)
        {
            DoVariation(ref actingTeam.TeamControlAmount);
            receiver.TeamControlAmount = -actingTeam.GetControlAmount();
            DoVariationCheck(actingTeam,receiver);

            void DoVariation(ref float controlValue)
            {
                controlValue += controlGain;
                if (controlValue > 1) controlValue = 1;
            }
        }

        private void DoVariationCheck(TeamCombatState check, TeamCombatState reaction)
        {
            bool isAttacking = reaction.IsInDanger();
            bool isDefending = check.IsInDanger();

            switch (_lastPlayerStance)
            {
                case TeamCombatState.Stances.Attacking:
                    CheckForDefending();
                    CheckForNeutral();
                    break;
                case TeamCombatState.Stances.Neutral:
                    CheckForAttacking();
                    CheckForDefending();
                    break;
                case TeamCombatState.Stances.Defending:
                    CheckForNeutral();
                    CheckForAttacking();
                    break;
                default:
                    throw new NotImplementedException($"Team state not supported: {check.CurrentStance}");
            }


            void CheckForAttacking()
            {
                if (isAttacking)
                    DoVariation(check, reaction, TeamCombatState.Stances.Attacking);
            }

            void CheckForNeutral()
            {
                if (isAttacking || isDefending) return;
                    
                DoVariation(check, reaction, TeamCombatState.Stances.Neutral);
            }

            void CheckForDefending()
            {
                if (isDefending)
                    DoVariation(check, reaction, TeamCombatState.Stances.Defending);
            }
        }



        private static void DoVariation(TeamCombatState actingTeam, TeamCombatState receiver,
            TeamCombatState.Stances targetStance)
        {
            TeamCombatState winner;
            TeamCombatState loser;
            switch (targetStance)
            {
                case TeamCombatState.Stances.Attacking:
                    winner = actingTeam;
                    loser = receiver;
                    HandleTeams();
                    break;
                case TeamCombatState.Stances.Defending:
                    winner = receiver;
                    loser = actingTeam;
                    HandleTeams();
                    break;
                default:
                    actingTeam.DoForceStance(TeamCombatState.Stances.Neutral);
                    receiver.DoForceStance(TeamCombatState.Stances.Neutral);
                    break;
            }

            InvokeEvent(actingTeam);
            InvokeEvent(receiver);

            void HandleTeams()
            {
                winner.DoForceStance(TeamCombatState.Stances.Attacking);
                loser.DoForceStance(TeamCombatState.Stances.Defending);
            }
            void InvokeEvent(TeamCombatState target)
            {
                foreach (CombatingEntity entity in target.Team)
                {
                    entity.Events.InvokeAreaChange();
                }
            }
        }

        public void OnCombatStart()
        {
            InvokeListeners();
            _firstCall = false;
        }

        public void OnRoundCompleted()
        {
            if (!IsBurstState()) return;

            _burstRoundCount--;
            if(_burstRoundCount <= 0)
                FinishBurstControl();
        }
    }

}
