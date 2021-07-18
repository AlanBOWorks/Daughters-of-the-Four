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
    public class CombatCharacterEvents 
    {
        [ShowInInspector]
        private readonly Queue<IVitalityChangeListener> _onVitalityChange;
        [ShowInInspector]
        private readonly Queue<ITemporalStatsChangeListener> _onTemporalStatsChange;
        [ShowInInspector]
        private readonly Queue<IAreaStateChangeListener> _onAreaChange;

        private readonly CombatingEntity _user;
        public CombatCharacterEvents(CombatingEntity user)
        {
            _user = user;
            _onVitalityChange = new Queue<IVitalityChangeListener>();
            _onTemporalStatsChange = new Queue<ITemporalStatsChangeListener>();
            _onAreaChange = new Queue<IAreaStateChangeListener>();
        }

        public void SubscribeListener(ICharacterListener listener)
        {
            if(listener is IVitalityChangeListener vitalityListener)
                _onVitalityChange.Enqueue(vitalityListener);
            if(listener is ITemporalStatsChangeListener temporalStatListener)
                _onTemporalStatsChange.Enqueue(temporalStatListener);
            if(listener is IAreaStateChangeListener areaStateListener)
                _onAreaChange.Enqueue(areaStateListener);
        }

       
        public void InvokeVitalityChange()
        {
            IVitalityStats onStats = _user.CombatStats;
            foreach (IVitalityChangeListener listener in _onVitalityChange)
            {
                listener.OnVitalityChange(onStats);
            }
        }
        public void InvokeTemporalStatChange()
        {
            ICombatTemporalStats onStats = _user.CombatStats;
            foreach (ITemporalStatsChangeListener listener in _onTemporalStatsChange)
            {
                listener.OnTemporalStatsChange(onStats);
            }
        }
        public void InvokeAreaChange()
        {
            CombatAreasData areasData = _user.AreasDataTracker;
            foreach (IAreaStateChangeListener listener in _onAreaChange)
            {
                listener.OnAreaStateChange(areasData);
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

    public interface IAreaStateChangeListener : ICharacterListener
    {
        void OnAreaStateChange(CombatAreasData data);
    }
}
