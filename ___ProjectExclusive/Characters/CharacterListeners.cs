using System.Collections.Generic;

namespace Characters
{
    public class CharacterListeners
    {
        private readonly CombatingEntity _entity;
        private readonly Queue<IVitalityChangeListener> _onVitalityChange;
        private readonly Queue<ITemporalStatsChangeListener> _onTemporalStatsChange;

        public CharacterListeners(CombatingEntity entity)
        {
            _entity = entity;
            _onVitalityChange = new Queue<IVitalityChangeListener>();
            _onTemporalStatsChange = new Queue<ITemporalStatsChangeListener>();
        }

        public void SubscribeListener(ICharacterListener listener)
        {
            if(listener is IVitalityChangeListener vitalityListener)
                _onVitalityChange.Enqueue(vitalityListener);
            if(listener is ITemporalStatsChangeListener temporalStatListener)
                _onTemporalStatsChange.Enqueue(temporalStatListener);
        }

        public void UpdateVitalityStats()
        {
            foreach (var listener in _onVitalityChange)
            {
                listener.OnVitalityChange(_entity.CombatStats);
            }
        }

        public void UpdateTemporalStats()
        {
            foreach (var listener in _onTemporalStatsChange)
            {
                listener.OnTemporalStatsChange(_entity.CombatStats);
            }
        }
    }

    /// <summary>
    /// A listener of stats changes (instead of every frame)
    /// </summary>
    public interface ICharacterListener { }

    public interface IVitalityChangeListener : ICharacterListener
    {
        void OnVitalityChange(IVitalityStats currentStats);
    }

    public interface ITemporalStatsChangeListener : ICharacterListener
    {
        void OnTemporalStatsChange(ICombatTemporalStats currentStats);
    }
}
