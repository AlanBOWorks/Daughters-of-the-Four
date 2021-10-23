using System;
using System.Collections.Generic;
using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Events
{
    public class PlayerEvents : SystemEventsHolder, IVirtualSkillInjectionListener, IVirtualSkillInteraction
    {
        public PlayerEvents() : base()
        {
            SkillInteractions = new List<IVirtualSkillInteraction>();
            VirtualSkillInjections = new List<IVirtualSkillInjectionListener>();
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
        /// For [<seealso cref="OnInjectionSkills"/>] check: <br></br>
        /// - [<seealso cref="PlayerVirtualSkillsInjector.OnInitiativeTrigger"/>]
        /// </summary>
        [ShowInInspector]
        public readonly List<IVirtualSkillInjectionListener> VirtualSkillInjections;

        public void Subscribe(IVirtualSkillInteraction listener)
        {
            SkillInteractions.Add(listener);
        }

        public void Subscribe(IVirtualSkillInjectionListener listener)
        {
            VirtualSkillInjections.Add(listener);
        }



        public void OnSelect(IVirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnSelect(selection);
            }
        }

        public void OnDeselect(IVirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnDeselect(selection);
            }
        }

        public void OnSubmit(IVirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnSubmit(selection);
            }
        }

        public void OnHover(IVirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnHover(selection);
            }
        }

        public void OnHoverExit(IVirtualSkillSelection selection)
        {
            foreach (var listener in SkillInteractions)
            {
                listener.OnHoverExit(selection);
            }
        }

        public void OnInjectionSkills(CombatingEntity user, ISkillGroupTypesRead<List<PlayerVirtualSkill>> skillGroup)
        {
            foreach (var listener in VirtualSkillInjections)
            {
                listener.OnInjectionSkills(user,skillGroup);
            }
        }
    }
}
