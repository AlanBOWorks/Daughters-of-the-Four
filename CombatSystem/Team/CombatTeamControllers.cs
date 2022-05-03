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
            controller.InjectionOnRequestTrinitySequence(in entity);
            
            if (CurrentController != null) return;

            CurrentController = controller;
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if (!canAct) return;

            var controller = UtilsTeam.GetElement(entity, this);
            controller.InjectionOnRequestOffSequence(in entity);
        }

      

        public void OnEntityRequestSequence(CombatEntity entity, bool canAct)
        {
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            if(UtilsCombatStats.CanActRequest(entity)) return;

            PlayerTeamType.SafeRemove(entity);
            EnemyTeamType?.SafeRemove(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
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
                CurrentController = null;
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
            Clear(); //safe clear
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            Clear();
        }

        public void OnCombatQuit()
        {
            Clear();
        }

        private void Clear()
        {
            PlayerTeamType.Clear();
            EnemyTeamType?.Clear();
        }
    }
    
    public abstract class CombatTeamControllerBase : TeamBasicGroupHashSet<CombatEntity>
    {
        protected CombatTeamControllerBase() : base()
        {
            OffMembers = new HashSet<CombatEntity>();
            AllActiveMembers = new HashSet<CombatEntity>();
        }

        internal bool IsWaiting() => HashSet.Count > 0;
        [ShowInInspector]
        protected readonly HashSet<CombatEntity> OffMembers;
        public IReadOnlyCollection<CombatEntity> GetOffMembers() => OffMembers;

        protected readonly HashSet<CombatEntity> AllActiveMembers;
        public IReadOnlyCollection<CombatEntity> GetAllActiveMembers() => AllActiveMembers;

        public CombatTeam ControllingTeam { get; private set; }
        internal void Injection(CombatTeam team) => ControllingTeam = team;



        public void InjectionOnRequestTrinitySequence(in CombatEntity entity)
        {
            int index = UtilsTeam.GetRoleIndex(entity);
            Members[index] = entity;
            HashSet.Add(entity);
            AllActiveMembers.Add(entity);
        }

        public void InjectionOnRequestOffSequence(in CombatEntity entity)
        {
            OffMembers.Add(entity);
            AllActiveMembers.Add(entity);
        }

        public void Clear()
        {
            VanguardType = null;
            AttackerType = null;
            SupportType = null;
            OffMembers.Clear();
            HashSet.Clear();
            AllActiveMembers.Clear();
        }


        public void SafeRemove(CombatEntity entity)
        {
            if(!AllActiveMembers.Contains(entity)) return;
            AllActiveMembers.Remove(entity);

            for (int i = 0; i < Members.Length; i++)
            {
                if (Members[i] != entity) continue;

                Members[i] = null;
                HashSet.Remove(entity);
                return;
            }
            if (OffMembers.Contains(entity))
                OffMembers.Remove(entity);
        }
        
        public void ForceFinish()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var entity in HashSet)
            {
                eventsHolder.OnEntityFinishSequence(entity);
            }
            eventsHolder.OnTempoFinishControl(this);
            Clear();
        }
    }
}
