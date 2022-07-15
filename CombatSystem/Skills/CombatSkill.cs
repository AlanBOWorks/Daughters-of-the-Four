using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    public class CombatSkill : ICombatSkill
    {
        public CombatSkill(IFullSkill preset)
        {
            Preset = preset;
            SkillCost = preset.SkillCost;
        }

        [ShowInInspector,InlineEditor()]
        public IFullSkill Preset { get; }

        public string GetSkillName() => Preset.GetSkillName();
        public Sprite GetSkillIcon() => Preset.GetSkillIcon();
        public IEnumerable<PerformEffectValues> GetEffects() => Preset.GetEffects();
        public IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => Preset.GetEffectsFeedBacks();


        [ShowInInspector]
        public int SkillCost { get; private set; }
        public EnumsSkill.TeamTargeting TeamTargeting => Preset.TeamTargeting;
        public IEffect GetMainEffectArchetype() => Preset.GetMainEffectArchetype();


        public bool IgnoreSelf() => Preset.IgnoreSelf();
        public float LuckModifier => Preset.LuckModifier;


        public void IncreaseCost()
        {
            if(SkillCost < 0) return;
            SkillCost++;
        }
        public void ResetCost()
        {
            SkillCost = Preset.SkillCost;
        }
    }
}
