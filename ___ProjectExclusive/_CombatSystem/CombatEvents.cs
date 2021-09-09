using _Team;
using Characters;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatEvents : CombatCharacterEventsBase
    {
        public new void InvokeVitalityChange(CombatingEntity entity)
        {
            IVitalityStatsData<float> onStats = entity.CombatStats;
            foreach (IVitalityChangeListener listener in onVitalityChange)
            {
                listener.OnVitalityChange(onStats);
            }
            entity.Events.InvokeVitalityChange();

        }

        public new void InvokeTemporalStatChange(CombatingEntity entity)
        {
            ICombatHealthStatsData<float> onStats = entity.CombatStats;
            foreach (ICombatHealthChangeListener listener in onCombatHealthChange)
            {
                listener.OnTemporalStatsChange(onStats);
            }
            entity.Events.InvokeTemporalStatChange();
        }
        public new void InvokeAreaChange(CombatingEntity entity)
        {
            CharacterCombatAreasData areasData = entity.AreasDataTracker;
            foreach (IAreaStateChangeListener listener in onAreaChange)
            {
                listener.OnAreaStateChange(areasData);
            }
            entity.Events.InvokeAreaChange();
        }

        public new void OnHealthZero(CombatingEntity entity)
        {
            foreach (IHealthZeroListener listener in onHealthZeroListeners)
            {
                listener.OnHealthZero(entity);
            }
            entity.Events.OnHealthZero(entity);
        }

        public new void OnMortalityZero(CombatingEntity entity)
        {
            foreach (IHealthZeroListener listener in onHealthZeroListeners)
            {
                listener.OnMortalityZero(entity);
            }
            entity.Events.OnMortalityZero(entity);
        }

        public new void OnRevive(CombatingEntity entity)
        {
            foreach (IHealthZeroListener listener in onHealthZeroListeners)
            {
                listener.OnRevive(entity);
            }
            entity.Events.OnRevive(entity);
        }

        public new void OnTeamHealthZero(CombatingTeam losingTeam)
        {
            foreach (IHealthZeroListener listener in onHealthZeroListeners)
            {
                listener.OnTeamHealthZero(losingTeam);
            }
            foreach (CombatingEntity entity in losingTeam)
            {
                entity.Events.OnTeamHealthZero(losingTeam);
            }
        }
    }
}
