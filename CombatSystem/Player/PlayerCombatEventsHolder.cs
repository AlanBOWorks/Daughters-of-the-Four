using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerCombatEventsHolder : ControllerCombatEventsHolder, 
        ITempoTickListener, IDiscriminationEventsHolder,
        ICombatPauseListener,
        IPlayerCombatEventListener,

        ISkillPointerListener, ISkillSelectionListener,
        ISkillTooltipListener,

        ITargetPointerListener, ITargetSelectionListener, IHoverInteractionEffectTargetsListener
    {
        public PlayerCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);

            _pauseListeners = new HashSet<ICombatPauseListener>();
            _playerCombatEventListeners = new HashSet<IPlayerCombatEventListener>();

            _skillPointerListeners = new HashSet<ISkillPointerListener>();
            _skillSelectionListeners = new HashSet<ISkillSelectionListener>();
            _skillTooltipListeners = new HashSet<ISkillTooltipListener>();

            _targetPointerListeners = new HashSet<ITargetPointerListener>();
            _targetSelectionListeners = new HashSet<ITargetSelectionListener>();
            _hoverTargetsListeners = new HashSet<IHoverInteractionEffectTargetsListener>();


#if UNITY_EDITOR
            Subscribe(CombatDebuggerSingleton.CombatPlayerEventsLogs);
#endif

        }

        [ShowInInspector] 
        private readonly HashSet<ICombatPauseListener> _pauseListeners;
        [ShowInInspector]
        private readonly HashSet<IPlayerCombatEventListener> _playerCombatEventListeners;

        [ShowInInspector, TitleGroup("Skills")]
        private readonly HashSet<ISkillPointerListener> _skillPointerListeners;
        [ShowInInspector,TitleGroup("Skills")]
        private readonly HashSet<ISkillSelectionListener> _skillSelectionListeners;
        [ShowInInspector, TitleGroup("Skills")]
        private readonly HashSet<ISkillTooltipListener> _skillTooltipListeners;

        [ShowInInspector, TitleGroup("Target")]
        private readonly HashSet<ITargetPointerListener> _targetPointerListeners;
        [ShowInInspector, TitleGroup("Target")]
        private readonly HashSet<ITargetSelectionListener> _targetSelectionListeners;
        [ShowInInspector, TitleGroup("Target")]
        private readonly HashSet<IHoverInteractionEffectTargetsListener> _hoverTargetsListeners;


        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            SubscribeAsPlayerEvent(listener);
        }
        
        /// <summary>
        /// Check and subscribe as a: 
        /// <br></br>- <see cref="IPlayerCombatEventListener"/>
        /// <br></br>- <see cref="ISkillPointerListener"/>
        /// <br></br>- <see cref="ISkillSelectionListener"/>
        /// <br></br>- <see cref="ITargetPointerListener"/>
        /// <br></br>- <see cref="ITargetSelectionListener"/>
        /// <br></br>- <see cref="IHoverInteractionEffectTargetsListener"/>
        /// </summary>
        public void SubscribeAsPlayerEvent(ICombatEventListener listener)
        {
            if (listener is ICombatPauseListener pauseListener)
                _pauseListeners.Add(pauseListener);
            if (listener is IPlayerCombatEventListener playerEntityListener)
                _playerCombatEventListeners.Add(playerEntityListener);

            if (listener is ISkillPointerListener skillPointerListener)
                _skillPointerListeners.Add(skillPointerListener);
            if (listener is ISkillSelectionListener skillSelectionListener)
                _skillSelectionListeners.Add(skillSelectionListener);
            if (listener is ISkillTooltipListener skillTooltipListener)
                _skillTooltipListeners.Add(skillTooltipListener);

            if (listener is ITargetPointerListener targetPointerListener)
                _targetPointerListeners.Add(targetPointerListener);
            if (listener is ITargetSelectionListener targetSelectionListener)
                _targetSelectionListeners.Add(targetSelectionListener);
            if (listener is IHoverInteractionEffectTargetsListener hoverTargetsListener)
                _hoverTargetsListeners.Add(hoverTargetsListener);
        }
        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is ICombatPauseListener pauseListener)
                _pauseListeners.Remove(pauseListener);

            if (listener is IPlayerCombatEventListener playerEntityListener)
                _playerCombatEventListeners.Remove(playerEntityListener);

            if (listener is ISkillPointerListener skillPointerListener)
                _skillPointerListeners.Remove(skillPointerListener);
            if (listener is ISkillSelectionListener skillSelectionListener)
                _skillSelectionListeners.Remove(skillSelectionListener);
            if (listener is ISkillTooltipListener skillTooltipListener)
                _skillTooltipListeners.Remove(skillTooltipListener);

            if (listener is ITargetPointerListener targetPointerListener)
                _targetPointerListeners.Remove(targetPointerListener);
            if (listener is ITargetSelectionListener targetSelectionListener)
                _targetSelectionListeners.Remove(targetSelectionListener);
            if (listener is IHoverInteractionEffectTargetsListener hoverTargetsListener)
                _hoverTargetsListeners.Remove(hoverTargetsListener);
        }


        internal void ManualSubscribe(IPlayerCombatEventListener playerCombatEventListener)
        {
            _playerCombatEventListeners.Add(playerCombatEventListener);
        }
        internal void ManualSubscribe(ISkillPointerListener skillPointerListener)
        {
            _skillPointerListeners.Add(skillPointerListener);
        }
        internal void ManualSubscribe(ISkillSelectionListener skillSelectionListener)
        {
            _skillSelectionListeners.Add(skillSelectionListener);
        }

        internal void ManualSubscribe(ITargetPointerListener targetPointerListener)
        {
            _targetPointerListeners.Add(targetPointerListener);
        }

        internal void ManualSubscribe(ITargetSelectionListener targetSelectionListener)
        {
            _targetSelectionListeners.Add(targetSelectionListener);
        }

        internal void ManualSubscribe(ICombatPauseListener pauseListener)
        {
            _pauseListeners.Add(pauseListener);
        }



        internal void ManualUnSubscribe(ICombatPauseListener pauseListener)
        {
            _pauseListeners.Remove(pauseListener);
        }


        public void OnCombatPause()
        {
            foreach (var listener in _pauseListeners)
            {
                listener.OnCombatPause();
            }
        }

        public void OnCombatResume()
        {
            foreach (var listener in _pauseListeners)
            {
                listener.OnCombatResume();
            }
        }


        public void OnPerformerSwitch(CombatEntity performer)
        {
            foreach (var listener in _playerCombatEventListeners)
            {
                listener.OnPerformerSwitch(performer);
            }
        }

        public void OnTeamStancePreviewSwitch(EnumTeam.StanceFull targetStance)
        {
            foreach (var listener in _playerCombatEventListeners)
            {
                listener.OnTeamStancePreviewSwitch(targetStance);
            }   
        }


        // SKILL Events
        public void OnSkillButtonHover(ICombatSkill skill)
        {
            foreach (var listener in _skillPointerListeners)
            {
                listener.OnSkillButtonHover(skill);
            }
        }

        public void OnSkillButtonExit(ICombatSkill skill)
        {
            foreach (var listener in _skillPointerListeners)
            {
                listener.OnSkillButtonExit(skill);
            }
        }

        public void OnSkillSelect(CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSelect(skill);
            }
        }

        public void OnSkillSelectFromNull(CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSelectFromNull(skill);
            }
        }

        public void OnSkillSwitch(CombatSkill skill, CombatSkill previousSelection)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSwitch(skill, previousSelection);
            }
        }

        public void OnSkillDeselect(CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillDeselect(skill);
            }
        }

        public void OnSkillCancel(CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillCancel(skill);
            }
        }

        public void OnSkillSubmit(CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSubmit(skill);
            }
        }
        public void OnTooltipEffect(in PerformEffectValues values)
        {
            foreach (var listener in _skillTooltipListeners)
            {
                listener.OnTooltipEffect(in values);
            }
        }

        public void OnToolTipOffensiveEffect(in PerformEffectValues values)
        {
            foreach (var listener in _skillTooltipListeners)
            {
                listener.OnToolTipOffensiveEffect(in values);
            }
        }

        public void OnTooltipSupportEffect(in PerformEffectValues values)
        {
            foreach (var listener in _skillTooltipListeners)
            {
                listener.OnTooltipSupportEffect(in values);
            }
        }

        public void OnTooltipTeamEffect(in PerformEffectValues values)
        {
            foreach (var listener in _skillTooltipListeners)
            {
                listener.OnTooltipTeamEffect(in values);
            }
        }

        public void OnFinishPoolEffects()
        {
            foreach (var listener in _skillTooltipListeners)
            {
                listener.OnFinishPoolEffects();
            }
        }


        // TARGET Events
        public void OnTargetButtonHover(CombatEntity target)
        {
            foreach (var listener in _targetPointerListeners)
            {
                listener.OnTargetButtonHover(target);
            }
        }

        public void OnTargetButtonExit(CombatEntity target)
        {
            foreach (var listener in _targetPointerListeners)
            {
                listener.OnTargetButtonExit(target);
            }
            OnHoverTargetExit();
        }


        public void OnTargetSelect(CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetSelect(target);
            }
        }

        public void OnTargetCancel(CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetCancel(target);
            }
        }

        public void OnTargetSubmit(CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetSubmit(target);
            }
        }

        public void OnHoverTargetInteraction(CombatEntity target, ISkill skill)
        {
            foreach (var listener in _hoverTargetsListeners)
            {
                listener.OnHoverTargetInteraction(target, skill);
            }
        }

        public void OnHoverTargetExit()
        {
            foreach (var listener in _hoverTargetsListeners)
            {
                listener.OnHoverTargetExit();
            }
        }

    }
}
