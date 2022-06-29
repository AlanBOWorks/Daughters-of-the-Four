using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem._Core
{
    public sealed class CombatEntitiesTempoTicker :
        ICombatStatesListener
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
            var members = team.GetAllMembers();
            foreach (var member in members)
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
                if(!UtilsCombatStats.CanTick(stats) || !UtilsCombatStats.IsAlive(stats)) return;


                TickInitiative(stats, out var entityInitiativeAmount);
                UtilsCombatStats.CalculateTempoPercent(entityInitiativeAmount, out var initiativePercent);

                eventsHolder.OnEntityTick(entity, entityInitiativeAmount, initiativePercent);

                bool thresholdPassed = UtilsTempo.IsInitiativeTrigger(in entity);
                if (!thresholdPassed) return;

                bool canControl = UtilsCombatStats.CanControlRequest(entity);
                entity.Team.AddActiveEntity(entity, canControl);
            }
        }
        private const float InitiativeThreshold = TempoTicker.LoopThresholdAsIntended;
        public static void TickInitiative(CombatStats stats, out float currentTickAmount)
        {
            float initiativeIncrement = UtilsStatsFormula.CalculateInitiativeSpeed(stats);
            if (initiativeIncrement <= 0)
            {
                currentTickAmount = stats.CurrentInitiative;
                return;
            }

            currentTickAmount = initiativeIncrement + stats.CurrentInitiative;

            if (currentTickAmount >= InitiativeThreshold)
            {
                currentTickAmount = InitiativeThreshold;
            }

            stats.CurrentInitiative = currentTickAmount;
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
