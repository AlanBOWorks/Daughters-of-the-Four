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
                    entity.AreasDataTracker.PositionInTeam = UtilsCharacterArchetypes.GetTeamPosition(i);
                }
            }
            
        }

        public void OnCombatStart()
        {
            _combatControlsHandler.OnCombatStart();
        }
    }

    /// <summary>
    ///  Keeps track of the [<seealso cref="CombatTeamControl.ControlAmount"/>]
    /// of both [<seealso cref="ICharacterFaction{T}"/>]
    ///  groups and normalize the variations.
    /// </summary>
    public class CombatTeamControlsHandler : ICharacterFaction<CombatingTeam>, ICombatStartListener,
        IRoundListenerVoid
    {
        public CombatTeamControlsHandler(CombatingTeam playerEntities, CombatingTeam enemyEntities)
        {
            PlayerFaction = playerEntities;
            EnemyFaction = enemyEntities;
            _listeners = new List<ITeamVariationListener>();
            _firstCall = true;
        }

        [ShowInInspector]
        private readonly List<ITeamVariationListener> _listeners;
        [ShowInInspector]
        private EnumTeam.Stances _lastPlayerStance;

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
        public CombatingTeam PlayerFaction { get; }
        public CombatingTeam EnemyFaction { get; }

        private bool IsPlayerGroup(CombatingTeam team)
        {
            return PlayerFaction == team;
        }

        [Button, HideInEditorMode]
        private void DoVariationPlayer(float controlAddition) => 
            DoVariation(PlayerFaction, controlAddition);
       
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

            actingTeam.control.IsBurstStance = true;
            receiver.control.DoBurstControl(-burstControl); // Is negative because the Control is checked in negatives
            _burstRoundCount = team.StatsHolder.BurstCounterAmount;

            DoVariationCheck(actingTeam, receiver);
            InvokeListeners();
        }
        public void DoCounterBurstControl(CombatingTeam team, float counterBurst)
        {
            if (!IsBurstState()) return;
            HandleTeams(team, out var actingTeam, out var receiver);
            actingTeam.control.DoBurstVariation(counterBurst);

            _burstRoundCount = team.StatsHolder.BurstCounterAmount;
            _burstRoundCount -= _burstRoundCount;
            if (_burstRoundCount <= 0)
                FinishBurstControl();

            DoVariationCheck(actingTeam,receiver);
            InvokeListeners();
        }

        private void FinishBurstControl()
        {
            PlayerFaction.control.FinishBurstControl();
            EnemyFaction.control.FinishBurstControl();
            InvokeListeners();
        }


        private void InvokeListeners()
        {
            var playerControl = PlayerFaction.control;
            var enemyControl = EnemyFaction.control;
            float control = playerControl.GetControlAmount()
                - enemyControl.GetControlAmount();
            control = Mathf.Clamp(control, -1, 1);

                EnumTeam.Stances stance = playerControl.CurrentStance;
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

        private void HandleTeams(CombatingTeam team, out CombatingTeam actor, out CombatingTeam receiver)
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
        private void DoVariation(CombatingTeam actingTeam, CombatingTeam receiver, float controlGain)
        {
            var actingControl = actingTeam.control;
            var receiverControl = receiver.control;
            actingControl.VariateControl(controlGain);
            receiverControl.MirrorEnemy(actingControl);
        }

        private void DoVariationCheck(CombatingTeam check, CombatingTeam reaction)
        {
            bool isAttacking = reaction.control.IsLosing();
            bool isDefending = check.control.IsLosing();

            switch (_lastPlayerStance)
            {
                case EnumTeam.Stances.Attacking:
                    CheckForDefending();
                    CheckForNeutral();
                    break;
                case EnumTeam.Stances.Neutral:
                    CheckForAttacking();
                    CheckForDefending();
                    break;
                case EnumTeam.Stances.Defending:
                    CheckForNeutral();
                    CheckForAttacking();
                    break;
                default:
                    throw new NotImplementedException($"Team state not supported: {check.control.CurrentStance}");
            }


            void CheckForAttacking()
            {
                if (isAttacking)
                    DoVariation(check, reaction, EnumTeam.Stances.Attacking);
            }

            void CheckForNeutral()
            {
                if (isAttacking || isDefending) return;
                    
                DoVariation(check, reaction, EnumTeam.Stances.Neutral);
            }

            void CheckForDefending()
            {
                if (isDefending)
                    DoVariation(check, reaction, EnumTeam.Stances.Defending);
            }
        }



        private static void DoVariation(CombatingTeam actingTeam, CombatingTeam receiver,
            EnumTeam.Stances targetStance)
        {
            CombatingTeam winner;
            CombatingTeam loser;
            switch (targetStance)
            {
                case EnumTeam.Stances.Attacking:
                    winner = actingTeam;
                    loser = receiver;
                    HandleTeams();
                    break;
                case EnumTeam.Stances.Defending:
                    winner = receiver;
                    loser = actingTeam;
                    HandleTeams();
                    break;
                default:
                    actingTeam.control.DoForceStance(EnumTeam.Stances.Neutral);
                    receiver.control.DoForceStance(EnumTeam.Stances.Neutral);
                    break;
            }

            InvokeEvent(actingTeam);
            InvokeEvent(receiver);

            void HandleTeams()
            {
                winner.control.DoForceStance(EnumTeam.Stances.Attacking);
                loser.control.DoForceStance(EnumTeam.Stances.Defending);
            }
            void InvokeEvent(CombatingTeam targetTeam)
            {
                foreach (CombatingEntity entity in targetTeam)
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
