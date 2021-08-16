using System;
using System.Collections.Generic;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    /// <summary>
    /// Are temporal <see cref="Skill"/> that can be modified in runtime without worry
    /// </summary>
    public class CombatSkill : SkillBase
    {
        public CombatSkill(SkillPreset injection, bool isInCooldown = false)
        {
            this.preset = injection.Preset;
            if(preset != null)
                CooldownCost = preset.CoolDownCost + injection.coolDownVariation;
            SkillState = isInCooldown 
                ? State.Cooldown 
                : State.Idle;
        }

        public int CooldownCost;
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
        public State SkillState { get; private set; }

        public void SwitchState(State target)
        {
            SkillState = target;
        }

        public bool IsInCooldown()
        {
            return SkillState == State.Cooldown;
        }

        public bool CanBeUse(CombatingEntity user)
        {
            return !IsInCooldown() && preset.CanBeUse(user);
        }



        public void OnSkillUsage()
        {
            // >>>> Cost 0 can be used multiples times in the same initiative
            // >>>> Cost 1 can only be refreshed after the sequence is finish
            //
            // For characters with only 1 action per initiative is the same, but for 
            // others with >=2 actions there's a huge difference within cost 0 and cost 1
            if (CooldownCost <= 0) 
            {
                SkillState = State.Idle;
                return; 
            }
            CurrentCooldown = CooldownCost;
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
    public class SkillPreset : SkillBase
    {
        [Range(-100,100),SuffixLabel("Actions"), Tooltip("Adds to the preset cooldown amount")]
        public int coolDownVariation = 0;
    }

    public abstract class SkillBase
    {
        [TitleGroup("Variable")]
        [SerializeField] protected SSkillPreset preset = null;

        public float CriticalAddition => preset.criticalAddition;
        public bool CanCrit => preset.canCrit;
        public virtual SSkillPreset Preset => preset;
        public string SkillName => preset.SkillName; //could be modified by a special name in some cases
        public Sprite Icon => preset.SpecialIcon; // could be modified by another icon when upgraded or something
        public EnumSkills.TargetingType GetMainType() => preset.GetSkillType();
        public SEffectBase.EffectTarget GetMainTarget() => preset.GetMainEffect().GetEffectTarget();

    }
}
