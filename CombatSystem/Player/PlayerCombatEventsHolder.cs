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
        ITargetPointerListener, ITargetSelectionListener
    {
        public PlayerCombatEventsHolder() : base()
        {
            var combatEventsHolder = CombatSystemSingleton.EventsHolder;
            combatEventsHolder.SubscribeEventsHandler(this);

            _skillPointerListeners = new HashSet<ISkillPointerListener>();
            _skillSelectionListeners = new HashSet<ISkillSelectionListener>();
            _targetPointerListeners = new HashSet<ITargetPointerListener>();
            _targetSelectionListeners = new HashSet<ITargetSelectionListener>();

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

        public override void Subscribe(ICombatEventListener listener)
        {
            base.Subscribe(listener);
            SubscribeAsPlayerEvent(listener);
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
                Debug.Log($"Target Hover: {target.GetEntityName()}");
            }

            public void OnTargetButtonExit(in CombatEntity target)
            {
                Debug.Log($"Target Exit: {target.GetEntityName()}");
            }

            public void OnTargetSelect(in CombatEntity target)
            {
                Debug.Log($"Target Hover: {target.GetEntityName()}");
            }

            public void OnTargetCancel(in CombatEntity target)
            {
                Debug.Log($"Target Cancel: {target.GetEntityName()}");
            }

            public void OnTargetSubmit(in CombatEntity target)
            {
                Debug.Log($"xxxx - Target SUBMIT: {target.GetEntityName()}");
            }
        } 
#endif
    }
}
