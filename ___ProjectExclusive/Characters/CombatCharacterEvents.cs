using System.Collections.Generic;
using _CombatSystem;
using Sirenix.OdinInspector;
using Skills;

namespace Characters
{
    /// <summary>
    /// It's an invoker for [<see cref="ICharacterListener"/>] that will be invoked several times
    /// by any way that revolves a 'Character' (such HP changes, stats, buffs and others).<br></br>
    /// Unlike [<seealso cref="ITempoListener"/>] that requires Ticking and will
    /// be invoked in the specific [<seealso cref="CombatingEntity"/>]'s turn, this listeners
    /// can be invoked by anyone and in anytime. Thus this won't be included in the
    /// [<seealso cref="TempoHandler"/>] since this Handler only triggers the User (and not targets)
    /// and that's not the desired behaviour.
    /// <br></br>
    /// <br></br> _____ TL;DR _____ <br></br>
    /// [<see cref="ICharacterListener"/>]:
    /// can be invoked by anyone and anywhere (generally an User will invoke 
    /// a specific target's listeners)<br></br>
    /// [<seealso cref="ITempoListener"/>]:
    /// is deterministic and will only be invoked in one specific Entity that was triggered.
    /// </summary>
    public class CombatCharacterEventsBase 
    {
        [ShowInInspector]
        private readonly List<IVitalityChangeListener> _onVitalityChange;
        [ShowInInspector]
        private readonly List<ITemporalStatsChangeListener> _onTemporalStatsChange;
        [ShowInInspector]
        private readonly List<IAreaStateChangeListener> _onAreaChange;

        public CombatCharacterEventsBase()
        {
            _onVitalityChange = new List<IVitalityChangeListener>();
            _onTemporalStatsChange = new List<ITemporalStatsChangeListener>();
            _onAreaChange = new List<IAreaStateChangeListener>();
        }

        public void Subscribe(ICharacterListener listener)
        {
            if(listener is IVitalityChangeListener vitalityListener)
                _onVitalityChange.Add(vitalityListener);
            if(listener is ITemporalStatsChangeListener temporalStatListener)
                _onTemporalStatsChange.Add(temporalStatListener);
            if(listener is IAreaStateChangeListener areaStateListener)
                _onAreaChange.Add(areaStateListener);
        }

        public void RemoveListener(ICharacterListener listener)
        {
            if (listener is IVitalityChangeListener vitalityListener)
                _onVitalityChange.Remove(vitalityListener);
            if (listener is ITemporalStatsChangeListener temporalStatListener)
                _onTemporalStatsChange.Remove(temporalStatListener);
            if (listener is IAreaStateChangeListener areaStateListener)
                _onAreaChange.Remove(areaStateListener);
        }


        public void InvokeVitalityChange(CombatingEntity entity)
        {
            IVitalityStats onStats = entity.CombatStats;
            foreach (IVitalityChangeListener listener in _onVitalityChange)
            {
                listener.OnVitalityChange(onStats);
            }
        }

        public void InvokeTemporalStatChange(CombatingEntity entity)
        {
            ICombatTemporalStats onStats = entity.CombatStats;
            foreach (ITemporalStatsChangeListener listener in _onTemporalStatsChange)
            {
                listener.OnTemporalStatsChange(onStats);
            }
        }
        public void InvokeAreaChange(CombatingEntity entity)
        {
            CombatAreasData areasData = entity.AreasDataTracker;
            foreach (IAreaStateChangeListener listener in _onAreaChange)
            {
                listener.OnAreaStateChange(areasData);
            }
        }
    }

    public class CombatCharacterEvents : CombatCharacterEventsBase
    {
        private readonly CombatingEntity _user;
        public CombatCharacterEvents(CombatingEntity user)
        {
            _user = user;
        }

        public void InvokeVitalityChange()
        {
            InvokeVitalityChange(_user);
            CombatSystemSingleton.CharacterChangesEvent.InvokeVitalityChange(_user);
        }

        public void InvokeTemporalStatChange()
        {
            InvokeTemporalStatChange(_user);
            CombatSystemSingleton.CharacterChangesEvent.InvokeTemporalStatChange(_user);
        }

        public void InvokeAreaChange()
        {
            InvokeAreaChange(_user);
            CombatSystemSingleton.CharacterChangesEvent.InvokeAreaChange(_user);
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

    public interface IAreaStateChangeListener : ICharacterListener
    {
        void OnAreaStateChange(CombatAreasData data);
    }
}
