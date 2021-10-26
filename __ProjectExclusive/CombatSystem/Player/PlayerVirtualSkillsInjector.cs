using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    /// <summary>
    /// Just handles the creation and injection of [<see cref="CombatingSkill"/>] into [<see cref="PlayerVirtualSkill"/>]s.<br></br>
    /// It also sends the injection the [<seealso cref="PlayerEvents.OnInjectionVirtualSkills"/>] since it's the behaviour
    /// than handles all preparations for that.
    /// </summary>
    public class PlayerVirtualSkillsInjector : ITempoListener<CombatingEntity>
    {
#if UNITY_EDITOR
        [ShowInInspector, TextArea] 
        private const string BehaviourDescription = "Handles the creation and injection of PlayerVirtualSkills from " +
                                                    "CombatSkills. The main difference between those two is that virtualSkills " +
                                                    "have more states that depends on player inputs.";
#endif

        public PlayerVirtualSkillsInjector()
        {}




        public void OnFirstAction(CombatingEntity element)
        {
            var currentSkills = element.SkillsHolder.GetCurrentSkills();
            PlayerCombatSingleton.PlayerEvents.OnInjectionVirtualSkills(element,currentSkills);
        }

        public void OnFinishAction(CombatingEntity element)
        {}

        public void OnFinishAllActions(CombatingEntity element)
        {}

        public void OnCantAct(CombatingEntity element)
        {}

    }


    public struct VirtualSkillSelection
    {
        public VirtualSkillSelection(CombatingSkill prioritySkill, List<CombatingEntity> possibleTargets)
        {
            PrioritySkill = prioritySkill;
            PossibleTargets = possibleTargets;
        }

        public readonly CombatingSkill PrioritySkill;
        public readonly List<CombatingEntity> PossibleTargets;
    }

    public interface IVirtualSkillInjectionListener 
    {
        void OnInjectionVirtualSkills(CombatingEntity user, ISkillGroupTypesRead<List<CombatingSkill>> skillGroup);
       
    }

    /// <summary>
    /// Unlike [<seealso cref="ISkillEventListener"/>], which events are send at the moment of executing, this listener
    /// are meant to be used while in the middle process to select the a skill. 
    /// </summary>
    public interface IVirtualSkillInteraction
    {
        void OnSelect(VirtualSkillSelection selection);
        void OnDeselect(VirtualSkillSelection selection);
        void OnSubmit(VirtualSkillSelection selection);
        void OnHover(VirtualSkillSelection selection);
        void OnHoverExit(VirtualSkillSelection selection);
    }

    public interface IVirtualSkillTargetListener
    {
        void OnVirtualTargetSelect(CombatingEntity selectedTarget);
    }
}
