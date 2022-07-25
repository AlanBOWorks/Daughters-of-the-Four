using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Stats
{
    /**
 * Offensive = (Base + Buff) * (1 * Burst)
 * Support = (Base + Buff) * (1 * Burst)
 * Vitality = (Base + Buff) * (1 * Burst) [Mortality Exclude > Mortality = Base]
 *
 * Concentration = Base + Buff + Burst)
 */
    public static class UtilsCombatStats
    {
        /// <summary>
        /// Checks and returns the desired stats from [<see cref="BurstStats"/>]'s stats, depending if is self or other
        /// </summary>
        /// <param name="targetStats">The selection from do you want to extract the stats</param>
        /// <param name="controlStats">A control reference to know if the [<paramref name="targetStats"/>] is self</param>
        /// <returns></returns>
        public static StatsBase<float> GetBurstStats(CombatStats targetStats, CombatStats controlStats = null)
        {
            var burstStats = targetStats.BurstStats;
            return controlStats == targetStats
                ? burstStats.SelfBuffs
                : burstStats.AlliesBuffs;
        }

        /// <summary>
        /// Determines if the [<see cref="CombatEntity"/>] has the requirements to be affected by first call
        /// of actions/Tempo.
        /// </summary>
        public static bool CanRequestActing(CombatEntity entity)
        {
            var stats = entity.Stats;
            float actionsAmount = stats.BaseStats.ActionsType;
            // A zero actions is equivalent to a passive Entity (and can be affected on start sequence events/effects)
            // while a negative actions type are in-mobile entities (obstacles like entities) than can't be affected by
            // "time" nor "effects" related to tempo
            if (actionsAmount <= 0) return false;

            int skillsCount = entity.GetCurrentSkills().Count;
            return skillsCount > 0;
        }

        private const float MaxActionAmount = 12f;

        /// <summary>
        /// Check if is Alive and its actionsLimit > 0;
        /// </summary>
        public static bool CanControlRequest(CombatEntity entity)
        {
            float actionsLimit = CalculateActionsLimit(entity.Stats);
            return IsAlive(entity) && actionsLimit > 0;
        }

        /// <summary>
        /// Checks if the entity has enough Actions points and isAlive
        /// </summary>
        public static bool CanControlAct(CombatEntity entity)
        {
            return IsAlive(entity) && HasActionsLeft(entity.Stats);
        }
        /// <summary>
        /// Checks if the entity has enough Actions points
        /// </summary>
        public static bool HasActionsLeft(CombatStats stats)
        {
            float actionsLimit = CalculateActionsLimit(stats);
            var usedActionsAmount = stats.UsedActions;

            return usedActionsAmount < actionsLimit;
        }

        public static float CalculateActionsLimit(CombatStats stats)
        {
            float actionsLimit =
                stats.BaseStats.ActionsType + stats.BuffStats.ActionsType + stats.BurstStats.ActionsType;
            if (actionsLimit > MaxActionAmount) return MaxActionAmount;
            return actionsLimit;
        }


        /// <summary>
        /// <inheritdoc cref="CanControlAct"/> and if the entity is [<seealso cref="CombatTeam.IsActive(CombatEntity)"/>]= true;
        /// </summary>
        public static bool IsControlActive(CombatEntity entity)
        {
            return entity.Team.IsActive(entity) && CanControlAct(entity);
        }


        public static bool IsAlive(CombatEntity entity) => IsAlive(entity.Stats);

        public static bool IsAlive(CombatStats stats)
        {
            return stats.CurrentMortality > 0 || stats.CurrentHealth > 0;
        }



        // ----- TEMPO -----
        public static void ResetTempoStats(CombatStats stats)
        {
            ResetInitiative(stats);
            ResetActions(stats);
        }
        // ----- INITIATIVE -----
        private const float MaxInitiativeValue = TempoTicker.LoopThresholdAsIntended;
        public static void TickInitiative(CombatStats stats, float addition)
        {
            float targetInitiative = stats.CurrentInitiative + addition;
            if (targetInitiative >= MaxInitiativeValue)
            {
                targetInitiative = MaxInitiativeValue;
            }

            stats.CurrentInitiative = targetInitiative;
        }
        public static void BurstTickInitiative(CombatStats stats, float addition)
        {
            float targetInitiative = stats.InitiativeOffset + addition;
            if (targetInitiative >= MaxInitiativeValue)
            {
                targetInitiative = MaxInitiativeValue;
            }

            stats.InitiativeOffset = targetInitiative;
        }

        private const float MinInitiativeOffsetValue = -12;
        public static void ReduceTickInitiative(CombatStats stats, float reduction)
        {
            float targetInitiative = stats.InitiativeOffset - reduction;
            if (targetInitiative < MinInitiativeOffsetValue)
            {
                targetInitiative = MinInitiativeOffsetValue;
            }

            stats.InitiativeOffset = targetInitiative;
        }


        public static void ResetInitiative(CombatStats stats)
        {
            stats.CurrentInitiative = 0;
            stats.InitiativeOffset = 0;
        }

        private const float InitiativeThreshold = TempoTicker.LoopThresholdAsIntended;
        /// <summary>
        /// Check if the stats are below the [<see cref="InitiativeThreshold"/>] and thus can be ticked
        /// </summary>
        /// <param name="stats"></param>
        /// <returns></returns>
        public static bool CanTick(CombatStats stats)
        {
            float currentInitiative = stats.CurrentInitiative;
            return currentInitiative <= InitiativeThreshold;
        }


        public static bool IsInitiativeEnough(CombatStats stats)
        {
            var currentTickAmount = stats.CurrentInitiative;
            return currentTickAmount >= InitiativeThreshold;

        }

        // ----- ACTIONS -----
        public static void ResetActions(CombatStats stats)
        {
            stats.UsedActions = 0;
        }

        public static void TickActions(CombatStats stats, ICombatSkill usedSkill)
        {
            stats.UsedActions += usedSkill.SkillCost;
        }


        public static void FullTickActions(CombatEntity entity) => FullTickActions(entity.Stats);

        public static void FullTickActions(CombatStats stats)
        {
            stats.UsedActions = MaxActionAmount + 1;
        }


        public static int CalculateRemainingSteps(CombatStats stats)
        {
            var speed = UtilsStatsFormula.CalculateInitiativeSpeed(stats);
            var currentInitiative = stats.CurrentInitiative;
            var remainingTicks = InitiativeThreshold - currentInitiative;

            return Mathf.RoundToInt(remainingTicks / speed);
        }


        public static float CalculateTempoPercent(float currentTickAmount)
        {
            const float initiativeThreshold = TempoTicker.LoopThreshold;
            return currentTickAmount / initiativeThreshold;
        }

    }
}

