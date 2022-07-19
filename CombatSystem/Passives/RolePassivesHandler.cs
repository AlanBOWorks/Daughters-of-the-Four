using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Localization;
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
            public RolePassiveBase VanguardType { get; } = new VanguardPassive();
            public RolePassiveBase AttackerType { get; } = new AttackerPassive();
            public RolePassiveBase SupportType { get; } = new SupportPassive();
            public RolePassiveBase FlexType { get; } = new FlexPassive();
        }

        private abstract class RolePassiveBase : ICombatPassive, IBuffEffect
        {
            public EnumsEffect.ConcreteType GetEffectType() => EnumsEffect.ConcreteType.Buff;
            public abstract string GetPassiveEffectText();

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

            protected string LocalizeStat(EnumStats.StatType stats, string valueText)
            {
                return LocalizationsCombat.LocalizeStatName(stats) + " +" + valueText;
            }
        }

        private sealed class VanguardPassive : RolePassiveBase
        {
            private const float PassiveIncrement = .25f;
            private static readonly string PassiveValueText = PassiveIncrement.ToString("P0");
            public override string GetPassiveEffectText()
            {
                return LocalizeStat(EnumStats.StatType.Health, PassiveValueText);
            }

            protected override void DoPassiveBuff(StatsBase<float> stats, out float effectValue)
            {
                effectValue = PassiveIncrement;
                stats.HealthType += effectValue;
            }
        }
        private sealed class AttackerPassive : RolePassiveBase
        {
            private const float PassiveIncrement = .1f;
            private static readonly string PassiveValueText = PassiveIncrement.ToString("P0");
            public override string GetPassiveEffectText()
            {
                return LocalizeStat(EnumStats.StatType.Attack, PassiveValueText) + "\n" +
                LocalizeStat(EnumStats.StatType.OverTime, PassiveValueText);
            }
            protected override void DoPassiveBuff(StatsBase<float> stats, out float effectValue)
            {
                effectValue = PassiveIncrement;
                stats.AttackType += effectValue;
                stats.OverTimeType += effectValue;
            }
        }
        private sealed class SupportPassive : RolePassiveBase
        {
            private const float PassiveIncrement = .1f;
            private static readonly string PassiveValueText = PassiveIncrement.ToString("P0");
            public override string GetPassiveEffectText()
            {
                return LocalizeStat(EnumStats.StatType.Buff, PassiveValueText);
            }
            protected override void DoPassiveBuff(StatsBase<float> stats, out float effectValue)
            {
                effectValue = PassiveIncrement;
                stats.BuffType += effectValue;
            }
        }
        private sealed class FlexPassive : RolePassiveBase
        {
            private const float PassiveIncrement = .25f;
            private static readonly string PassiveValueText = PassiveIncrement.ToString("P0");
            public override string GetPassiveEffectText()
            {
                return LocalizeStat(EnumStats.StatType.Control, PassiveValueText);
            }
            protected override void DoPassiveBuff(StatsBase<float> stats, out float effectValue)
            {
                effectValue = PassiveIncrement;
                stats.ControlType += effectValue;
            }
        }
    }
}
