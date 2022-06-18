using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using UnityEngine;

namespace CombatSystem.Skills
{
    public static class DebugSkillTypes
    {
        public static readonly IFullSkill OffensiveSkillPreset
            = new PresetSkill(DebugEffectTypes.OffensiveEffect, EnumsSkill.Archetype.Offensive, EnumsSkill.TargetType.Direct);
        public static readonly IFullSkill SupportSkillPreset
            = new PresetSkill(DebugEffectTypes.SupportEffect,EnumsSkill.Archetype.Support,EnumsSkill.TargetType.Direct);
        public static readonly IFullSkill TeamSkillPreset
            = new PresetSkill(DebugEffectTypes.TeamEffect,EnumsSkill.Archetype.Self,EnumsSkill.TargetType.Direct);

        public static readonly CombatSkill OffensiveCombatSkill
        = new CombatSkill(OffensiveSkillPreset);
        public static readonly CombatSkill SupportCombatSkill
        = new CombatSkill(SupportSkillPreset);
        public static readonly CombatSkill TeamCombatSkill
        = new CombatSkill(TeamSkillPreset);



        private sealed class PresetSkill : IFullSkill
        {
            public PresetSkill(IEffect effect, EnumsSkill.Archetype archetype, EnumsSkill.TargetType targetType)
            {
                _effect = effect;
                Archetype = archetype;
                TargetType = targetType;
            }

            private readonly IEffect _effect;

            public int SkillCost => 0;
            public EnumsSkill.Archetype Archetype { get; }
            public EnumsSkill.TargetType TargetType { get; }
            public IEffect GetMainEffectArchetype() => _effect;

            public bool IgnoreSelf() => true;

            public string GetSkillName() => "PRESET - " + Archetype + " [" + TargetType + "] "+ ToString();

            public Sprite GetSkillIcon() => null;

            public IEnumerable<PerformEffectValues> GetEffects() => null;
            public IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => null;
        }

    }

    public static class DebugEffectTypes
    {
        public static readonly IEffect OffensiveEffect
            = new PresetEffect(EnumsEffect.ConcreteType.DamageType);

        public static readonly IEffect SupportEffect
            = new PresetEffect(EnumsEffect.ConcreteType.Buff);

        public static readonly IEffect TeamEffect
            = new PresetEffect(EnumsEffect.ConcreteType.ControlGain);

        private sealed class PresetEffect : IEffect
        {
            public PresetEffect(EnumsEffect.ConcreteType effectType)
            {
                EffectType = effectType;
            }

            private string TryGetName() => "PRESET - " + ToString() + " - " + EffectType;
            public string EffectTag => TryGetName();
            public string EffectSmallPrefix => TryGetName();
            public EnumsEffect.ConcreteType EffectType { get; }
            public Sprite GetIcon() => null;
            public GameObject GetSecondaryParticlesPrefab() => null;

            public void DoEffect(CombatEntity performer, CombatEntity target, float effectValue)
            {
                Debug.Log(EffectTag + $" - P[{performer.CombatCharacterName}] >> T[{target.CombatCharacterName}]");
            }
        }
    }
    
}
