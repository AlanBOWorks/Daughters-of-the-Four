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
            _sequenceStepper = new TempoSequenceStepper();
            _entityEventHandler = new CombatEntityEventHandler();

            var entityTempoStepper = new EntityTempoStepper();
            _eventsHolder.Subscribe(entityTempoStepper);



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
        private readonly TempoSequenceStepper _sequenceStepper;


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

        private void HandleCurrentEntityEventsHolder(in CombatTeamControllerBase controller)
        {
            bool isPlayerElement = CombatSystemSingleton.TeamControllers.PlayerTeamType == controller;
            _currentDiscriminatedEntityEventsHolder = isPlayerElement 
                ? _playerCombatEvents.DiscriminationEventsHolder 
                : _enemyCombatEvents.DiscriminationEventsHolder;
        }
        private void HandleCurrentEntityEventsHolder(in CombatEntity entity)
        {
            bool isPlayerElement = UtilsTeam.IsPlayerTeam(in entity);
            _currentDiscriminatedEntityEventsHolder = isPlayerElement
                ? _playerCombatEvents.DiscriminationEventsHolder
                : _enemyCombatEvents.DiscriminationEventsHolder;
        }

        // ------ PREPARATIONS ----- 
        public void OnCombatPrepares(IReadOnlyCollection<CombatEntity> allMembers, CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _eventsHolder.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
            _playerCombatEvents.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
            _enemyCombatEvents.OnCombatPrepares(allMembers,playerTeam,enemyTeam);
        }

        public void OnCombatPreStarts(CombatTeam playerTeam, CombatTeam enemyTeam)
        {
            _eventsHolder.OnCombatPreStarts(playerTeam, enemyTeam);
            _playerCombatEvents.OnCombatPreStarts(playerTeam, enemyTeam);
            _enemyCombatEvents.OnCombatPreStarts(playerTeam, enemyTeam);
        }

        public void OnCombatStart()
        {
            _eventsHolder.OnCombatStart();
            _playerCombatEvents.OnCombatStart();
            _enemyCombatEvents.OnCombatStart();
        }

        public void OnCombatEnd()
        {
            _eventsHolder.OnCombatEnd();
            _playerCombatEvents.OnCombatEnd();
            _enemyCombatEvents.OnCombatEnd();
        }

        public void OnCombatFinish(bool isPlayerWin)
        {
            OnCombatEnd();

            _eventsHolder.OnCombatFinish(isPlayerWin);
            _playerCombatEvents.OnCombatFinish(isPlayerWin);
            _enemyCombatEvents.OnCombatFinish(isPlayerWin);
        }

        public void OnCombatQuit()
        {
            OnCombatEnd();

            _eventsHolder.OnCombatQuit();
            _playerCombatEvents.OnCombatQuit();
            _enemyCombatEvents.OnCombatQuit();
        }


        // ------ TEMPO ----- 

        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            _playerCombatEvents.OnEntityTick(in entity, in currentTick, in percentInitiative);
            _enemyCombatEvents.OnEntityTick(in entity, in currentTick, in percentInitiative);
        }


        public void OnEntityRequestSequence(CombatEntity entity, bool canControl)
        {
            _entityEventHandler.OnEntityRequestSequence(entity,canControl);

            HandleCurrentEntityEventsHolder(in entity);

            _eventsHolder.OnEntityRequestSequence(entity, canControl);
            _playerCombatEvents.OnEntityRequestSequence(entity, canControl);
            _enemyCombatEvents.OnEntityRequestSequence(entity, canControl);

            _currentDiscriminatedEntityEventsHolder.OnEntityRequestSequence(entity, canControl);

            _entityEventHandler.OnEntityRequestSequence(entity,canControl);
        }

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _eventsHolder.OnTrinityEntityRequestSequence(entity,canAct);
            _playerCombatEvents.OnTrinityEntityRequestSequence(entity, canAct);
            _enemyCombatEvents.OnTrinityEntityRequestSequence(entity, canAct);

            _currentDiscriminatedEntityEventsHolder.OnTrinityEntityRequestSequence(entity,canAct);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _eventsHolder.OnOffEntityRequestSequence(entity, canAct);
            _playerCombatEvents.OnOffEntityRequestSequence(entity, canAct);
            _enemyCombatEvents.OnOffEntityRequestSequence(entity, canAct);

            _currentDiscriminatedEntityEventsHolder.OnOffEntityRequestSequence(entity, canAct);
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
            _eventsHolder.OnTrinityEntityFinishSequence(entity);
            _playerCombatEvents.OnTrinityEntityFinishSequence(entity);
            _enemyCombatEvents.OnTrinityEntityFinishSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnTrinityEntityFinishSequence(entity);
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            _eventsHolder.OnOffEntityFinishSequence(entity);
            _playerCombatEvents.OnOffEntityFinishSequence(entity);
            _enemyCombatEvents.OnOffEntityFinishSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnOffEntityFinishSequence(entity);
        }


        public void OnEntityRequestAction(CombatEntity entity)
        {
            _entityEventHandler.OnEntityRequestAction(entity);

            _eventsHolder.OnEntityRequestAction(entity);
            _playerCombatEvents.OnEntityRequestAction(entity);
            _enemyCombatEvents.OnEntityRequestAction(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityRequestAction(entity);
        }

        public void OnEntityBeforeSkill(CombatEntity entity)
        {
            _eventsHolder.OnEntityBeforeSkill(entity);
            _playerCombatEvents.OnEntityBeforeSkill(entity);
            _enemyCombatEvents.OnEntityBeforeSkill(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityBeforeSkill(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            _entityEventHandler.OnEntityFinishAction(entity);

            _eventsHolder.OnEntityFinishAction(entity);
            _playerCombatEvents.OnEntityFinishAction(entity);
            _enemyCombatEvents.OnEntityFinishAction(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishAction(entity);

            _entityEventHandler.OnEntityAfterFinishAction(in entity);

        }

        public void OnEntityEmptyActions(CombatEntity entity)
        {
            _eventsHolder.OnEntityEmptyActions(entity);
            _playerCombatEvents.OnEntityEmptyActions(entity);
            _enemyCombatEvents.OnEntityEmptyActions(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityEmptyActions(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity, in bool isForcedByController)
        {
            _entityEventHandler.OnEntityFinishSequence(entity,in isForcedByController);

            _eventsHolder.OnEntityFinishSequence(entity,in isForcedByController);
            _playerCombatEvents.OnEntityFinishSequence(entity,in isForcedByController);
            _enemyCombatEvents.OnEntityFinishSequence(entity,in isForcedByController);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishSequence(entity,in isForcedByController);

            _entityEventHandler.OnEntityFinishSequence(entity, in isForcedByController);
        }

        public void OnAfterEntityRequestSequence(CombatEntity entity)
        {
            _eventsHolder.OnAfterEntityRequestSequence(entity);
            _playerCombatEvents.OnAfterEntityRequestSequence(entity);
            _enemyCombatEvents.OnAfterEntityRequestSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnAfterEntityRequestSequence(entity);

            _entityEventHandler.RequestEntityAction(in entity);
        }

        public void OnAfterEntitySequenceFinish(CombatEntity entity)
        {
            _eventsHolder.OnAfterEntitySequenceFinish(entity);
            _playerCombatEvents.OnAfterEntitySequenceFinish(entity);
            _enemyCombatEvents.OnAfterEntitySequenceFinish(entity);

            _currentDiscriminatedEntityEventsHolder.OnAfterEntitySequenceFinish(entity);

            _entityEventHandler.TryCallOnFinishAllActors(in entity);
        }

        public void OnNoActionsForcedFinish(CombatEntity entity)
        {
            _eventsHolder.OnNoActionsForcedFinish(entity);
            _playerCombatEvents.OnNoActionsForcedFinish(entity);
            _enemyCombatEvents.OnNoActionsForcedFinish(entity);

            _currentDiscriminatedEntityEventsHolder.OnNoActionsForcedFinish(entity);
        }


        public void OnTempoPreStartControl(CombatTeamControllerBase controller)
        {
            HandleCurrentEntityEventsHolder(in controller);


            _eventsHolder.OnTempoPreStartControl(controller);
            _playerCombatEvents.OnTempoPreStartControl(controller);
            _enemyCombatEvents.OnTempoPreStartControl(controller);

            _currentDiscriminatedEntityEventsHolder.OnTempoPreStartControl(controller);

        }

        public void OnAllActorsNoActions(CombatEntity lastActor)
        {
            _eventsHolder.OnAllActorsNoActions(lastActor);
            _playerCombatEvents.OnAllActorsNoActions(lastActor);
            _enemyCombatEvents.OnAllActorsNoActions(lastActor);

            _currentDiscriminatedEntityEventsHolder.OnAllActorsNoActions(lastActor);
        }

        public void OnControlFinishAllActors(CombatEntity lastActor)
        {
            _eventsHolder.OnControlFinishAllActors(lastActor);
            _playerCombatEvents.OnControlFinishAllActors(lastActor);
            _enemyCombatEvents.OnControlFinishAllActors(lastActor);

            _currentDiscriminatedEntityEventsHolder.OnControlFinishAllActors(lastActor);
        }

        public void OnTempoFinishControl(CombatTeamControllerBase controller)
        {
            _eventsHolder.OnTempoFinishControl(controller);
            _playerCombatEvents.OnTempoFinishControl(controller);
            _enemyCombatEvents.OnTempoFinishControl(controller);

            _currentDiscriminatedEntityEventsHolder.OnTempoFinishControl(controller);

            _sequenceStepper.OnTempoFinishControl(in controller);

            OnTempoFinishLastCall(controller);
        }

        public void OnTempoFinishLastCall(CombatTeamControllerBase controller)
        {
            _sequenceStepper.OnTempoFinishLastCall(in controller);

            _eventsHolder.OnTempoFinishLastCall(controller);
            _playerCombatEvents.OnTempoFinishLastCall(controller);
            _enemyCombatEvents.OnTempoFinishLastCall(controller);

            _currentDiscriminatedEntityEventsHolder.OnTempoFinishLastCall(controller);
        }


        // ------ SKILLS ----- 

        public void OnCombatSkillSubmit(in SkillUsageValues values)
        {
            var performer = values.Performer;
            var usedSkill = values.UsedSkill;

            _entityEventHandler.OnCombatSkillPreSubmit(in performer, in usedSkill);

            _eventsHolder.OnCombatSkillSubmit(in values);
            _playerCombatEvents.OnCombatSkillSubmit(in values);
            _enemyCombatEvents.OnCombatSkillSubmit(in values);

            _currentDiscriminatedEntityEventsHolder.OnCombatSkillSubmit(in values);

            _entityEventHandler.OnCombatSkillSubmit(in performer, in usedSkill);

        }

        public void OnCombatSkillPerform(in SkillUsageValues values)
        {
            values.Extract(out var performer,out var target,out var usedSkill);
            _entityEventHandler.OnCombatSkillPerform(in performer, in usedSkill, in target);

            _eventsHolder.OnCombatSkillPerform(in values);
            _playerCombatEvents.OnCombatSkillPerform(in values);
            _enemyCombatEvents.OnCombatSkillPerform(in values);

            _currentDiscriminatedEntityEventsHolder.OnCombatSkillPerform(in values);

        }

        public void OnCombatPrimaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            _eventsHolder.OnCombatPrimaryEffectPerform(performer, target, in values);
            _playerCombatEvents.OnCombatPrimaryEffectPerform(performer, target, in values);
            _enemyCombatEvents.OnCombatPrimaryEffectPerform(performer, target, in values);

            _currentDiscriminatedEntityEventsHolder.OnCombatPrimaryEffectPerform(performer, target, values);
        }

        public void OnCombatSecondaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            _eventsHolder.OnCombatSecondaryEffectPerform(performer,target, in values);
            _playerCombatEvents.OnCombatSecondaryEffectPerform(performer,target, in values);
            _enemyCombatEvents.OnCombatSecondaryEffectPerform(performer,target, in values);

            _currentDiscriminatedEntityEventsHolder.OnCombatSecondaryEffectPerform(performer, target, values);
        }

        public void OnCombatSkillFinish(CombatEntity performer)
        {
            _eventsHolder.OnCombatSkillFinish(performer);
            _playerCombatEvents.OnCombatSkillFinish(performer);
            _enemyCombatEvents.OnCombatSkillFinish(performer);

            _currentDiscriminatedEntityEventsHolder.OnCombatSkillFinish(performer);

            OnEntityFinishAction(performer);
        }


        public void OnCreateEntity(in CombatEntity entity, in bool isPlayers)
        {
            _eventsHolder.OnCreateEntity(in entity, in isPlayers);
            _playerCombatEvents.OnCreateEntity(in entity, in isPlayers);
            _enemyCombatEvents.OnCreateEntity(in entity, in isPlayers);
        }

        public void OnDestroyEntity(in CombatEntity entity, in bool isPlayers)
        {
            _eventsHolder.OnDestroyEntity(in entity, in isPlayers);
            _playerCombatEvents.OnDestroyEntity(in entity, in isPlayers);
            _enemyCombatEvents.OnDestroyEntity(in entity, in isPlayers);
        }
        
        public void OnDamageBeforeDone(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnDamageBeforeDone(in performer, in target, in amount);
            _playerCombatEvents.OnDamageBeforeDone(in performer, in target, in amount);
            _enemyCombatEvents.OnDamageBeforeDone(in performer, in target, in amount);
        }

        public void OnRevive(in CombatEntity entity, bool isHealRevive)
        {
            _eventsHolder.OnRevive(in entity, isHealRevive);
            _playerCombatEvents.OnRevive(in entity, isHealRevive);
            _enemyCombatEvents.OnRevive(in entity, isHealRevive);
        }


        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnShieldLost(in performer, in target, in amount);
            _playerCombatEvents.OnShieldLost(in performer, in target, in amount);
            _enemyCombatEvents.OnShieldLost(in performer, in target, in amount);
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnHealthLost(in performer, in target, in amount);
            _playerCombatEvents.OnHealthLost(in performer, in target, in amount);
            _enemyCombatEvents.OnHealthLost(in performer, in target, in amount);
        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnMortalityLost(in performer, in target, in amount);
            _playerCombatEvents.OnMortalityLost(in performer, in target, in amount);
            _enemyCombatEvents.OnMortalityLost(in performer, in target, in amount);
        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            _eventsHolder.OnDamageReceive(in performer, in target);
            _playerCombatEvents.OnDamageReceive(in performer, in target);
            _enemyCombatEvents.OnDamageReceive(in performer, in target);
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            _eventsHolder.OnKnockOut(in performer, in target);
            _playerCombatEvents.OnKnockOut(in performer, in target);
            _enemyCombatEvents.OnKnockOut(in performer, in target);
        }

        public void OnShieldGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnShieldGain(in performer, in target, in amount);
            _playerCombatEvents.OnShieldGain(in performer, in target, in amount);
            _enemyCombatEvents.OnShieldGain(in performer, in target, in amount);
        }

        public void OnHealthGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnHealthGain(in performer, in target, in amount);
            _playerCombatEvents.OnHealthGain(in performer, in target, in amount);
            _enemyCombatEvents.OnHealthGain(in performer, in target, in amount);
        }

        public void OnMortalityGain(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            _eventsHolder.OnMortalityGain(in performer, in target, in amount);
            _playerCombatEvents.OnMortalityGain(in performer, in target, in amount);
            _enemyCombatEvents.OnMortalityGain(in performer, in target, in amount);
        }

        public void OnRecoveryReceive(in CombatEntity performer, in CombatEntity target)
        {
            _eventsHolder.OnRecoveryReceive(in performer, in target);
            _playerCombatEvents.OnRecoveryReceive(in performer, in target);
            _enemyCombatEvents.OnRecoveryReceive(in performer, in target);
        }

        public void OnKnockHeal(in CombatEntity performer, in CombatEntity target, in int currentAmount, in int amount)
        {
            _eventsHolder.OnKnockHeal(in performer, in target, in currentAmount, in amount);
            _playerCombatEvents.OnKnockHeal(in performer, in target, in currentAmount, in amount);
            _enemyCombatEvents.OnKnockHeal(in performer, in target, in currentAmount, in amount);
        }


        public void OnStanceChange(CombatTeam team, EnumTeam.StanceFull switchedStance)
        {
            _eventsHolder.OnStanceChange(team, switchedStance);
            _playerCombatEvents.OnStanceChange(team, switchedStance);
            _enemyCombatEvents.OnStanceChange(team, switchedStance);
        }

        public void OnControlChange(CombatTeam team, float phasedControl, bool isBurst)
        {
            _eventsHolder.OnControlChange(team, phasedControl,isBurst);
            _playerCombatEvents.OnControlChange(team, phasedControl, isBurst);
            _enemyCombatEvents.OnControlChange(team, phasedControl, isBurst);
        }


        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
            _eventsHolder.OnVanguardEffectIncrement(type,attacker);
            _playerCombatEvents.OnVanguardEffectIncrement(type,attacker);
            _enemyCombatEvents.OnVanguardEffectIncrement(type,attacker);

            _currentDiscriminatedEntityEventsHolder.OnVanguardEffectIncrement(type,attacker);
        }

        public void OnVanguardRevengeEffectPerform(IVanguardSkill skill, int iterations)
        {
            _eventsHolder.OnVanguardRevengeEffectPerform(skill, iterations);
            _playerCombatEvents.OnVanguardRevengeEffectPerform(skill, iterations);
            _enemyCombatEvents.OnVanguardRevengeEffectPerform(skill, iterations);

            _currentDiscriminatedEntityEventsHolder.OnVanguardRevengeEffectPerform(skill, iterations);
        }

        public void OnVanguardPunishEffectPerform(IVanguardSkill skill, int iterations)
        {
            _eventsHolder.OnVanguardPunishEffectPerform(skill, iterations);
            _playerCombatEvents.OnVanguardPunishEffectPerform(skill, iterations);
            _enemyCombatEvents.OnVanguardPunishEffectPerform(skill, iterations);

            _currentDiscriminatedEntityEventsHolder.OnVanguardPunishEffectPerform(skill, iterations);
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
        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            foreach (var listener in _tempoEntityPercentListeners)
            {
                listener.OnEntityTick(in entity, in currentTick, in percentInitiative);
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

        public void OnEntityFinishSequence(CombatEntity entity,in bool isForcedByController)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishSequence(entity,in isForcedByController);
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

        public void OnVanguardEffectIncrement(EnumsVanguardEffects.VanguardEffectType type, CombatEntity attacker)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardEffectIncrement(type,attacker);
            }
        }

        public void OnVanguardRevengeEffectPerform(IVanguardSkill skill, int iterations)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardRevengeEffectPerform(skill,iterations);
            }
        }

        public void OnVanguardPunishEffectPerform(IVanguardSkill skill, int iterations)
        {
            foreach (var listener in _vanguardEffectUsageListeners)
            {
                listener.OnVanguardPunishEffectPerform(skill,iterations);
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
