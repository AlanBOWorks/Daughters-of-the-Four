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
        ITempoControlStatesListener, ITempoControlStatesExtraListener,
        ICombatStartListener, ICombatTerminationListener
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

        // - PROBLEM: using CombatTeam.CanControl is automatic and will return true once all entities has ended their sequence;
        // meanwhile it should be called by the controller itself.
        // - SOLUTION: keep track of the current controller and check if null (this should be set to null once the controller
        // had being invoked)
        [ShowInInspector]
        public bool IsControlling() => _currentControl != null;

        public bool HasTeamWaiting() =>
            _playerTeamType.ControllingTeam.IsActive() ||
            _enemyTeamType.ControllingTeam.IsActive();


        public IEnumerable<CombatTeamControllerBase> GetActiveControllers()
        {
            if(IsActive(_playerTeamType))
                yield return _playerTeamType;
            if(IsActive(_enemyTeamType))
                yield return _enemyTeamType;

            bool IsActive(CombatTeamControllerBase controller) => controller.ControllingTeam.IsActive();
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller, CombatEntity firstEntity)
        {
            _currentControl = controller;
        }

        public void OnTempoStartControl(CombatTeamControllerBase controller, CombatEntity firstControl)
        {
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
        }
        
        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            _currentControl = null;
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
        }


        public void TryInvokeControl(in CombatTeamControllerBase controller)
        {
            var team = controller.ControllingTeam;
            bool isActive = team.IsActive();
            
            if (!isActive) return;
            
            UtilsTeamMembers.HandleActiveMembers(controller, team);
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

        public void InvokeFinishControl()
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
