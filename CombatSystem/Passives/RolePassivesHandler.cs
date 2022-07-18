using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Passives
{
    public static class RolePassivesHandler 
    {
        private static readonly PassivesHolder Passives = new PassivesHolder();

        public static void ExecutePassive(CombatEntity entity)
        {
            var passive = UtilsTeam.GetElement(entity.RoleType, Passives);
            passive?.DoPassive(entity);
        }

        private sealed class PassivesHolder : ITeamFlexStructureRead<RolePassiveBase>
        {
            public RolePassiveBase VanguardType { get; }
            public RolePassiveBase AttackerType { get; } = new OffensivePassive();
            public RolePassiveBase SupportType { get; }
            public RolePassiveBase FlexType { get; }
        }

        private abstract class RolePassiveBase : ICombatPassive, IBuffEffect
        {
            public EnumsEffect.ConcreteType GetEffectType() => EnumsEffect.ConcreteType.Buff;

            public void DoPassive(CombatEntity onEntity)
            {
                var stats = onEntity.Stats.BuffStats;
                DoPassiveBuff(stats,out var value);

                var eventsHolder = CombatSystemSingleton.EventsHolder;
                eventsHolder.OnPassiveTrigged(onEntity,this,ref value);
                eventsHolder.OnBuffDone(new EntityPairInteraction(onEntity), this, value);
            }

            protected abstract void DoPassiveBuff(StatsBase<float> stats, out float effectValue);
            public bool IsBurstEffect() => true;
        }
        private sealed class OffensivePassive : RolePassiveBase
        {
            private const float PassiveIncrement = .1f;
            protected override void DoPassiveBuff(StatsBase<float> stats, out float effectValue)
            {
                effectValue = PassiveIncrement;
                stats.AttackType += effectValue;
                stats.OverTimeType += effectValue;
            }
        }
    }
}
