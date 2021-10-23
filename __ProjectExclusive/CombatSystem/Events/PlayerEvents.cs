using System;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Events
{
    public class PlayerEvents : SystemEventsHolder, IVirtualSkillInjectionListener, IVirtualSkillInteraction,
        IVirtualSkillTargetListener
    {
        public PlayerEvents() : base()
        {
            SkillInteractions = new List<IVirtualSkillInteraction>();
            VirtualSkillInjections = new List<IVirtualSkillInjectionListener>();
            VirtualTargetListeners = new List<IVirtualSkillTargetListener>();
        }

        /// <summary>
        /// For [<seealso cref="PlayerEvents.OnSelect"/>],
        /// [<seealso cref="PlayerEvents.OnSubmit"/>],
        /// [<seealso cref="PlayerEvents.OnDeselect"/> check: <br></br>
        /// - [<seealso cref="USkillButtonsHolder"/>]<br></br>
        /// - [<seealso cref="USkillButton"/>]  
        /// </summary>
        [ShowInInspector]
        public readonly List<IVirtualSkillInteraction> SkillInteractions;
        /// <summary>
        /// For [<seealso cref="OnInjectionVirtualSkills"/>] check: <br></br>
        /// - [<seealso cref="PlayerVirtualSkillsInjector.OnInitiativeTrigger"/>]
        /// </summary>
        [ShowInInspector]
        public readonly List<IVirtualSkillInjectionListener> VirtualSkillInjections;

        [ShowInInspector] 
        public readonly List<IVirtualSkillTargetListener> VirtualTargetListeners;

        public void Subscribe(IVirtualSkillInteraction listener)
        {
            SkillInteractions.Add(listener);
        }

        public void Subscribe(IVirtualSkillInjectionListener listener)
        {
            VirtualSkillInjections.Add(listener);
        }

        public void Subscribe(IVirtualSkillTargetListener listener)
        {
            VirtualTargetListeners.Add(listener);
        }


        public void OnSelect(VirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnSelect(selection);
            }
        }

        public void OnDeselect(VirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnDeselect(selection);
            }
        }

        public void OnSubmit(VirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnSubmit(selection);
            }
        }

        public void OnHover(VirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnHover(selection);
            }
        }

        public void OnHoverExit(VirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnHoverExit(selection);
            }
        }

        public void OnInjectionVirtualSkills(CombatingEntity user, ISkillGroupTypesRead<List<CombatingSkill>> skillGroup)
        {
            foreach (var listener in VirtualSkillInjections)
            {
                listener.OnInjectionVirtualSkills(user,skillGroup);
            }
        }

        public void OnVirtualTargetSelect(CombatingEntity selectedTarget)
        {
            foreach (var listener in VirtualTargetListeners)
            {
                listener.OnVirtualTargetSelect(selectedTarget);
            }
        }
    }
}
