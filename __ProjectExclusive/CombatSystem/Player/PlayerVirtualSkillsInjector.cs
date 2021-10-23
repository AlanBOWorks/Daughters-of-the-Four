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
    /// It also sends the injection the [<seealso cref="PlayerEvents.OnInjectionSkills"/>] since it's the behaviour
    /// than handles all preparations for that.
    /// </summary>
    public class PlayerVirtualSkillsInjector : ITempoListener<CombatingEntity>, ISkillGroupTypesRead<List<PlayerVirtualSkill>>
    {
#if UNITY_EDITOR
        [ShowInInspector, TextArea] 
        private const string BehaviourDescription = "Handles the creation and injection of PlayerVirtualSkills from " +
                                                    "CombatSkills. The main difference between those two is that virtualSkills " +
                                                    "have more states that depends on player inputs.";
#endif

        public PlayerVirtualSkillsInjector()
        {
            _skillsPool = new Queue<PlayerVirtualSkill>();
            SharedSkillTypes = new List<PlayerVirtualSkill>();
            MainSkillTypes = new List<PlayerVirtualSkill>();
        }

        private readonly Queue<PlayerVirtualSkill> _skillsPool;
        [ShowInInspector,HorizontalGroup()]
        public List<PlayerVirtualSkill> SharedSkillTypes { get; }
        [ShowInInspector,HorizontalGroup()]
        public List<PlayerVirtualSkill> MainSkillTypes { get; }



        public void OnInitiativeTrigger(CombatingEntity element)
        {
            var currentSkills = element.SkillsHolder.GetCurrentSkills();
            UtilSkills.DoActionOn(currentSkills,this,HandleList);
            PlayerCombatSingleton.PlayerEvents.OnInjectionSkills(element,this);


            void HandleList(List<CombatingSkill> injectionSkills, List<PlayerVirtualSkill> injectIn)
            {
                if(injectionSkills == null || injectIn == null) return;

                foreach (var injectionSkill in injectionSkills)
                {
                    PlayerVirtualSkill virtualSkill;

                    if (_skillsPool.Count > 0)
                    {
                        virtualSkill = _skillsPool.Dequeue();
                        virtualSkill.Injection(injectionSkill);
                    }
                    else
                    {
                        virtualSkill = new PlayerVirtualSkill(injectionSkill);
                    }
                    injectIn.Add(virtualSkill);
                }
            }
            
        }

        public void OnDoMoreActions(CombatingEntity element)
        {
            UtilSkills.DoActionOn(this,UpdateSkills);
            void UpdateSkills(List<PlayerVirtualSkill> skills)
            {
                if(skills == null) return;

                foreach (var skill in skills)
                {
                    skill.TickUpdate();
                }
            }
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
            UtilSkills.DoActionOn(this,EnQueueSkills);
            void EnQueueSkills(List<PlayerVirtualSkill> skills)
            {
                if(skills == null) return;

                for (var i = skills.Count - 1; i >= 0; i--)
                {
                    var skill = skills[i];
                    _skillsPool.Enqueue(skill);
                    skills.RemoveAt(i);
                }
            }
        }

        public void OnSkipActions(CombatingEntity element)
        {
        }

    }

    /// <summary>
    /// Used as a wrapper for <seealso cref="CombatingSkill"/>;<br></br>
    /// The difference reside in the
    /// [<see cref="PlayerVirtualSkill.State"/>] vs [<seealso cref="EnumSkills.SKillState"/>],
    /// being the first wider than the second one.
    /// </summary>
    public class PlayerVirtualSkill
    {
        public PlayerVirtualSkill(CombatingSkill skill)
        {
            Injection(skill);
        }
        [ShowInInspector]
        public CombatingSkill CurrentSkill { get; private set; }
        [ShowInInspector]
        public State CurrentState { get; private set; }

        public void Injection(CombatingSkill skill)
        {
            CurrentSkill = skill;
            HandleInjectionState(skill.GetState());
        }

        private void HandleInjectionState(EnumSkills.SKillState state)
        {
            switch (state)
            {
                case EnumSkills.SKillState.Idle:
                    CurrentState = State.Idle;
                    break;
                case EnumSkills.SKillState.Cooldown:
                    CurrentState = State.Cooldown;
                    break;
                case EnumSkills.SKillState.Silence:
                    CurrentState = State.Silence;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void TickUpdate()
        {
            HandleInjectionState(CurrentSkill.GetState());
        }

        public enum State
        {
            Idle = EnumSkills.SKillState.Idle,
            Cooldown = EnumSkills.SKillState.Cooldown,
            Silence = EnumSkills.SKillState.Silence,
            /// <summary>
            /// Once the player selects the skills but is not confirmed the action (e.g: puts in a queue of desired actions)
            /// </summary>
            Selected,
            /// <summary>
            /// Confirms the action and submit to the action performer or a handler for it
            /// </summary>
            Submit
        }
    }

    public struct VirtualSkillSelection
    {
        public VirtualSkillSelection(PlayerVirtualSkill prioritySkill, List<CombatingEntity> possibleTargets)
        {
            PrioritySkill = prioritySkill;
            PossibleTargets = possibleTargets;
        }

        public readonly PlayerVirtualSkill PrioritySkill;
        public readonly List<CombatingEntity> PossibleTargets;
    }

    public interface IVirtualSkillInjectionListener 
    {
        void OnInjectionSkills(CombatingEntity user, ISkillGroupTypesRead<List<PlayerVirtualSkill>> skillGroup);
       
    }

    public interface IVirtualSkillInteraction
    {
        void OnSelect(VirtualSkillSelection selection);
        void OnDeselect(VirtualSkillSelection selection);
        void OnSubmit(VirtualSkillSelection selection);
        void OnHover(VirtualSkillSelection selection);
        void OnHoverExit(VirtualSkillSelection selection);
    }

    public interface IVirtualTargetInteraction
    {
        void OnTargetSelect(CombatingEntity selectedTarget);
    }
}
