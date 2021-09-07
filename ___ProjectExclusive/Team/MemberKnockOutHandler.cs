using System.Collections.Generic;
using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace _Team
{
    public class MemberKnockOutHandler : ITempoTicker
    {
        private readonly CombatingTeam _team;
        private readonly List<CombatingEntity> _standByEntities;
        [ShowInInspector]
        private readonly List<float> _timerCheck;

        public MemberKnockOutHandler(CombatingTeam team)
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

            Add(healthLessEntity,statsHolder.ReviveTime);
            UtilsCombatStats.DoHarmonyKnockOutDamage(healthLessEntity);
            
        }
        private void Add(CombatingEntity entity, float revivingThreshold)
        {
            _standByEntities.Add(entity);
            _timerCheck.Add(revivingThreshold);
            if(_team.Count <= _standByEntities.Count)
                OnAllKnockOut();
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
            entity.Events.OnRevive(entity);
        }

        
        public void OnAllKnockOut()
        {
            _timerCheck.Clear();
            _standByEntities.Clear();
            foreach (CombatingEntity entity in _team)
            {
                CallRevive(entity);
            }

#if UNITY_EDITOR
            Debug.LogWarning("All Members KO");
#endif
        }

        public bool Contains(CombatingEntity entity)
        {
            return _standByEntities.Contains(entity);
        }
    }
}
