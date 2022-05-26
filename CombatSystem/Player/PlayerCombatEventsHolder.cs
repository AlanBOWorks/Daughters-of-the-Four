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

        [ShowInInspector,HorizontalGroup("Skills")]
        private readonly HashSet<ISkillPointerListener> _skillPointerListeners;
        [ShowInInspector,HorizontalGroup("Skills")]
        private readonly HashSet<ISkillSelectionListener> _skillSelectionListeners;
        [ShowInInspector,HorizontalGroup("Target")]
        private readonly HashSet<ITargetPointerListener> _targetPointerListeners;
        [ShowInInspector,HorizontalGroup("Target")]
        private readonly HashSet<ITargetSelectionListener> _targetSelectionListeners;
        [ShowInInspector, HorizontalGroup("Target")]
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

        public void OnSkillSelect(in CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSelect(in skill);
            }
        }

        public void OnSkillSelectFromNull(in CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSelectFromNull(in skill);
            }
        }

        public void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSwitch(in skill, in previousSelection);
            }
        }

        public void OnSkillDeselect(in CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillDeselect(in skill);
            }
        }

        public void OnSkillCancel(in CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillCancel(in skill);
            }
        }

        public void OnSkillSubmit(in CombatSkill skill)
        {
            foreach (var listener in _skillSelectionListeners)
            {
                listener.OnSkillSubmit(in skill);
            }

            OnHoverTargetExit();
        }


        // TARGET Events
        public void OnTargetButtonHover(in CombatEntity target)
        {
            foreach (var listener in _targetPointerListeners)
            {
                listener.OnTargetButtonHover(in target);
            }
        }

        public void OnTargetButtonExit(in CombatEntity target)
        {
            foreach (var listener in _targetPointerListeners)
            {
                listener.OnTargetButtonExit(in target);
            }
            OnHoverTargetExit();
        }


        public void OnTargetSelect(in CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetSelect(in target);
            }
        }

        public void OnTargetCancel(in CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetCancel(in target);
            }
        }

        public void OnTargetSubmit(in CombatEntity target)
        {
            foreach (var listener in _targetSelectionListeners)
            {
                listener.OnTargetSubmit(in target);
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
