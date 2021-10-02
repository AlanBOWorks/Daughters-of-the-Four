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
            Preset = providerParams.preset;
            _cooldownOnUsage = Preset.GetCooldownAmount() + providerParams.cooldownVariation;
        }


        //this exists because some skill could have their cooldown altered by passives TODO make the passives for that
        private readonly int _cooldownOnUsage;
        private int _currentCooldownAmount;
        private EnumSkills.SKillState _currentState;

        public bool IsInCooldown() => _currentState == EnumSkills.SKillState.InCooldown;
        public void PutInCooldown()
        {
            _currentCooldownAmount = _cooldownOnUsage;
            // cooldown == 0 are special skills that can't be put in cooldown
            if (_currentCooldownAmount > 0) _currentState = EnumSkills.SKillState.InCooldown;
        }
        public void TickCooldown()
        {
            _currentCooldownAmount--;
        }
        /// <summary>
        /// Resets the state of the 
        /// </summary>
        public void TickRefresh()
        {
            if(_currentCooldownAmount > 0) return;
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
        public IEffect GetDescriptiveEffect() => Preset.GetDescriptiveEffect();
        public EffectParameter[] GetEffects() => Preset.GetEffects();
    }
}