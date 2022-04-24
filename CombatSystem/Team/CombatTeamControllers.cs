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
        ITempoDedicatedEntityStatesListener
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
            controller.InjectionOnRequestMainSequence(in entity);
            
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
    }
    
    public abstract class CombatTeamControllerBase : TeamBasicGroupDictionary<CombatEntity,bool>
    {
        protected CombatTeamControllerBase() : base()
        {
            OffMembers = new HashSet<CombatEntity>();
        }

        internal bool IsWaiting() => Dictionary.Count > 0;

        public readonly HashSet<CombatEntity> OffMembers; 
        public CombatTeam ControllingTeam { get; private set; }
        internal void Injection(CombatTeam team) => ControllingTeam = team;

        public void InjectionOnRequestMainSequence(in CombatEntity entity)
        {
            Dictionary.Add(entity,true);
        }

        public void InjectionOnRequestOffSequence(in CombatEntity entity)
        {
            OffMembers.Add(entity);
        }

        public void Clear()
        {
            VanguardType = false;
            AttackerType = false;
            SupportType = false;
            Dictionary.Clear();
            OffMembers.Clear();
        }


        public void SafeRemove(CombatEntity entity)
        {
            if (Dictionary.ContainsKey(entity))
            {
                Dictionary.Remove(entity);
                return;
            }
            if (OffMembers.Contains(entity))
                OffMembers.Remove(entity);
        }
        
        public void ForceFinish()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnTempoFinishControl(this);


            foreach (var pair in Dictionary)
            {
                eventsHolder.OnEntityFinishSequence(pair.Key);
            }
            Clear();
        }
    }
}
