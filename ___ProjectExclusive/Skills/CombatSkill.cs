using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Are temporal <see cref="Skill"/> that can be modified in runtime without worry
    /// </summary>
    public class CombatSkill : Skill
    {
        public CombatSkill(Skill injection, bool isInCooldown = false)
        {
            this.preset = injection.Preset;
            cooldownAmount = injection.cooldownAmount;
            SkillState = isInCooldown 
                ? State.Cooldown 
                : State.Idle;
        }

        /// <summary>
        /// After use, this coldDown will be used for disable the skill until reach 0
        /// </summary>
        [ShowInInspector]
        public int CurrentCooldown { get; private set; }

        public enum State
        {
            /// <summary>
            /// Haven't being used 
            /// </summary>
            Idle,
            /// <summary>
            /// Have being used but it's waiting to confirmation
            /// </summary>
            Selected,
            /// <summary>
            /// Waits for refresh
            /// </summary>
            Cooldown
        }
        [ShowInInspector]
        public State SkillState;

        public bool IsInCooldown()
        {
            return SkillState == State.Cooldown;
        }

        //TODO give additional buff to skills if applied

        public void OnSelect()
        {
            switch (SkillState)
            {
                case State.Idle:
                    SkillState = State.Selected;
                    break;
                case State.Selected:
                    SkillState = State.Idle;
                    break;
                default:
                case State.Cooldown:
                    break;
            }
        }

        public void OnSubmit()
        {
            if(cooldownAmount <= 0) return; //Cost 0 can be used multiples times in the same "round"
            CurrentCooldown = cooldownAmount;
            //TODO Invoke character Animator + applyEffects
        }
        public void OnTick()
        {
            // Idle or Selected
            if(!IsInCooldown()) return;

            //do cooldown
            --CurrentCooldown;
            if (CurrentCooldown >= 0) return;
            CurrentCooldown = 0; // this value could be less than 0 by other skills
            SkillState = State.Idle;
        }
    }

    /// <summary>
    /// Specific skill for each <seealso cref="SCharacterSkillsPreset"/> group.<br></br>
    /// For <seealso cref="___ProjectExclusive.Characters.PlayerCharacterCombatData"/>
    /// this could even be modified for 'upgrade mechanics'
    /// </summary>
    [Serializable]
    public class Skill
    {
        [TitleGroup("Variable")]
        [SerializeField] protected SSkillPreset preset = null;
        [TitleGroup("Stats"), Range(0, 100), SuffixLabel("actions")]
        public int cooldownAmount = 1;

        /// <summary>
        /// Used for to extract preset data or KeyReferences
        /// </summary>
        public SSkillPreset Preset => preset;
        public string SkillName => preset.SkillName; //could be modified by a special name in some cases
        public Sprite Icon => preset.Icon; // could be modified by another icon when upgraded or something

    }
}
