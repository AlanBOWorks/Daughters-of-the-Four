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

        public bool CurrentControllerIsActive() => PlayerTeamType.CanControl() || EnemyTeamType.CanControl();

       
        public void OnTempoStartControl(in CombatTeamControllerBase controller,in CombatEntity firstEntity)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            controller.AllowControl = false;
        }

        public void OnTempoFinishLastCall(in CombatTeamControllerBase controller)
        {
            InvokeControls();
        }

        public void InvokeControls()
        {
            if(_playerTeamType.CanInvoke())
                InvokeControlEvent(in _playerTeamType);
            else if(_enemyTeamType.CanInvoke())
                InvokeControlEvent(in _enemyTeamType);
        }


        private static void InvokeControlEvent(in CombatTeamControllerBase controller)
        {
            if(controller == null) return;
            controller.AllowControl = true;
            var firstEntity = controller.ControllingTeam.GetTrinityActiveMembers()[0];
            CombatSystemSingleton.EventsHolder.OnTempoStartControl(controller, in firstEntity);
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
        internal bool CanInvoke() => ControllingTeam != null && ControllingTeam.IsControlActive;

        [ShowInInspector]
        internal bool CanControl() => AllowControl && CanInvoke();

        internal bool AllowControl;

        public IReadOnlyCollection<CombatEntity> GetTrinityMembers() => ControllingTeam.GetTrinityActiveMembers();
        public IReadOnlyCollection<CombatEntity> GetOffMembers() => ControllingTeam.GetOffMembersActiveMembers();
        public IReadOnlyList<CombatEntity> GetAllActiveMembers() => ControllingTeam.GetActiveMembers();

        [ShowInInspector]
        public CombatTeam ControllingTeam { get; private set; }
        internal void Injection(CombatTeam team) => ControllingTeam = team;
        
        
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
