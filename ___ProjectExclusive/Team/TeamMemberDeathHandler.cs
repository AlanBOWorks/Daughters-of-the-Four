using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace _Team
{
    public class TeamMemberDeathHandler : ITempoTicker
    {
        private readonly CombatingTeam _team;
        private readonly List<CombatingEntity> _standByEntities;
        [ShowInInspector]
        private readonly List<float> _timerCheck;

        public TeamMemberDeathHandler(CombatingTeam team)
        {
            _team = team;
            int listsLength = team.Count;
            _standByEntities = new List<CombatingEntity>(listsLength);
            _timerCheck = new List<float>(listsLength);
        }


        public void TempoTick(float deltaVariation)
        {
            for (int i = 0; i < _timerCheck.Count; i++)
            {
                _timerCheck[i] -= deltaVariation;
                if (_timerCheck[i] < 0)
                    RemoveAt(i);
            }
        }
        public void Add(CombatingEntity healthLessEntity)
        {
            if(Contains(healthLessEntity) || !healthLessEntity.IsAlive()) return;

            var statsHolder = _team.StatsHolder;

                DoControlCalculation();
            Add(healthLessEntity,statsHolder.ReviveTime);

            void DoControlCalculation()
            {
                var teamControlHandler =
                    CombatSystemSingleton.CombatTeamControlHandler;
                float controlGain = 
                    CharacterArchetypes.GetElement(statsHolder.ControlLoseOnDeath, healthLessEntity.Role);
                controlGain = -controlGain; //Negative since controlLoseOnDeath is negative

                teamControlHandler.DoVariation(_team, controlGain);
            }
        }
        private void Add(CombatingEntity entity, float revivingThreshold)
        {
            _standByEntities.Add(entity);
            _timerCheck.Add(revivingThreshold);
            if(_team.Count <= _standByEntities.Count)
                OnAllDeath();
        }
        public void Remove(CombatingEntity entity)
        {
            for (int i = 0; i < _standByEntities.Count; i++)
            {
                if (entity != _standByEntities[i]) continue;

                RemoveAt(i);
                return;
            }
        }

        private void RemoveAt(int index)
        {
            var removing = _standByEntities[index];
            _standByEntities.RemoveAt(index);
            _timerCheck.RemoveAt(index);
            CallRevive(removing);
        }

        private static void CallRevive(CombatingEntity entity)
        {
            entity.CombatStats.Revive();
            entity.Events.OnRevive();
        }

        private const float HarmonyAddition = .5f;
        private const float HarmonyPenalty = -.5f;
        private const int ActionsAddition = 3; 
        public void OnAllDeath()
        {
            _timerCheck.Clear();
            _standByEntities.Clear();
            foreach (CombatingEntity entity in _team)
            {
                UtilsCombatStats.AddHarmony(entity,HarmonyPenalty);
                CallRevive(entity);
            }

            var enemyTeam = _team[0].CharacterGroup.Enemies;
            foreach (CombatingEntity entity in enemyTeam)
            {
                UtilsCombatStats.AddHarmony(entity,HarmonyAddition);
                UtilsCombatStats.AddActionAmount(entity.CombatStats,ActionsAddition);
            }

            CombatSystemSingleton.CharacterChangesEvent.OnTeamHealthZero(_team);
        }

        public bool Contains(CombatingEntity entity)
        {
            return _standByEntities.Contains(entity);
        }
    }
}
