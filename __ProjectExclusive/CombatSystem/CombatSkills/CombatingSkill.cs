using System.Collections.Generic;
using CombatEffects;
using Stats;
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
            _originalUseCost = preset.GetUseCost(); 
        }

        public CombatingSkill(SkillProviderParams providerParams)
        {
            Preset = providerParams.preset;
            _originalUseCost = Preset.GetUseCost() + providerParams.cooldownVariation;
        }


        //this exists because some skill could have their cooldown altered by passives TODO make the passives for that
        private readonly int _originalUseCost;
        private int _currentUseCost;
        private EnumSkills.SKillState _currentState;

        public bool CanBeUsed(CombatStatsHolder stats) =>
            _currentState == EnumSkills.SKillState.Idle
            && stats.CurrentActions >= _currentUseCost;
       
        public void Silence() => _currentState = EnumSkills.SKillState.Silence;
        public void ResetCost()
        {
            _currentUseCost = _originalUseCost;
            _currentState = EnumSkills.SKillState.Idle;
        }
        public void OnUseIncreaseCost() => _currentUseCost++;

        public readonly ISkill Preset;
        public string GetSkillName() => Preset.GetSkillName();
        public Sprite GetIcon() => Preset.GetIcon();
        public EnumSkills.TargetType GetTargetType() => Preset.GetTargetType();
        public int GetUseCost() => _currentUseCost;
        public bool CanCrit() => Preset.CanCrit();
        public float GetCritVariation() => Preset.GetCritVariation();
        public ISkillComponent GetDescriptiveEffect() => Preset.GetDescriptiveEffect();
        public bool IsMainEffectAfterListEffects => Preset.IsMainEffectAfterListEffects;
        public EffectParameter GetMainEffect() => Preset.GetMainEffect();
        public List<EffectParameter> GetEffects() => Preset.GetEffects();
    }
}
