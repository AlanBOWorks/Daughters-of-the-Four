using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem._Core
{
    public sealed class CombatEntitiesTempoTicker :
        ICombatStatesListener, ITempoEntityStatesListener
    {
        public CombatEntitiesTempoTicker()
        {
            _tickingTrackers = new HashSet<CombatEntity>();
        }

        [ShowInInspector] private readonly HashSet<CombatEntity> _tickingTrackers;


        public void ResetState()
        {
            _tickingTrackers.Clear();
        }

        public void AddEntities(CombatTeam team)
        {
            foreach (var member in team)
            {
                AddEntity(member);
            }
        }

        private void AddEntity(CombatEntity entity)
        {
            _tickingTrackers.Add(entity);
        }



        public void TickEntities()
        {
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var entity in _tickingTrackers)
            {
                HandleTickEntity(entity);
            }


            void HandleTickEntity(CombatEntity entity)
            {
                CombatStats stats = entity.Stats;
                if(!stats.IsAlive()) return;


                UtilsCombatStats.TickInitiative(stats, out var entityInitiativeAmount);
                UtilsCombatStats.CalculateTempoPercent(in entityInitiativeAmount, out var initiativePercent);

                eventsHolder.OnEntityTick(in entity, in entityInitiativeAmount, in initiativePercent);

                bool thresholdPassed = UtilsTempo.IsInitiativeTrigger(in entity);
                bool canControl = UtilsCombatStats.CanControlRequest(in entity);
                if (thresholdPassed)
                {
                    entity.Team.AddActiveEntity(in entity, in canControl);
                }
            }
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            if (!canControl) return;

            _tickingTrackers.Remove(entity);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
            
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            _tickingTrackers.Add(entity);
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            ResetState();//safe clear

            AddEntities(playerTeam);
            AddEntities(enemyTeam);
        }

        public void OnCombatStart()
        {
        }

        public void OnCombatEnd()
        {
            ResetState();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
        }

        public void OnCombatQuit()
        {
        }
    }
}
