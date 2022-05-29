using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
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

        public const int ReviveThreshold = TempoTicker.LoopThreshold;

        private static int CalculateTick(int current, int increment)
        {
            int target = current + increment;
            if (target > ReviveThreshold) return ReviveThreshold;
            return target;
        }


        private void DoHealKnockOut(CombatEntity performer,int index, int currentTick, int increment,in SystemCombatEventsHolder eventsHolder)
        {
            var target = _knockOutEntities[index];
            _knockOutValues[index] = currentTick;

            eventsHolder.OnKnockHeal(in performer, in target, in currentTick, in increment);
        }

        private void DoHealKnockOut(CombatEntity performer, int index, int currentTick, int increment)
            => DoHealKnockOut(performer, index, currentTick, increment, CombatSystemSingleton.EventsHolder);


        public void HealKnockOut(CombatEntity performer, CombatEntity target, float healPercent)
        {
            if(healPercent <= 0) return;

            int index = _knockOutEntities.IndexOf(target);
            float increment = ReviveThreshold * healPercent;
            int knockOutTick = CalculateTick(_knockOutValues[index] , (int)increment);
            
            DoHealKnockOut(performer,index,knockOutTick,ReviveTickIncrement);

            if (knockOutTick < ReviveThreshold) return;
            DoReviveEntity(target, true);

        }




        private const int ReviveTickIncrement = 1;
        private void IterateKnockOutCollections()
        {
            var eventHolder = CombatSystemSingleton.EventsHolder;
            for (var i = 0; i < _knockOutEntities.Count; i++)
            {
                TickHealEntity(ref i);
            }

            void TickHealEntity(ref int index)
            {
                int knockOutTick = CalculateTick(_knockOutValues[index] , ReviveTickIncrement);
                DoHealKnockOut(null, index, knockOutTick, ReviveTickIncrement, in eventHolder);

                if (knockOutTick < ReviveThreshold) return;
                DoReviveEntity(index, false);
                index--;

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
