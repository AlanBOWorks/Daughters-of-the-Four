using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Team
{
    public sealed class CombatTeamControllersHandler : IOppositionTeamStructureRead<CombatTeamControllerBase>,
        ITempoEntityStatesListener, ITempoTeamStatesListener
    {

       
        public CombatTeamControllerBase CurrentController { get; private set; }

        [ShowInInspector, HorizontalGroup()]
        public CombatTeamControllerBase PlayerTeamType { get; set; }
        [ShowInInspector, HorizontalGroup()]
        public CombatTeamControllerBase EnemyTeamType { get; set; }

        public bool CurrentControllerIsActive() => CurrentController != null;



        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            if(!canAct) return;

            var controller = UtilsTeam.GetElement(entity, this);
            controller.InjectionOnRequestSequence(entity);
            
            if (CurrentController != null) return;

            SwitchController(in controller);
        }

        private void SwitchController(in CombatTeamControllerBase controller)
        {
            CurrentController = controller;
            CombatSystemSingleton.EventsHolder.OnTempoStartControl(in controller);
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

        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            var oppositeControl = UtilsTeam.GetOppositeElement(CurrentController, this);

            if (oppositeControl == null || !oppositeControl.IsWaiting())
                CurrentController = null;
            else
                SwitchController(in oppositeControl);
        }
    }
    
    public abstract class CombatTeamControllerBase 
    {
        protected CombatTeamControllerBase()
        {
            ActiveControls = new List<CombatEntity>();
        }
        [ShowInInspector]
        protected readonly List<CombatEntity> ActiveControls;

        public IReadOnlyList<CombatEntity> GetActiveControllingEntities() => ActiveControls;

        internal bool IsWaiting() => ActiveControls.Count > 0;

        private CombatEntity _lastEntity;
        public void InjectionOnRequestSequence(CombatEntity entity)
        {
            ActiveControls.Add(entity);
            _lastEntity = entity;
        }

        public void Clear()
        {
            ActiveControls.Clear();
        }


        public void SafeRemove(CombatEntity entity)
        {
            if (!ActiveControls.Contains(entity))  return;
            OnRemoveMember(in entity);
        }


        private void OnRemoveMember(in CombatEntity entity)
        {
            ActiveControls.Remove(entity);
            if(_lastEntity == entity && ActiveControls.Count > 0)
                _lastEntity = ActiveControls[ActiveControls.Count - 1];
        }

        public void ForceFinish()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            eventsHolder.OnTempoFinishControl(this);
            
            foreach (var entity in ActiveControls)
            {
                eventsHolder.OnEntityFinishSequence(entity);
            }

            ActiveControls.Clear();
        }
    }
}
