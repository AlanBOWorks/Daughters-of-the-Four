using System;
using System.Collections.Generic;
using System.Linq;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamControllersHandler : IOppositionTeamStructureRead<CombatTeamControllerBase>,
        ITempoTeamStatesListener,
        ICombatStatesListener
    {
        [ShowInInspector, HorizontalGroup()]
        private CombatTeamControllerBase _playerTeamType;
        [ShowInInspector, HorizontalGroup()]
        private CombatTeamControllerBase _enemyTeamType;

        [ShowInInspector]
        private CombatTeamControllerBase _currentControl;


        public CombatTeamControllerBase PlayerTeamType
        {
            get => _playerTeamType;
            set => _playerTeamType = value;
        }

        public CombatTeamControllerBase EnemyTeamType
        {
            get => _enemyTeamType;
            set => _enemyTeamType = value;
        }

        [ShowInInspector]
        public bool IsControlling() => _currentControl != null;


        public void TickControllers()
        {
            TryInvokeControl(in _playerTeamType, out bool playerWasInvoked);

            if (!playerWasInvoked)
            {
                TryInvokeControl(in _enemyTeamType, out _);
            }
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            var oppositeTeam = UtilsTeam.GetOppositeElement(controller, this);

            TryInvokeControl(in oppositeTeam, out _);
            _currentControl = null;
        }


     

        private void TryInvokeControl(in CombatTeamControllerBase controller, out bool controlWasInvoked)
        {
            
            var team = controller.ControllingTeam;
            bool isActive = team.IsActive();
            bool canControl = team.CanControl();
            controlWasInvoked = isActive;

            if (!isActive) return;




            var eventsHolder = CombatSystemSingleton.EventsHolder;

            if (canControl)
            {
                eventsHolder.OnTempoStartControl(in controller);
            }

            var activeMembers = team.GetActiveMembers();
            foreach (var pair in activeMembers)
            {
                var member = pair.Key;
                var canControlMember = pair.Value;
                HandleMember(in member, in canControlMember);
            }

            void HandleMember(in CombatEntity member, in bool canControlMember)
            {
                bool isTrinity = UtilsTeam.IsTrinityRole(in member);
                eventsHolder.OnEntityRequestSequence(member, canControlMember);
                if (isTrinity)
                    eventsHolder.OnTrinityEntityRequestSequence(member, canControlMember);
                else
                    eventsHolder.OnOffEntityRequestSequence(member, canControlMember);
            }


            controller.InvokeStartControl();
            _currentControl = controller;
        }



        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            PlayerTeamType.Injection(playerTeam);
            EnemyTeamType.Injection(enemyTeam);
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            _currentControl = null;
            PlayerTeamType.Clear();
            EnemyTeamType.Clear();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }
    }
    
    public abstract class CombatTeamControllerBase
    {
        

        public CombatTeam ControllingTeam { get; private set; }
        internal void Injection(CombatTeam team) => ControllingTeam = team;


        public IReadOnlyList<CombatEntity> GetAllControllingMembers() => ControllingTeam.GetControllingMembers();



        public abstract void InvokeStartControl();

        public void ForceFinish()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnTempoFinishControl(this);
        }

        public void Clear()
        {
            ControllingTeam = null;
        }
    }
}
