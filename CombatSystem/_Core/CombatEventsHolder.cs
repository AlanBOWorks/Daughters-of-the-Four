using System;
using System.Collections.Generic;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem._Core
{
    public sealed class SystemCombatEventsHolder : ICombatEventsHolder,ITempoEntityPercentListener
        //: ICombatEventsHolder
    {
        public SystemCombatEventsHolder() 
        {
            _eventsHolder = new SystemEventsHolder();

#if UNITY_EDITOR
            var debugEvents = new DebugEvents();

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
        private CombatEntityEventsHolder _currentDiscriminatedEntityEventsHolder;




        public void SubscribeEventsHandler(PlayerCombatEventsHolder eventsHandler)
        {
            _playerCombatEvents = eventsHandler;
        }

        public void SubscribeEventsHandler(EnemyCombatEventsHolder eventsHandler)
        {
            _enemyCombatEvents = eventsHandler;
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

        public void OnCombatFinish(bool isPlayerWin)
        {
            _eventsHolder.OnCombatFinish(isPlayerWin);
            _playerCombatEvents.OnCombatFinish(isPlayerWin);
            _enemyCombatEvents.OnCombatFinish(isPlayerWin);
        }

        public void OnCombatQuit()
        {
            _eventsHolder.OnCombatQuit();
            _playerCombatEvents.OnCombatQuit();
            _enemyCombatEvents.OnCombatQuit();
        }


        // ------ TEMPO ----- 

        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            _playerCombatEvents.OnEntityTick(in entity, in currentInitiative, in percentInitiative);
            _enemyCombatEvents.OnEntityTick(in entity, in currentInitiative, in percentInitiative);
        }
        
        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _eventsHolder.OnMainEntityRequestSequence(entity,canAct);
            _playerCombatEvents.OnMainEntityRequestSequence(entity, canAct);
            _enemyCombatEvents.OnMainEntityRequestSequence(entity, canAct);

            _currentDiscriminatedEntityEventsHolder.OnMainEntityRequestSequence(entity,canAct);
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            _eventsHolder.OnOffEntityRequestSequence(entity, canAct);
            _playerCombatEvents.OnOffEntityRequestSequence(entity, canAct);
            _enemyCombatEvents.OnOffEntityRequestSequence(entity, canAct);

            _currentDiscriminatedEntityEventsHolder.OnOffEntityRequestSequence(entity, canAct);
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            _eventsHolder.OnEntityRequestAction(entity);
            _playerCombatEvents.OnEntityRequestAction(entity);
            _enemyCombatEvents.OnEntityRequestAction(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityRequestAction(entity);
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishAction(entity);
            _playerCombatEvents.OnEntityFinishAction(entity);
            _enemyCombatEvents.OnEntityFinishAction(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishAction(entity);
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            _eventsHolder.OnEntityFinishSequence(entity);
            _playerCombatEvents.OnEntityFinishSequence(entity);
            _enemyCombatEvents.OnEntityFinishSequence(entity);

            _currentDiscriminatedEntityEventsHolder.OnEntityFinishSequence(entity);
        }


        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
            HandleCurrentEntityEventsHolder(in controller);


            _eventsHolder.OnTempoStartControl(in controller);
            _playerCombatEvents.OnTempoStartControl(in controller);
            _enemyCombatEvents.OnTempoStartControl(in controller);

            _currentDiscriminatedEntityEventsHolder.OnTempoStartControl(in controller);
        }
        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            _eventsHolder.OnTempoFinishControl(in controller);
            _playerCombatEvents.OnTempoFinishControl(in controller);
            _enemyCombatEvents.OnTempoFinishControl(in controller);

            _currentDiscriminatedEntityEventsHolder.OnTempoFinishControl(in controller);
        }


        // ------ SKILLS ----- 

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill,in CombatEntity target)
        {
            _eventsHolder.OnSkillSubmit(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillSubmit(in performer, in usedSkill, in target);
            _enemyCombatEvents.OnSkillSubmit(in performer, in usedSkill, in target);

            _currentDiscriminatedEntityEventsHolder.OnSkillSubmit(in performer, in usedSkill, target);

            // Manual sequence Flow
            OnSkillPerform(in performer,in usedSkill, in target);
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            _eventsHolder.OnSkillPerform(in performer, in usedSkill, in target);
            _playerCombatEvents.OnSkillPerform(in performer, in usedSkill, in target);
            _enemyCombatEvents.OnSkillPerform(in performer, in usedSkill, in target);

            _currentDiscriminatedEntityEventsHolder.OnSkillPerform(in performer, in usedSkill, in target);

            //todo wait until skill ends (animation)
            OnSkillFinish();
            OnEntityFinishAction(performer);
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            _eventsHolder.OnEffectPerform(in performer,in usedSkill,in target, in effect);
            _playerCombatEvents.OnEffectPerform(in performer,in usedSkill,in target, in effect);
            _enemyCombatEvents.OnEffectPerform(in performer,in usedSkill,in target, in effect);

            _currentDiscriminatedEntityEventsHolder.OnEffectPerform(in performer, in usedSkill, target, effect);
        }

        public void OnSkillFinish()
        {
            _eventsHolder.OnSkillFinish();
            _playerCombatEvents.OnSkillFinish();
            _enemyCombatEvents.OnSkillFinish();
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
        
        public void OnDamageDone(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            _eventsHolder.OnDamageDone(in target, in performer, in amount);
            _playerCombatEvents.OnDamageDone(in target, in performer, in amount);
            _enemyCombatEvents.OnDamageDone(in target, in performer, in amount);
        }


        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            _eventsHolder.OnShieldLost(in target, in performer, in amount);
            _playerCombatEvents.OnShieldLost(in target, in performer, in amount);
            _enemyCombatEvents.OnShieldLost(in target, in performer, in amount);
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            _eventsHolder.OnHealthLost(in target, in performer, in amount);
            _playerCombatEvents.OnHealthLost(in target, in performer, in amount);
            _enemyCombatEvents.OnHealthLost(in target, in performer, in amount);
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            _eventsHolder.OnMortalityLost(in target, in performer, in amount);
            _playerCombatEvents.OnMortalityLost(in target, in performer, in amount);
            _enemyCombatEvents.OnMortalityLost(in target, in performer, in amount);
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
            _eventsHolder.OnKnockOut(in target, in performer);
            _playerCombatEvents.OnKnockOut(in target, in performer);
            _enemyCombatEvents.OnKnockOut(in target, in performer);
        }

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            _eventsHolder.OnStanceChange(in team, in switchedStance);
            _playerCombatEvents.OnStanceChange(in team, in switchedStance);
            _enemyCombatEvents.OnStanceChange(in team, in switchedStance);
        }

        public void OnControlChange(in CombatTeam team, in float phasedControl, in bool isBurst)
        {
            _eventsHolder.OnControlChange(in team, in phasedControl,in isBurst);
            _playerCombatEvents.OnControlChange(in team, in phasedControl, in isBurst);
            _enemyCombatEvents.OnControlChange(in team, in phasedControl, in isBurst);
        }



        private sealed class SystemEventsHolder : CombatEventsHolder
        {
            public SystemEventsHolder()
            {
                _tempoTicker = new TempoTicker();

                CombatSystemSingleton.TempoTicker = _tempoTicker;
                Subscribe(_tempoTicker);
                Subscribe(_tempoTicker.EntitiesTempoTicker);

                var skillsUsageEventHandler = new CombatSkillEventHandler();
                Subscribe(skillsUsageEventHandler);
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
        }

#if UNITY_EDITOR
        #region -- DEBUG --
        private sealed class DebugEvents : ITempoEntityStatesListener, ITempoTickListener, ISkillUsageListener
        {
            public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
            {
                Debug.Log($"> --- Entity MAIN Sequence: {entity.GetProviderEntityName()}");
            }

            public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
            {
                Debug.Log($"> --- Entity OFF Sequence: {entity.GetProviderEntityName()}");
            }

            public void OnEntityRequestAction(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [Request ACTION]: {entity.GetProviderEntityName()}");
            }

            public void OnEntityFinishAction(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [End ACTION]: {entity.GetProviderEntityName()}");
            }

            public void OnEntityFinishSequence(CombatEntity entity)
            {
                Debug.Log($"> --- Entity [END] Sequence: {entity.GetProviderEntityName()}");
            }

            public void OnTempoFinishControl(CombatEntity mainEntity)
            {
                Debug.Log($"> --- Entity [WAIT] Sequence: {mainEntity.GetProviderEntityName()}");
            }

            public void OnStartTicking()
            {
                Debug.Log("xx - START Ticking");
            }

            public void OnTick()
            {
            }

            public void OnRoundPassed()
            {
                Debug.Log("xx - Round Passed");
            }

            public void OnStopTicking()
            {
                Debug.Log("xx - STOP Ticking");
            }

            public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
            {

                Debug.Log($"----------------- SKILL --------------- \n" +
                          $"Random Controller: {performer.GetProviderEntityName()} / " +
                          $"Used : {usedSkill.Preset} /" +
                          $"Target: {target.GetProviderEntityName()}");
                Debug.Log($"ACTIONS Used: {performer.Stats.UsedActions}");
            }

            public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
            {
                Debug.Log($"Performing ----- > {usedSkill.GetSkillName()}");
            }

            public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
            {
                Debug.Log($"Effect performed  {performer.GetProviderEntityName()} / On target: {target.GetProviderEntityName()} ");
            }

            public void OnSkillFinish()
            {
                Debug.Log("-------------- SKILL END --------------- ");
            }
        }
        #endregion
#endif

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
        public void OnEntityTick(in CombatEntity entity, in float currentInitiative, in float percentInitiative)
        {
            foreach (var listener in _tempoEntityPercentListeners)
            {
                listener.OnEntityTick(in entity, in currentInitiative, in percentInitiative);
            }
        }

        public CombatEntityEventsHolder GetDiscriminationEventsHolder() => DiscriminationEventsHolder;
    }

    public abstract class CombatEventsHolder : CombatEntityEventsHolder, ICombatEventsHolder

    {
        protected CombatEventsHolder() : base()
        {
            _combatPreparationListeners = new HashSet<ICombatPreparationListener>();
            _combatStatesListeners = new HashSet<ICombatStatesListener>();
            _entitiesExistenceListeners = new HashSet<ICombatEntityExistenceListener>();
            _damageDoneListeners = new HashSet<IDamageDoneListener>();
            _vitalityChangeListeners = new HashSet<IVitalityChangeListeners>();
        }



        [Title("Events")]
        [ShowInInspector]
        private readonly ICollection<ICombatPreparationListener> _combatPreparationListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatStatesListener> _combatStatesListeners;
        [ShowInInspector] 
        private readonly ICollection<ICombatEntityExistenceListener> _entitiesExistenceListeners;
        [ShowInInspector] 
        private readonly ICollection<IDamageDoneListener> _damageDoneListeners;

        [ShowInInspector] 
        private readonly ICollection<IVitalityChangeListeners> _vitalityChangeListeners;

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            if (listener is ICombatPreparationListener preparationListener)
                _combatPreparationListeners.Add(preparationListener);
            if(listener is ICombatStatesListener combatStatesListener)
                _combatStatesListeners.Add(combatStatesListener);

            if (listener is ITempoTickListener tickListener)
                SubscribeTempo(tickListener);

            if (listener is ICombatEntityExistenceListener entityExistenceListener)
                _entitiesExistenceListeners.Add(entityExistenceListener);

            if(listener is IDamageDoneListener damageDoneListener)
                _damageDoneListeners.Add(damageDoneListener);
            if(listener is IVitalityChangeListeners vitalityChangeListener)
                _vitalityChangeListeners.Add(vitalityChangeListener);

        }

        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is ICombatPreparationListener preparationListener)
                _combatPreparationListeners.Remove(preparationListener);
            if (listener is ICombatStatesListener combatStatesListener)
                _combatStatesListeners.Remove(combatStatesListener);

            if (listener is ITempoTickListener tickListener)
                UnSubscribeTempo(tickListener);


            if (listener is ICombatEntityExistenceListener entityExistenceListener)
                _entitiesExistenceListeners.Remove(entityExistenceListener);

            if (listener is IDamageDoneListener damageDoneListener)
                _damageDoneListeners.Remove(damageDoneListener);
            if (listener is IVitalityChangeListeners vitalityChangeListener)
                _vitalityChangeListeners.Add(vitalityChangeListener);
        }


        protected abstract void SubscribeTempo(ITempoTickListener tickListener);
        protected abstract void UnSubscribeTempo(ITempoTickListener tickListener);


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

        public void OnCombatFinish(bool isPlayerWin)
        {
            foreach (var listener in _combatStatesListeners)
            {
                listener.OnCombatFinish(isPlayerWin);
            }
        }

        public void OnCombatQuit()
        {
            foreach (var listener in _combatStatesListeners)
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

        public void OnDamageDone(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            foreach (var listener in _vitalityChangeListeners)
            {
                listener.OnDamageDone(in target, in performer, in amount);
            }
        }



        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnShieldLost(in target, in performer,in amount);
            }
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnHealthLost(in target, in performer, in amount);
            }

        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnMortalityLost(in target, in performer, in amount);
            }

        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
        {
            foreach (var listener in _damageDoneListeners)
            {
                listener.OnKnockOut(in target, in performer);
            }
        }

    }

    public class CombatEntityEventsHolder : ITempoEntityStatesListener, ITempoTeamStatesListener,
        ISkillUsageListener, ITeamEventListener
    {
        public CombatEntityEventsHolder()
        {
            _tempoEntityListeners = new HashSet<ITempoEntityStatesListener>();
            _tempoTeamListeners = new HashSet<ITempoTeamStatesListener>();

            _skillUsageListeners = new HashSet<ISkillUsageListener>();
            _teamEventListeners = new HashSet<ITeamEventListener>();
        }

        [ShowInInspector] private readonly ICollection<ITempoEntityStatesListener> _tempoEntityListeners;
        [ShowInInspector]
        private readonly ICollection<ITempoTeamStatesListener> _tempoTeamListeners;

        [ShowInInspector] private readonly ICollection<ISkillUsageListener> _skillUsageListeners;
        [ShowInInspector] private readonly ICollection<ITeamEventListener> _teamEventListeners;


        public virtual void Subscribe(ICombatEventListener listener)
        {
            if (listener == null)
                throw new ArgumentNullException(nameof(listener), "Event listener can't be Null");


            if (listener is ITempoEntityStatesListener tempoEntityListener)
                _tempoEntityListeners.Add(tempoEntityListener);
            if (listener is ITempoTeamStatesListener tempoTeamStatesListener)
                _tempoTeamListeners.Add(tempoTeamStatesListener);


            if (listener is ISkillUsageListener skillUsageListener)
                _skillUsageListeners.Add(skillUsageListener);


            if (listener is ITeamEventListener teamEventListener)
                _teamEventListeners.Add(teamEventListener);
        }

        public virtual void UnSubscribe(ICombatEventListener listener)
        {
            if (listener is ITempoEntityStatesListener tempoEntityListener)
                _tempoEntityListeners.Remove(tempoEntityListener);
            if (listener is ITempoTeamStatesListener tempoTeamStatesListener)
                _tempoTeamListeners.Remove(tempoTeamStatesListener);


            if (listener is ISkillUsageListener skillUsageListener)
                _skillUsageListeners.Remove(skillUsageListener);


            if (listener is ITeamEventListener teamEventListener)
                _teamEventListeners.Remove(teamEventListener);
        }


        public void ManualSubscribe(ITempoEntityStatesListener tempoEntityListener)
        {
            _tempoEntityListeners.Add(tempoEntityListener);
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


        public void OnMainEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnMainEntityRequestSequence(entity, canAct);
            }
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnOffEntityRequestSequence(entity,canAct);
            }
        }

        public void OnEntityRequestAction(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityRequestAction(entity);
            }
        }

        public void OnEntityFinishAction(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishAction(entity);
            }
        }

        public void OnEntityFinishSequence(CombatEntity entity)
        {
            foreach (var listener in _tempoEntityListeners)
            {
                listener.OnEntityFinishSequence(entity);
            }
        }

        public void OnTempoStartControl(in CombatTeamControllerBase controller)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnTempoStartControl(in controller);
            }
        }

        public void OnTempoFinishControl(in CombatTeamControllerBase controller)
        {
            foreach (var listener in _tempoTeamListeners)
            {
                listener.OnTempoFinishControl(in controller);
            }

        }

        public void OnSkillSubmit(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillSubmit(in performer, in usedSkill, in target);
            }
        }

        public void OnSkillPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillPerform(in performer, in usedSkill, in target);
            }
        }

        public void OnEffectPerform(in CombatEntity performer, in CombatSkill usedSkill, in CombatEntity target, in IEffect effect)
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnEffectPerform(in performer, in usedSkill, in target, in effect);
            }
        }

        public void OnSkillFinish()
        {
            foreach (var listener in _skillUsageListeners)
            {
                listener.OnSkillFinish();
            }
        }

        public void OnStanceChange(in CombatTeam team, in EnumTeam.StanceFull switchedStance)
        {
            foreach (var listener in _teamEventListeners)
            {
                listener.OnStanceChange(in team, in switchedStance);
            }
        }

        public void OnControlChange(in CombatTeam team, in float phasedControl,in bool isBurst)
        {
            foreach (var listener in _teamEventListeners)
            {
                listener.OnControlChange(in team, in phasedControl, in isBurst);
            }
        }

    }


    public interface ICombatEventsHolder : ICombatPreparationListener, ICombatStatesListener,
        ITempoEntityStatesListener, ITempoTeamStatesListener,
        ITeamEventListener,
        ICombatEntityExistenceListener, 
        ISkillUsageListener,
        IDamageDoneListener, IVitalityChangeListeners
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
