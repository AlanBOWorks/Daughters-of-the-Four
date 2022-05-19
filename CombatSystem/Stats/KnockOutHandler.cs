using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;

namespace CombatSystem.Stats
{
    public sealed class KnockOutHandler : IDamageDoneListener, ITempoTickListener
    {
        public KnockOutHandler()
        {
            _knockOutEntities = new List<CombatEntity>();
            _knockOutValues = new List<int>();
            _reviveQueue = new Queue<CombatEntity>();
        }
        [ShowInInspector,HorizontalGroup()]
        private readonly List<CombatEntity> _knockOutEntities;
        [ShowInInspector, HorizontalGroup()]
        private readonly List<int> _knockOutValues;
        private readonly Queue<CombatEntity> _reviveQueue;


        private void AddKnockOut(in CombatEntity target)
        {
            _knockOutEntities.Add(target);
            _knockOutValues.Add(0);
        }

        private void RemoveKnockOut(int entityIndex)
        {
            _knockOutEntities.RemoveAt(entityIndex);
            _knockOutValues.RemoveAt(entityIndex);
        }

        public void DoReviveEntity(CombatEntity entity, bool isHealRevive)
        {
            if(!_knockOutEntities.Contains(entity))return;

            var entityIndex = _knockOutEntities.IndexOf(entity);
           DoReviveEntity(entityIndex, isHealRevive);
        }

        private void DoReviveEntity(int entityIndex, bool isHealRevive)
        {
            var entity = _knockOutEntities[entityIndex];
            RemoveKnockOut(entityIndex);
            HealReviveEntity(entity, isHealRevive);
        }

        private const float ReviveHealingPercent = 1f;
        private static void HealReviveEntity(CombatEntity entity, bool isHealRevive)
        {
            var entityStats = entity.Stats;
            UtilsEffect.DoHealToPercent(in entityStats, ReviveHealingPercent);
            CombatSystemSingleton.EventsHolder.OnRevive(in entity, isHealRevive);
        }

        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            AddKnockOut(in target);
        }


        public void OnRoundPassed()
        {
        }

        public void OnStopTicking()
        {
        }
        public void OnStartTicking()
        {
            _knockOutEntities.Clear();
            _knockOutValues.Clear();
            _reviveQueue.Clear();
        }
        public void OnTick()
        {
            IterateKnockOutCollections();
            IterateRevivingCollection();
        }

        private const int ReviveThreshold = TempoTicker.LoopThreshold;
        

       
        public void HealKnockOut(CombatEntity target, float percent)
        {
            int index = _knockOutEntities.IndexOf(target);
            float increment = ReviveThreshold * percent;
            var knockOutTick = _knockOutValues[index] + increment;
            if (knockOutTick < ReviveThreshold)
            {
                _knockOutValues[index] = (int)knockOutTick;
            }
            else
            {
                DoReviveEntity(target, true);
            }
        }




        private const int ReviveTickIncrement = 8;
        private void IterateKnockOutCollections()
        {
            for (var i = 0; i < _knockOutEntities.Count; i++)
            {
                TickHealEntity(ref i);
            }

            void TickHealEntity(ref int i)
            {
                var knockOutTick = _knockOutValues[i] + ReviveTickIncrement;
                if (knockOutTick < ReviveThreshold)
                {
                    _knockOutValues[i] = knockOutTick;
                }
                else
                {
                    DoReviveEntity(i, false);
                    i--;
                }
            }
        }

        private void IterateRevivingCollection()
        {
            while (_reviveQueue.Count > 0)
            {
                var entity = _reviveQueue.Dequeue();
                DoReviveEntity(entity, false);
            }
        }


    }
}
