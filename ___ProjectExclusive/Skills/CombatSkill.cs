using System;
using System.Collections.Generic;
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
            cooldownCost = injection.cooldownCost;
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


        public void OnSkillUsage()
        {
            // >>>> Cost 0 can be used multiples times in the same initiative
            // >>>> Cost 1 can only be refreshed after the sequence is finish
            //
            // For characters with only 1 action per initiative is the same, but for 
            // others with >=2 actions there's a huge difference within cost 0 and cost 1
            if (cooldownCost <= 0) 
            {
                SkillState = State.Idle;
                return; 
            }
            CurrentCooldown = cooldownCost;
            SkillState = State.Cooldown;
        }
        public void OnCharacterAction()
        {
            // Idle or Selected
            if(!IsInCooldown()) return;

            //do cooldown
            --CurrentCooldown;
            if (CurrentCooldown >= 0) return;
            CurrentCooldown = 0; // this value could be less than 0 by other skills
        }

        public void OnCharacterFinish()
        {
            OnCharacterAction();
            if(CurrentCooldown > 0) return;
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
        public int cooldownCost = 1;


        public float CriticalAddition => preset.criticalAddition;
        public bool CanCrit => preset.canCrit;

        /// <summary>
        /// Used for to extract preset data or KeyReferences
        /// </summary>
        public SSkillPreset Preset => preset;
        public string SkillName => preset.SkillName; //could be modified by a special name in some cases
        public Sprite Icon => preset.Icon; // could be modified by another icon when upgraded or something
        public List<EffectParams> GetEffects() => preset.Effects;

        public SEffectBase.EffectType GetMainType() => preset.GetMainEffect().GetEffectType();
        public SEffectBase.EffectTarget GetMainTarget() => preset.GetMainEffect().GetEffectTarget();
    }
}
