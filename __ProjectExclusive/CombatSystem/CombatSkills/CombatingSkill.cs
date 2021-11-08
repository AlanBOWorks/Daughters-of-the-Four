using System.Collections.Generic;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace CombatSkills
{
    // This is mean for keeping an unique reference of each skill (since it follows lightweight pattern);
    // Also will keep variation of effects/passives and cooldown when applicable
    public class CombatingSkill : ISkill
    {
        protected CombatingSkill(ISkill preset, int useCost)
        {
            Preset = preset;
            _originalUseCost = useCost;
            _currentUseCost = useCost;
        }

        public CombatingSkill(ISkill preset) : this(preset,preset.GetUseCost())
        { }


        [ShowInInspector]
        private readonly int _originalUseCost;
        [ShowInInspector]
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
        public virtual void OnUseIncreaseCost() => _currentUseCost++;



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

    public class ControlCombatingSkill : CombatingSkill
    {
        private const float UseCostToControlConversionRate = .1f;
        private const float FinalCostAddition = .05f;
        private const int ControlSkillUseCostFixedAmount = 1;

        public ControlCombatingSkill(ISkill preset) : base(preset, ControlSkillUseCostFixedAmount)
        {
            ControlCost = preset.GetUseCost() * UseCostToControlConversionRate;
            ControlCost += FinalCostAddition;
            if (ControlCost < 0) ControlCost = 0; //This is a safety check
        }

        public readonly float ControlCost;

        public override void OnUseIncreaseCost()
        {
            //By design ControlCombatingSkills cost always 1
        }
    }
}
