using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Player.UI;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerCombatEventsHolder : ControllerCombatEventsHolder, ITempoTickListener, IDiscriminationEventsHolder,
        ICombatPauseListener,
        IPlayerEntityListener,

        ISkillPointerListener, ISkillSelectionListener,
        ISkillTooltipListener,

        ITargetPointerListener, ITargetSelectionListener, IHoverInteractionTargetsListener,
        ICameraHolderListener
    {
        public PlayerCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);

            _pauseListeners = new HashSet<ICombatPauseListener>();
            _playerEntityListeners = new HashSet<IPlayerEntityListener>();

            _skillPointerListeners = new HashSet<ISkillPointerListener>();
            _skillSelectionListeners = new HashSet<ISkillSelectionListener>();
            _skillTooltipListeners = new HashSet<ISkillTooltipListener>();

            _targetPointerListeners = new HashSet<ITargetPointerListener>();
            _targetSelectionListeners = new HashSet<ITargetSelectionListener>();
            _hoverTargetsListeners = new HashSet<IHoverInteractionTargetsListener>();

            _cameraHolderListeners = new HashSet<ICameraHolderListener>();

#if UNITY_EDITOR
            Subscribe(CombatDebuggerSingleton.CombatPlayerEventsLogs);
#endif

        }

        [ShowInInspector] 
        private readonly HashSet<ICombatPauseListener> _pauseListeners;
        [ShowInInspector]
        private readonly HashSet<IPlayerEntityListener> _playerEntityListeners;

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
        private readonly HashSet<IHoverInteractionTargetsListener> _hoverTargetsListeners;

        [ShowInInspector] 
        private readonly HashSet<ICameraHolderListener> _cameraHolderListeners;

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            SubscribeAsPlayerEvent(listener);
        }
        
        /// <summary>
        /// Check and subscribe as a: 
        /// <br></br>- <see cref="IPlayerEntityListener"/>
        /// <br></br>- <see cref="ISkillPointerListener"/>
        /// <br></br>- <see cref="ISkillSelectionListener"/>
        /// <br></br>- <see cref="ITargetPointerListener"/>
        /// <br></br>- <see cref="ITargetSelectionListener"/>
        /// <br></br>- <see cref="IHoverInteractionTargetsListener"/>
        /// <br></br>- <see cref="ICameraHolderListener"/>
        /// </summary>
        public void SubscribeAsPlayerEvent(ICombatEventListener listener)
        {
            if (listener is ICombatPauseListener pauseListener)
                _pauseListeners.Add(pauseListener);
            if (listener is IPlayerEntityListener playerEntityListener)
                _playerEntityListeners.Add(playerEntityListener);

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
            if (listener is IHoverInteractionTargetsListener hoverTargetsListener)
                _hoverTargetsListeners.Add(hoverTargetsListener);


            if (listener is ICameraHolderListener cameraHolderListener)
                _cameraHolderListeners.Add(cameraHolderListener);
        }
        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);

            if (listener is ICombatPauseListener pauseListener)
                _pauseListeners.Remove(pauseListener);

            if (listener is IPlayerEntityListener playerEntityListener)
                _playerEntityListeners.Remove(playerEntityListener);

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
            if (listener is IHoverInteractionTargetsListener hoverTargetsListener)
                _hoverTargetsListeners.Remove(hoverTargetsListener);

            if (listener is ICameraHolderListener cameraHolderListener)
                _cameraHolderListeners.Remove(cameraHolderListener);
        }


        internal void ManualSubscribe(IPlayerEntityListener playerEntityListener)
        {
            _playerEntityListeners.Add(playerEntityListener);
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

        internal void ManualSubscribe(ICameraHolderListener cameraHolderListener)
        {
            _cameraHolderListeners.Add(cameraHolderListener);
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

        public void OnSwitchMainCamera(in Camera combatCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchMainCamera(in combatCamera);
            }
        }

        public void OnSwitchBackCamera(in Camera combatBackCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchBackCamera(in combatBackCamera);
            }
        }

        public void OnSwitchFrontCamera(in Camera combatFrontCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchFrontCamera(in combatFrontCamera);
            }
        }

        public void OnPerformerSwitch(in CombatEntity performer)
        {
            foreach (var listener in _playerEntityListeners)
            {
                listener.OnPerformerSwitch(in performer);
            }
        }



        // SKILL Events
        public void OnSkillButtonHover(in CombatSkill skill)
        {
            foreach (var listener in _skillPointerListeners)
            {
                listener.OnSkillButtonHover(in skill);
            }
        }

        public void OnSkillButtonExit(in CombatSkill skill)
        {
            foreach (var listener in _skillPointerListeners)
            {
                listener.OnSkillButtonExit(in skill);
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

            OnHoverTargetExit();
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

        public void OnHoverTargetInteraction(in CombatEntity target)
        {
            foreach (var listener in _hoverTargetsListeners)
            {
                listener.OnHoverTargetInteraction(in target);
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
