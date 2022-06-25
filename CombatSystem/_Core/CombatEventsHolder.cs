using System;
using System.Collections.Generic;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using CombatSystem.Team.VanguardEffects;
using Sirenix.OdinInspector;

namespace CombatSystem._Core
{
    public sealed class SystemCombatEventsHolder : ICombatEventsHolder,ITempoEntityPercentListener
        //: ICombatEventsHolder
    {
        public SystemCombatEventsHolder() 
        {
            _eventsHolder = new SystemEventsHolder();
            _tempoSequenceStepper = new TempoSequenceStepper();
            _entityEventHandler = new CombatEntityEventHandler();

            _entityTempoStepper = new EntityTempoStepper();

            _mainEventsHolders = GetMainEventsHolder();
            _discriminationEventsHolders = GetEventsHolderWithDiscrimination();

#if UNITY_EDITOR
            var debugEvents = CombatDebuggerSingleton.CombatEventsLogs;

            _eventsHolder.Subscribe(debugEvents);
#endif
        }

       

        // Problem: PlayerEvents could be invoked before core events while it should always be invoked last
        // so the data represented is the more consistent for the player;
        // Solution: Manual invoke through code
        [ShowInInspector]
        private readonly SystemEventsHolder _eventsHolder;
        private ControllerCombatEventsHolder _playerCombatEvents;
        private ControllerCombatEventsHolder _enemyCombatEvents;
        [ShowInInspector]
        private CombatEntityEventsHolder _currentDiscriminatedEntityEventsHolder;

        private readonly CombatEntityEventHandler _entityEventHandler;
        private readonly TempoSequenceStepper _tempoSequenceStepper;
        private readonly EntityTempoStepper _entityTempoStepper;

        private readonly IEnumerable<ICombatEventsHolder> _mainEventsHolders;
        private readonly IEnumerable<ICombatEventsHolderBase> _discriminationEventsHolders;

        public ControllerCombatEventsHolder GetControllerEvents(bool isPlayer) =>
            (isPlayer) ? _playerCombatEvents : _enemyCombatEvents;


        private IEnumerable<ICombatEventsHolder> GetMainEventsHolder()
        {
            yield return _eventsHolder;
            yield return _playerCombatEvents;
            yield return _enemyCombatEvents;
        }

        private IEnumerable<ICombatEventsHolderBase> GetEventsHolderWithDiscrimination()
        {
            yield return _eventsHolder;
            yield return _playerCombatEvents;
            yield return _enemyCombatEvents;
            yield return _currentDiscriminatedEntityEventsHolder;
        }

        public void SubscribeEventsHandler(PlayerCombatEventsHolder eventsHandler)
        {
            if (_playerCombatEvents != null)
                throw new ArgumentOutOfRangeException(nameof(eventsHandler),
                    "Trying to inject another Player's Events holder");

            _playerCombatEvents = eventsHandler;
            _eventsHolder.ManualSubscribe(eventsHandler);
        }

        public void SubscribeEventsHandler(EnemyCombatEventsHolder eventsHandler)
        {
            if(_enemyCombatEvents != null)
                throw new ArgumentOutOfRangeException(nameof(eventsHandler), 
                    "Trying to inject another enemy's Events Holder");

            _enemyCombatEvents = eventsHandler;
            _eventsHolder.ManualSubscribe(eventsHandler);
        }


        public void Subscribe(ICombatEventListener listener)
        {
            _eventsHolder.Subscribe(listener);
        }

        public void UnSubscribe(ICombatEventListener listener)
        {
            _eventsHolder.UnSubscribe(listener);
        }

        private void HandleCurrentDiscriminationEventsHolder(in CombatTeamControllerBase controller)
        {
            bool isPlayerElement = CombatSystemSingleton.TeamControllers.PlayerTeamType == controller;
            _currentDiscriminatedEntityEventsHolder = isPlayerElement 
                ? _playerCombatEvents.DiscriminationEventsHolder 
                : _enemyCombatEvents.DiscriminationEventsHolder;
        }
        private void HandleCurrentDiscriminationEventsHolder(in CombatEntity entity)
        {
            bool isPlayerElement = UtilsTeam.IsPlayerTeam(in entity);
            _currentDiscriminatedEntityEventsHolder = isPlayerElement
                ? _playerCombatEvents.DiscriminationEventsHolder
                : _enemyCombatEvents.DiscriminationEventsHolder;
        }

        // ------ PREPARATIONS ----- 
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
            }
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatPreStarts(playerTeam,enemyTeam);
            }
        }

        public void OnCombatStart()
        {
            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatStart();
            }
        }

        public void OnCombatEnd()
        {
            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatEnd();
            }
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            OnCombatEnd();
            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatFinish(isPlayerWin);
            }
        }

        public void OnCombatQuit()
        {
            OnCombatEnd();

            foreach (var eventsHolder in _mainEventsHolders)
            {
                eventsHolder.OnCombatQuit();
            }
        }


        // ------ TEMPO ----- 

        public void OnEntityTick(CombatEntity entity, float currentTick, float percentInitiative)
        {
            _playerCombatEvents.OnEntityTick(entity, currentTick, percentInitiative);
            _enemyCombatEvents.OnEntityTick(entity, currentTick, percentInitiative);
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            _entityTempoStepper.OnEntityRequestSequence(entity,canControl);
            _entityEventHandler.OnEntityRequestSequence(entity,canControl);

            HandleCurrentDiscriminationEventsHolder(in entity);
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnEntityRequestSequence(entity,canControl);
            }
        }

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnTrinityEntityRequestSequence(entity,canAct);
            }
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnOffEntityRequestSequence(entity,canAct);
            }
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnTrinityEntityFinishSequence(entity);
            }
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnOffEntityFinishSequence(entity);
            }
        }


        public void OnEntityRequestAction(CombatEntity entity)
        {
            _entityEventHandler.OnEntityRequestAction(entity);
            foreach (var eventsHolder in _discriminationEventsHolders)
            {
                eventsHolder.OnEntityRequestAction(entity);
            }

        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders) 
                eventsHolder.OnEntityBeforeSkill(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            _entityEventHandler.OnEntityFinishAction(entity);
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnEntityFinishAction(entity);

            _entityEventHandler.OnEntityAfterFinishAction(in entity);
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnEntityEmptyActions(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
            _entityEventHandler.OnEntityFinishSequence(entity,in isForcedByController);
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnEntityFinishSequence(entity,isForcedByController);

        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnAfterEntityRequestSequence(entity);


            _entityEventHandler.RequestEntityAction(in entity);
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnAfterEntitySequenceFinish(entity);


            _entityEventHandler.TryCallOnFinishAllActors(in entity);
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnNoActionsForcedFinish(entity);

        }


        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            HandleCurrentDiscriminationEventsHolder(in controller);
            _tempoSequenceStepper.OnTempoStartControl(in controller);

            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnTempoPreStartControl(controller);
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnAllActorsNoActions(lastActor);
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnControlFinishAllActors(lastActor);
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnTempoFinishControl(controller);
            
            _tempoSequenceStepper.OnTempoFinishControl(in controller);
            OnTempoFinishLastCall(controller);
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
            _tempoSequenceStepper.OnTempoFinishLastCall(in controller);

            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnTempoFinishLastCall(controller);
        }


        // ------ SKILLS ----- 

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var performer = values.Performer;
            var usedSkill = values.UsedSkill;

            _entityEventHandler.OnCombatSkillPreSubmit(usedSkill, performer);

            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatSkillSubmit(in values);

            _entityEventHandler.OnCombatSkillSubmit(usedSkill, performer);

        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            values.Extract(out var performer,out var target,out var usedSkill);
            _entityEventHandler.OnCombatSkillPerform(usedSkill, performer, target);

            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatSkillPerform(in values);
        }

        public void OnCombatPrimaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatPrimaryEffectPerform(performer,target,in values);
        }

        public void OnCombatSecondaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatSecondaryEffectPerform(performer,target,in values);
        }

        public void OnCombatVanguardEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatVanguardEffectPerform(performer,target,in values);
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnCombatSkillFinish(performer);

            OnEntityFinishAction(performer);
        }

        // ---- VANGUARD EFFECTS
        public void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnVanguardEffectSubscribe(in values);
        }

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnVanguardEffectIncrement(type,attacker);
        }

        public void OnVanguardEffectPerform(VanguardSkillUsageValues values)
        {
            foreach (var eventsHolder in _discriminationEventsHolders)
                eventsHolder.OnVanguardEffectPerform(values);
        }


        // ---- ENTITIES
        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnCreateEntity(in entity, in isPlayers);
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnDestroyEntity(in entity,in isPlayers);
        }
        
        // ---- VITALITY
        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnDamageBeforeDone(in performer,in target, in amount);

        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnRevive(in entity, isHealRevive);

        }


        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnShieldLost(in performer,in target, in amount);

        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnHealthLost(in performer, in target, in amount);
        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnMortalityLost(in performer, in target, in amount);
        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnDamageReceive(in performer, in target);

        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnKnockOut(in performer, in target);

        }

        public void OnShieldGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnShieldGain(in performer, in target, in amount);

        }

        public void OnHealthGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnHealthGain(in performer, in target, in amount);

        }

        public void OnMortalityGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnMortalityGain(in performer, in target, in amount);

        }

        public void OnRecoveryReceive(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnRecoveryReceive(in performer, in target);

        }

        public void OnKnockHeal(in CombatEntity performer, in CombatEntity target, in int currentAmount, in int amount)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnKnockHeal(in performer, in target,in currentAmount, in amount);

        }


        // ---- TEAM VALUEs
        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnStanceChange(team, switchedStance);

        }

        public void OnControlChange(CombatTeam team, float phasedControl, bool isBurst)
        {
            foreach (var eventsHolder in _mainEventsHolders)
                eventsHolder.OnControlChange(team, phasedControl, isBurst);

        }


        private sealed class SystemEventsHolder : CombatEventsHolder
        {

            public SystemEventsHolder()
            {
                _tempoTicker = new TempoTicker();

                CombatSystemSingleton.TempoTicker = _tempoTicker;
                Subscribe(_tempoTicker);
                Subscribe(_tempoTicker.EntitiesTempoTicker);
            }


            [TitleGroup("Combat System Exclusive"), ShowInInspector]
            private readonly TempoTicker _tempoTicker;
            protected override void SubscribeTempo(ITempoTickListener tickListener)
            {
                _tempoTicker.Subscribe(tickListener);
            }

            protected override void UnSubscribeTempo(ITempoTickListener tickListener)
            {
                _tempoTicker.UnSubscribe(tickListener);
            }

            public void ManualSubscribe(ITempoTickListener tickListener)
            {
                SubscribeTempo(tickListener);
            }

            public void ManualUnSubscribe(ITempoTickListener tickListener)
            {
                UnSubscribeTempo(tickListener);
            }

        }


    }


    public abstract class ControllerCombatEventsHolder : CombatEventsHolder, ITempoEntityPercentListener
    {
        private protected ControllerCombatEventsHolder() : base()
        {
            _tempoTickListeners = new HashSet<ITempoTickListener>();
            _tempoEntityPercentListeners = new HashSet<ITempoEntityPercentListener>();

            DiscriminationEventsHolder = new CombatEntityEventsHolder();
        }

        [ShowInInspector]
        private readonly HashSet<ITempoTickListener> _tempoTickListeners;
        [ShowInInspector]
        private readonly HashSet<ITempoEntityPercentListener> _tempoEntityPercentListeners;


        public readonly CombatEntityEventsHolder DiscriminationEventsHolder;

        protected override void SubscribeTempo(ITempoTickListener tickListener)
        {
            _tempoTickListeners.Add(tickListener);
        }

        protected override void UnSubscribeTempo(ITempoTickListener tickListener)
        {
            _tempoTickListeners.Remove(tickListener);
        }

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            if (listener is ITempoEntityPercentListener entityPercentListener)
                _tempoEntityPercentListeners.Add(entityPercentListener);
        }

        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is ITempoEntityPercentListener entityPercentListener)
                _tempoEntityPercentListeners.Remove(entityPercentListener);
        }

        public void ManualSubscription(ITempoEntityPercentListener percentListener)
        {
            _tempoEntityPercentListeners.Add(percentListener);
        }
        public void ManualUnSubscription(ITempoEntityPercentListener percentListener)
        {
            _tempoEntityPercentListeners.Remove(percentListener);
        }



        public void OnStartTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStartTicking();
            }
        }

        public void OnTick()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnTick();
            }
        }

        public void OnRoundPassed()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnRoundPassed();
            }
        }

        public void OnStopTicking()
        {
            foreach (var listener in _tempoTickListeners)
            {
                listener.OnStopTicking();
            }
        }
        public void OnEntityTick(CombatEntity entity, float currentTick, float percentInitiative)
        {
            foreach (var listener in _tempoEntityPercentListeners)
            {
                listener.OnEntityTick(entity, currentTick, percentInitiative);
            }
        }

        public CombatEntityEventsHolder GetDiscriminationEventsHolder() => DiscriminationEventsHolder;
    }

    public abstract class CombatEventsHolder : CombatEntityEventsHolder, ICombatEventsHolder

    {
        protected CombatEventsHolder() : base()
        {
            _combatPreparationListeners = new HashSet<ICombatPreparationListener>();
            _combatTerminationListeners = new HashSet<ICombatTerminationListener>();
            _combatStatesListeners = new HashSet<ICombatStatesListener>();
            _entitiesExistenceListeners = new HashSet<ICombatEntityExistenceListener>();
            
            
            _damageDoneListeners = new HashSet<IDamageDoneListener>();
            _recoveryDoneListeners = new HashSet<IRecoveryDoneListener>();
            _vitalityChangeListeners = new HashSet<IVitalityChangeListener>();
        }



        [Title("Events")]
        [ShowInInspector]
        private readonly ICollection<ICombatPreparationListener> _combatPreparationListeners;

        [ShowInInspector] 
        private readonly ICollection<ICombatTerminationListener> _combatTerminationListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatStatesListener> _combatStatesListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatEntityExistenceListener> _entitiesExistenceListeners;
        [ShowInInspector] 
        private readonly ICollection<IDamageDoneListener> _damageDoneListeners;
        [ShowInInspector]
        private readonly ICollection<IRecoveryDoneListener> _recoveryDoneListeners;

        [ShowInInspector] 
        private readonly ICollection<IVitalityChangeListener> _vitalityChangeListeners;

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            if (listener is ICombatPreparationListener preparationListener)
                _combatPreparationListeners.Add(preparationListener);

            if (listener is ICombatTerminationListener terminationListener)
            {
                _combatTerminationListeners.Add(terminationListener);
                if (listener is ICombatStatesListener combatStatesListener)
                    _combatStatesListeners.Add(combatStatesListener);
            }
           



            if (listener is ITempoTickListener tickListener)
                SubscribeTempo(tickListener);

            if (listener is ICombatEntityExistenceListener entityExistenceListener)
                _entitiesExistenceListeners.Add(entityExistenceListener);

            if(listener is IDamageDoneListener damageDoneListener)
                _damageDoneListeners.Add(damageDoneListener);
            if(listener is IRecoveryDoneListener recoveryDoneListener)
                _recoveryDoneListeners.Add(recoveryDoneListener);
            if(listener is IVitalityChangeListener vitalityChangeListener)
                _vitalityChangeListeners.Add(vitalityChangeListener);

        }

        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is ICombatPreparationListener preparationListener)
                _combatPreparationListeners.Remove(preparationListener);
            if (listener is ICombatTerminationListener terminationListener)
            {
                _combatTerminationListeners.Remove(terminationListener);
                if (listener is ICombatStatesListener combatStatesListener)
                    _combatStatesListeners.Remove(combatStatesListener);
            }

            if (listener is ITempoTickListener tickListener)
                UnSubscribeTempo(tickListener);


            if (listener is ICombatEntityExistenceListener entityExistenceListener)
                _entitiesExistenceListeners.Remove(entityExistenceListener);

            if (listener is IDamageDoneListener damageDoneListener)
                _damageDoneListeners.Remove(damageDoneListener);
            if (listener is IRecoveryDoneListener recoveryDoneListener)
                _recoveryDoneListeners.Remove(recoveryDoneListener);
            if (listener is IVitalityChangeListener vitalityChangeListener)
                _vitalityChangeListeners.Add(vitalityChangeListener);
        }


        protected abstract void SubscribeTempo(ITempoTickListener tickListener);
        protected abstract void UnSubscribeTempo(ITempoTickListener tickListener);


        public void ManualSubscribe(ICombatPreparationListener preparationListener)
        {
            _combatPreparationListeners.Add(preparationListener);
        }

        public void ManualSubscribe(ICombatStatesListener statesListener)
        {
            _combatStatesListeners.Add(statesListener);
        }



        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var listener in _combatPreparationListeners)
            {
                listener.OnCombatPrepares(allMembers, playerTeam, enemyTeam);
            }
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatPreStarts(playerTeam, enemyTeam);
            }
        }

        public void OnCombatStart()
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatStart();
            }
        }

        public void OnCombatEnd()
        {
            foreach (var listener in _combatTerminationListeners)
            {
                listener.OnCombatEnd();
            }
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            foreach (var listener in _combatTerminationListeners)
            {
                listener.OnCombatFinish(isPlayerWin);
            }
        }

        public void OnCombatQuit()
        {
            foreach (var listener in _combatTerminationListeners)
            {
                listener.OnCombatQuit();
            }
        }

        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var listener in _entitiesExistenceListeners)
            {
                listener.OnCreateEntity(in entity, in isPlayers);
            }
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
            foreach (var listener in _entitiesExistenceListeners)
            {
                listener.OnDestroyEntity(in entity, in isPlayers);
            }
        }

        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnDamageBeforeDone(in performer, in target, in amount);
            }
        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnRevive(in entity, isHealRevive);
            }
        }


        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnShieldLost(in performer,in target, in amount);
            }
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnHealthLost(in performer, in target, in amount);
            }

        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnMortalityLost(in performer, in target, in amount);
            }

        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnDamageReceive(in performer, in target);
            }
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnKnockOut(in performer, in target);
            }
        }


        public void OnShieldGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _recoveryDoneListeners)
            {
                listener.OnShieldGain(in performer, in target, in amount);
            }
        }

        public void OnHealthGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _recoveryDoneListeners)
            {
                listener.OnHealthGain(in performer, in target, in amount);
            }

        }

        public void OnMortalityGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            foreach (var listener in _recoveryDoneListeners)
            {
                listener.OnMortalityGain(in performer, in target, in amount);
            }

        }

        public void OnRecoveryReceive(in CombatEntity performer, in CombatEntity target)
        {
            foreach (var listener in _recoveryDoneListeners)
            {
                listener.OnRecoveryReceive(in performer, in target);
            }
        }

        public void OnKnockHeal(in CombatEntity performer, in CombatEntity target, in int currentTick, in int amount)
        {
            foreach (var listener in _recoveryDoneListeners)
            {
                listener.OnKnockHeal(in performer, in target, in currentTick, in amount);
            }
        }

    }

    public class CombatEntityEventsHolder :  ICombatEventsHolderBase
    {
        public CombatEntityEventsHolder()
        {
            _tempoEntityListeners = new HashSet<ITempoEntityStatesListener>();
            _tempoDedicatedEntitiesListeners = new HashSet<ITempoDedicatedEntityStatesListener>();
            _tempoEntityExtraListeners = new HashSet<ITempoEntityStatesExtraListener>();

            _tempoTeamListeners = new HashSet<ITempoTeamStatesListener>();

            _skillUsageListeners = new HashSet<ISkillUsageListener>();
            _effectUsageListeners = new HashSet<IEffectUsageListener>();
            _vanguardEffectUsageListeners = new HashSet<IVanguardEffectUsageListener>();

            _teamEventListeners = new HashSet<ITeamEventListener>();
        }

        [ShowInInspector,HorizontalGroup("Entities")] 
        private readonly ICollection<ITempoEntityStatesListener> _tempoEntityListeners;
        [ShowInInspector, HorizontalGroup("Entities")]
        private readonly ICollection<ITempoDedicatedEntityStatesListener> _tempoDedicatedEntitiesListeners;
        [ShowInInspector, HorizontalGroup("Entities")]
        private readonly ICollection<ITempoEntityStatesExtraListener> _tempoEntityExtraListeners;

        [ShowInInspector]
        private readonly ICollection<ITempoTeamStatesListener> _tempoTeamListeners;

        [ShowInInspector] private readonly ICollection<ISkillUsageListener> _skillUsageListeners;
        [ShowInInspector] private readonly ICollection<IEffectUsageListener> _effectUsageListeners;
        [ShowInInspector] private readonly ICollection<IVanguardEffectUsageListener> _vanguardEffectUsageListeners;

        [ShowInInspector] private readonly ICollection<ITeamEventListener> _teamEventListeners;


        public virtual void Subscribe(ICombatEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Event listener can't be Null");


            if (listener is ITempoEntityStatesListener tempoEntityListener)
                _tempoEntityListeners.Add(tempoEntityListener);
            if(listener is ITempoDedicatedEntityStatesListener tempoDedicatedEntityListener)
                _tempoDedicatedEntitiesListeners.Add(tempoDedicatedEntityListener);
            if(listener is ITempoEntityStatesExtraListener tempoExtraListener)
                _tempoEntityExtraListeners.Add(tempoExtraListener);

            if (listener is ITempoTeamStatesListener tempoTeamStatesListener)
                _tempoTeamListeners.Add(tempoTeamStatesListener);
            if (listener is ITeamEventListener teamEventListener)
                _teamEventListeners.Add(teamEventListener);


            if (listener is ISkillUsageListener skillUsageListener)
                _skillUsageListeners.Add(skillUsageListener);
            if(listener is IEffectUsageListener effectUsageListener)
                _effectUsageListeners.Add(effectUsageListener);
            if(listener is IVanguardEffectUsageListener vanguardEffectUsageListener)
                _vanguardEffectUsageListeners.Add(vanguardEffectUsageListener);


        }

        public virtual void UnSubscribe(ICombatEventListener listener)
        {
            if (listener is ITempoEntityStatesListener tempoEntityListener)
                _tempoEntityListeners.Remove(tempoEntityListener);
            if (listener is ITempoDedicatedEntityStatesListener tempoDedicatedEntityListener)
                _tempoDedicatedEntitiesListeners.Remove(tempoDedicatedEntityListener);
            if (listener is ITempoEntityStatesExtraListener tempoExtraListener)
                _tempoEntityExtraListeners.Remove(tempoExtraListener);

            if (listener is ITempoTeamStatesListener tempoTeamStatesListener)
                _tempoTeamListeners.Remove(tempoTeamStatesListener);


            if (listener is ISkillUsageListener skillUsageListener)
                _skillUsageListeners.Remove(skillUsageListener);
            if (listener is IEffectUsageListener effectUsageListener)
                _effectUsageListeners.Remove(effectUsageListener);
            if (listener is IVanguardEffectUsageListener vanguardEffectUsageListener)
                _vanguardEffectUsageListeners.Remove(vanguardEffectUsageListener);


            if (listener is ITeamEventListener teamEventListener)
                _teamEventListeners.Remove(teamEventListener);
        }


        public void ManualSubscribe(ITempoEntityStatesListener tempoEntityListener)
        {
            _tempoEntityListeners.Add(tempoEntityListener);
        }

        public void ManualSubscribe(ITempoEntityStatesExtraListener tempoEntityExtraListener)
        {
            _tempoEntityExtraListeners.Add(tempoEntityExtraListener);
        }

        public void ManualSubscribe(ITempoDedicatedEntityStatesListener tempoEntityListener)
        {
            _tempoDedicatedEntitiesListeners.Add(tempoEntityListener);
        }
        public void ManualSubscribe(ITempoTeamStatesListener teamStatesListener)
        {
            _tempoTeamListeners.Add(teamStatesListener);
        }

        public void ManualSubscribe(ISkillUsageListener skillUsageListener)
        {
            _skillUsageListeners.Add(skillUsageListener);
        }

        public void ManualSubscribe(ITeamEventListener teamEventListener)
        {
            _teamEventListeners.Add(teamEventListener);
        }

        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityRequestSequence(entity,canControl);
            }
        }
        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in _tempoDedicatedEntitiesListeners)
            {
                listener.OnTrinityEntityRequestSequence(entity, canAct);
            }
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in _tempoDedicatedEntitiesListeners)
            {
                listener.OnOffEntityRequestSequence(entity,canAct);
            }
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            foreach (var listener in _tempoDedicatedEntitiesListeners)
            {
                listener.OnTrinityEntityFinishSequence(entity);
            }
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            foreach (var listener in _tempoDedicatedEntitiesListeners)
            {
                listener.OnOffEntityFinishSequence(entity);
            }
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityRequestAction(entity);
            }
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityBeforeSkill(entity);
            }
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishAction(entity);
            }
        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityEmptyActions(entity);
            }
        }

        public void OnEntityFinishSequence(CombatEntity entity, bool isForcedByController)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishSequence(entity,isForcedByController);
            }
        }

        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnTempoPreStartControl(controller);
            }
        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnAllActorsNoActions(lastActor);
            }
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnControlFinishAllActors(lastActor);
            }
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnTempoFinishControl(controller);
            }

        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnTempoFinishLastCall(controller);
            }
        }


        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnCombatSkillSubmit(in values);
            }
        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnCombatSkillPerform(in values);
            }
        }

        public void OnCombatPrimaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            foreach (var listener in _effectUsageListeners)
            {
                listener.OnCombatPrimaryEffectPerform(performer,target,in values);
            }
        }

        public void OnCombatSecondaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            foreach (var listener in _effectUsageListeners)
            {
                listener.OnCombatSecondaryEffectPerform(performer, target, in values);
            }
        }

        public void OnCombatVanguardEffectPerform(CombatEntity performer, CombatEntity target,
            in PerformEffectValues values)
        {
            foreach (var listener in _effectUsageListeners)
            {
                listener.OnCombatVanguardEffectPerform(performer,target,in values);
            }
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnCombatSkillFinish(performer);
            }
        }

        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
            foreach (var listener in _teamEventListeners)
            {
                listener.OnStanceChange(team, switchedStance);
            }
        }

        public void OnControlChange(CombatTeam team, float phasedControl, bool isBurst)
        {
            foreach (var listener in _teamEventListeners)
            {
                listener.OnControlChange(team, phasedControl, isBurst);
            }
        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityExtraListeners)
            {
                listener.OnAfterEntityRequestSequence(entity);
            }
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityExtraListeners)
            {
                listener.OnAfterEntitySequenceFinish(entity);
            }
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityExtraListeners)
            {
                listener.OnNoActionsForcedFinish(entity);
            }
        }

        public void OnVanguardEffectSubscribe(in VanguardSkillAccumulation values)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardEffectSubscribe(in values);
            }
        }

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardEffectIncrement(type,attacker);
            }
        }

        public void OnVanguardEffectPerform(VanguardSkillUsageValues values)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardEffectPerform(values);
            }
        }
    }

    public interface ICombatEventsHolderBase : ITempoEntityStatesListener, ITempoDedicatedEntityStatesListener, ITempoEntityStatesExtraListener,
        ITempoTeamStatesListener,
        ISkillUsageListener, IEffectUsageListener,
        IVanguardEffectUsageListener,
        ITeamEventListener
    {

    }

    public interface ICombatEventsHolder : ICombatEventsHolderBase,
        ICombatPreparationListener, ICombatStatesListener,
        ICombatEntityExistenceListener, 
        IDamageDoneListener, IVitalityChangeListener, IRecoveryDoneListener
    {
        void Subscribe(ICombatEventListener listener);
        void UnSubscribe(ICombatEventListener listener);
    }


    public interface ICombatEntityExistenceListener : ICombatEventListener
    {
        void OnCreateEntity(in CombatEntity entity, in bool isPlayers);
        void OnDestroyEntity(in CombatEntity entity, in bool isPlayers);
    }


    /// <summary>
    /// EventsHolder that will be summon if the associated [<see cref="CombatEntity"/>] belongs to them
    /// </summary>
    public interface IDiscriminationEventsHolder
    {
        CombatEntityEventsHolder GetDiscriminationEventsHolder();
    }


    /// <summary>
    /// An eventListener interface to extend from (others eventListeners types should extend from this to
    /// be assorted correctly)
    /// </summary>
    public interface ICombatEventListener
    {

    }
}
