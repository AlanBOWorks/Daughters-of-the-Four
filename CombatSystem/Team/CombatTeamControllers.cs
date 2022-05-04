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
        ITempoEntityStatesListener, ITempoTeamStatesListener,
        ITempoDedicatedEntityStatesListener,
        ICombatStatesListener
    {

        [ShowInInspector]
        public CombatTeamControllerBase CurrentController { get; private set; }

        [ShowInInspector, HorizontalGroup()]
        public CombatTeamControllerBase PlayerTeamType { get; set; }
        [ShowInInspector, HorizontalGroup()]
        public CombatTeamControllerBase EnemyTeamType { get; set; }

        public bool CurrentControllerIsActive() => CurrentController != null;



        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            var controller = UtilsTeam.GetElement(entity, this);
            
            if (CurrentController != null) return;

            CurrentController = controller;
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
           
        }

      

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            // PROBLEM: can't remove entity in this events since there's a foreach loop in the controllers
            // thus making an InvalidOperationException
            // SOLUTION: check and remove on FinishAction
        }
        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
        }
        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller,in CombatEntity firstEntity)
        {
        }

        public void OnControlFinishAllActors(in CombatEntity lastActor)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var oppositeControl = UtilsTeam.GetOppositeElement(CurrentController, this);

            if (oppositeControl == null || !oppositeControl.IsWaiting())
            {
                CurrentController = null;
            }
            else
            {
                CurrentController = oppositeControl;
                InvokeControlEvent();
            }
        }

        public void InvokeControlEvent()
        {
            if(CurrentController == null) return; //Safe check (it should be false always)
            var firstEntity = CurrentController.ControllingTeam.GetTrinityActiveMembers()[0];
            CombatSystemSingleton.EventsHolder.OnTempoStartControl(CurrentController, in firstEntity);
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
            CurrentController = null;
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
        internal bool IsWaiting() => ControllingTeam != null && ControllingTeam.IsControlActive;

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
