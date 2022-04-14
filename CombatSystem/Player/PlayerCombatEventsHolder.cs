using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Player.Events;
using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Player
{
    public sealed class PlayerCombatEventsHolder : ControllerCombatEventsHolder, ITempoTickListener, IDiscriminationEventsHolder,

        ISkillPointerListener, ISkillSelectionListener,
        ITargetPointerListener, ITargetSelectionListener,
        ICameraHolderListener
    {
        public PlayerCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);

            _skillPointerListeners = new HashSet<ISkillPointerListener>();
            _skillSelectionListeners = new HashSet<ISkillSelectionListener>();
            _targetPointerListeners = new HashSet<ITargetPointerListener>();
            _targetSelectionListeners = new HashSet<ITargetSelectionListener>();

            _cameraHolderListeners = new HashSet<ICameraHolderListener>();

#if UNITY_EDITOR
            //Subscribe(new DebugPlayerEvents());
#endif

        }

        [ShowInInspector,HorizontalGroup("Skills")]
        private readonly HashSet<ISkillPointerListener> _skillPointerListeners;
        [ShowInInspector,HorizontalGroup("Skills")]
        private readonly HashSet<ISkillSelectionListener> _skillSelectionListeners;
        [ShowInInspector,HorizontalGroup("Target")]
        private readonly HashSet<ITargetPointerListener> _targetPointerListeners;
        [ShowInInspector,HorizontalGroup("Target")]
        private readonly HashSet<ITargetSelectionListener> _targetSelectionListeners;

        [ShowInInspector] 
        private readonly HashSet<ICameraHolderListener> _cameraHolderListeners;

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            SubscribeAsPlayerEvent(listener);
        }

        public override void UnSubscribe(ICombatEventListener listener)
        {
            base.UnSubscribe(listener);
            if (listener is ISkillPointerListener skillPointerListener)
                _skillPointerListeners.Remove(skillPointerListener);
            if (listener is ISkillSelectionListener skillSelectionListener)
                _skillSelectionListeners.Remove(skillSelectionListener);
            if (listener is ITargetPointerListener targetPointerListener)
                _targetPointerListeners.Remove(targetPointerListener);
            if (listener is ITargetSelectionListener targetSelectionListener)
                _targetSelectionListeners.Remove(targetSelectionListener);
            if (listener is ICameraHolderListener cameraHolderListener)
                _cameraHolderListeners.Remove(cameraHolderListener);
        }

        /// <summary>
        /// Subscribe as a: 
        /// <br></br>- <see cref="ISkillPointerListener"/>
        /// <br></br>- <see cref="ISkillSelectionListener"/>
        /// <br></br>- <see cref="ITargetPointerListener"/>
        /// <br></br>- <see cref="ITargetSelectionListener"/>
        /// </summary>
        public void SubscribeAsPlayerEvent(ICombatEventListener listener)
        {

            if (listener is ISkillPointerListener skillPointerListener)
                _skillPointerListeners.Add(skillPointerListener);
            if (listener is ISkillSelectionListener skillSelectionListener)
                _skillSelectionListeners.Add(skillSelectionListener);
            if (listener is ITargetPointerListener targetPointerListener)
                _targetPointerListeners.Add(targetPointerListener);
            if (listener is ITargetSelectionListener targetSelectionListener)
                _targetSelectionListeners.Add(targetSelectionListener);
            if (listener is ICameraHolderListener cameraHolderListener)
                _cameraHolderListeners.Add(cameraHolderListener);
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

        internal void ManualUnSubscribe(ICameraHolderListener cameraHolderListener)
        {
            _cameraHolderListeners.Remove(cameraHolderListener);
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
        public void OnSwitchCamera(in Camera combatCamera)
        {
            foreach (var listener in _cameraHolderListeners)
            {
                listener.OnSwitchCamera(in combatCamera);
            }
        }
#if UNITY_EDITOR

        private class DebugPlayerEvents :
            ISkillPointerListener, ISkillSelectionListener,
            ITargetPointerListener, ITargetSelectionListener
        {
            public void OnSkillButtonHover(in CombatSkill skill)
            {
            }

            public void OnSkillButtonExit(in CombatSkill skill)
            {
            }

            public void OnSkillSelect(in CombatSkill skill)
            {
                Debug.Log($"Skill Select: {skill.Preset}");
            }

            public void OnSkillSwitch(in CombatSkill skill,in CombatSkill previousSelection)
            {
                Debug.Log($"Skill SWITCH: {skill.Preset} FROM {previousSelection.Preset}");
            }

            public void OnSkillDeselect(in CombatSkill skill)
            {
                Debug.Log($"Skill DESELECTED: {skill.Preset}");
            }

            public void OnSkillCancel(in CombatSkill skill)
            {
                Debug.Log($"Skill CANCEL: {skill.Preset}");
            }

            public void OnSkillSubmit(in CombatSkill skill)
            {
                Debug.Log($"xxxx - Skill Submit: {skill.Preset}");
            }

            public void OnTargetButtonHover(in CombatEntity target)
            {
                Debug.Log($"Target Hover: {target.GetProviderEntityName()}");
            }

            public void OnTargetButtonExit(in CombatEntity target)
            {
                Debug.Log($"Target Exit: {target.GetProviderEntityName()}");
            }

            public void OnTargetSelect(in CombatEntity target)
            {
                Debug.Log($"Target Hover: {target.GetProviderEntityName()}");
            }

            public void OnTargetCancel(in CombatEntity target)
            {
                Debug.Log($"Target Cancel: {target.GetProviderEntityName()}");
            }

            public void OnTargetSubmit(in CombatEntity target)
            {
                Debug.Log($"xxxx - Target SUBMIT: {target.GetProviderEntityName()}");
            }
        } 
#endif
    }
}
