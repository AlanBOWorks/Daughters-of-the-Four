using System.Collections.Generic;
using CombatEffects;
using UnityEngine;

namespace CombatSkills
{
    // This is mean for keeping an unique reference of each skill (since it follows lightweight pattern);
    // Also will keep variation of effects/passives and cooldown when applicable
    public class CombatingSkill : ISkill
    {
       
        public CombatingSkill(ISkill preset)
        {
            Preset = preset;
            _cooldownOnUsage = preset.GetCooldownAmount(); 
        }

        public CombatingSkill(SkillProviderParams providerParams)
        {
            Preset = providerParams.preset.GetSkillPreset();
            _cooldownOnUsage = Preset.GetCooldownAmount() + providerParams.cooldownVariation;
        }


        //this exists because some skill could have their cooldown altered by passives TODO make the passives for that
        private readonly int _cooldownOnUsage;
        private int _currentCooldownAmount;
        private EnumSkills.SKillState _currentState;

        public EnumSkills.SKillState GetState() => _currentState;
        public bool CanBeUsed() => _currentState == EnumSkills.SKillState.Idle;
        public void PutInCooldown()
        {
            _currentCooldownAmount = _cooldownOnUsage;
            // cooldown == 0 are special skills that can't be put in cooldown
            if (_currentCooldownAmount > 0) _currentState = EnumSkills.SKillState.Cooldown;
        }
        public void Silence() => _currentState = EnumSkills.SKillState.Silence;

        public void TickCooldown()
        {
            _currentCooldownAmount--;
            if (_currentCooldownAmount > 0) return;
            _currentCooldownAmount = 0;

            _currentState = EnumSkills.SKillState.Idle;
        }
        public void ForceRefresh()
        {
            _currentCooldownAmount = 0;
            _currentState = EnumSkills.SKillState.Idle;
        }


        public readonly ISkill Preset;
        public string GetSkillName() => Preset.GetSkillName();
        public Sprite GetIcon() => Preset.GetIcon();
        public EnumSkills.TargetType GetTargetType() => Preset.GetTargetType();
        public int GetCooldownAmount() => _currentCooldownAmount;
        public bool CanCrit() => Preset.CanCrit();
        public float GetCritVariation() => Preset.GetCritVariation();
        public ISkillComponent GetDescriptiveEffect() => Preset.GetDescriptiveEffect();
        public List<EffectParameter> GetEffects() => Preset.GetEffects();
    }
}
