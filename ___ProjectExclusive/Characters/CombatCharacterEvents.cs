using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using Stats;

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
    public class CombatCharacterEventsBase : ITempoListenerVoid, IHealthZeroListener
    {
        [ShowInInspector]
        private List<ITempoListenerVoid> _onTempoListeners;
        [ShowInInspector]
        private List<IVitalityChangeListener> _onVitalityChange;
        [ShowInInspector]
        private List<ITemporalStatsChangeListener> _onTemporalStatsChange;
        [ShowInInspector]
        private List<IAreaStateChangeListener> _onAreaChange;

        [ShowInInspector]
        private List<IHealthZeroListener> _onHealthZeroListeners;

        public CombatCharacterEventsBase()
        {
            _onVitalityChange = new List<IVitalityChangeListener>();
            _onTemporalStatsChange = new List<ITemporalStatsChangeListener>();
            _onAreaChange = new List<IAreaStateChangeListener>();
            _onHealthZeroListeners = new List<IHealthZeroListener>();
        }

        public void Subscribe(ICharacterListener listener)
        {
            if (listener is ITempoListenerVoid tempoListener)
                CheckAndSubscribe(tempoListener);

            if (listener is IVitalityChangeListener vitalityListener)
                CheckAndSubscribe(vitalityListener);
            if (listener is ITemporalStatsChangeListener temporalStatListener)
                CheckAndSubscribe(temporalStatListener);
            if (listener is IAreaStateChangeListener areaStateListener)
                CheckAndSubscribe(areaStateListener);

            if (listener is IHealthZeroListener healthCheckListener)
                CheckAndSubscribe(healthCheckListener);
        }

        protected void CheckAndSubscribe(ITempoListenerVoid listener)
        {
            if(_onTempoListeners == null)
                _onTempoListeners = new List<ITempoListenerVoid>();
            _onTempoListeners.Add(listener);
        }

        protected void CheckAndSubscribe(IVitalityChangeListener listener)
        {
            if (_onVitalityChange == null)
                _onVitalityChange = new List<IVitalityChangeListener>();
            _onVitalityChange.Add(listener);
        }

        protected void CheckAndSubscribe(ITemporalStatsChangeListener listener)
        {
            if (_onTemporalStatsChange == null)
                _onTemporalStatsChange = new List<ITemporalStatsChangeListener>();
            _onTemporalStatsChange.Add(listener);
        }

        protected void CheckAndSubscribe(IAreaStateChangeListener listener)
        {
            if (_onAreaChange == null)
                _onAreaChange = new List<IAreaStateChangeListener>();
            _onAreaChange.Add(listener);
        }

        protected void CheckAndSubscribe(IHealthZeroListener listener)
        {
            if (_onHealthZeroListeners == null)
                _onHealthZeroListeners = new List<IHealthZeroListener>();
            _onHealthZeroListeners.Add(listener);
        }


        public void InvokeVitalityChange(CombatingEntity entity)
        {
            if(_onVitalityChange is null) return;

            IVitalityStats onStats = entity.CombatStats;
            foreach (IVitalityChangeListener listener in _onVitalityChange)
            {
                listener.OnVitalityChange(onStats);
            }
        }

        public void InvokeTemporalStatChange(CombatingEntity entity)
        {
            if(_onTemporalStatsChange is null) return;

            ICombatTemporalStats onStats = entity.CombatStats;
            foreach (ITemporalStatsChangeListener listener in _onTemporalStatsChange)
            {
                listener.OnTemporalStatsChange(onStats);
            }
        }
        public void InvokeAreaChange(CombatingEntity entity)
        {
            if(_onAreaChange is null) return;

            CharacterCombatAreasData areasData = entity.AreasDataTracker;
            foreach (IAreaStateChangeListener listener in _onAreaChange)
            {
                listener.OnAreaStateChange(areasData);
            }
        }

        public void OnInitiativeTrigger()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnInitiativeTrigger();
            }
        }

        public void OnDoMoreActions()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnDoMoreActions();
            }
        }

        public void OnFinisAllActions()
        {
            if(_onTempoListeners is null) return;

            foreach (ITempoListenerVoid listener in _onTempoListeners)
            {
                listener.OnFinisAllActions();
            }
        }

        public void OnHealthZero(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnHealthZero(entity);
            }
        }

        public void OnMortalityZero(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnMortalityZero(entity);
            }
        }

        public void OnRevive(CombatingEntity entity)
        {
            if(_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnRevive(entity);
            }
        }

        public void OnTeamHealthZero(CombatingTeam losingTeam)
        {
            if (_onHealthZeroListeners is null) return;

            foreach (IHealthZeroListener listener in _onHealthZeroListeners)
            {
                listener.OnTeamHealthZero(losingTeam);
            }
        }
    }

    public class CombatCharacterEvents : CombatCharacterEventsBase, IHitEventHandler
    {
        private readonly CombatingEntity _user;
        public readonly InHitEventHandler OnHitEvent;
        public CombatCharacterEvents(CombatingEntity user)
        {
            _user = user;
            OnHitEvent = new InHitEventHandler(user);
            CheckAndSubscribe(OnHitEvent);
        }

        private static CombatCharacterEventsBase GlobalEvents()
        {
            return CombatSystemSingleton.CharacterChangesEvent;
        }

        public void InvokeVitalityChange()
        {
            // This is to loop Global listeners (like unique UI than is not exclusive to each Entity)
            GlobalEvents().InvokeVitalityChange(_user);
            // This is to loop the listeners in the base (the UI over the character for example)
            InvokeVitalityChange(_user); 
        }

        public void InvokeTemporalStatChange()
        {
            _user.HarmonyBuffInvoker?.OnTemporalStatsChange();
            GlobalEvents().InvokeTemporalStatChange(_user);
            InvokeTemporalStatChange(_user);
        }

        public void InvokeAreaChange()
        {
            GlobalEvents().InvokeAreaChange(_user);
            InvokeAreaChange(_user);
        }

        public void SubscribeListener(ICombatHitListener listener)
            => OnHitEvent.SubscribeListener(listener);


        public void UnSubscribeListener(ICombatHitListener listener)
            => OnHitEvent.UnSubscribeListener(listener);
       
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
        void OnAreaStateChange(CharacterCombatAreasData data);
    }


    public interface IHealthZeroListener : ICharacterListener
    {
        void OnHealthZero(CombatingEntity entity);
        void OnMortalityZero(CombatingEntity entity);
        void OnRevive(CombatingEntity entity);
        void OnTeamHealthZero(CombatingTeam losingTeam);
    }
}
