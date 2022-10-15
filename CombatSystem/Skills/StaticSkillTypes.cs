using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Stats;
using UnityEngine;
using Utils_Project;

namespace CombatSystem.Skills
{
    public static class StaticSkillTypes
    {
        public static readonly IFullSkill OffensiveSkillPreset
            = new PresetSkill(DebugEffectTypes.OffensiveEffect, EnumsSkill.TeamTargeting.Offensive);
        public static readonly IFullSkill SupportSkillPreset
            = new PresetSkill(DebugEffectTypes.SupportEffect,EnumsSkill.TeamTargeting.Support);
        public static readonly IFullSkill TeamSkillPreset
            = new PresetSkill(DebugEffectTypes.TeamEffect,EnumsSkill.TeamTargeting.Self);

        public static readonly CombatSkill OffensiveCombatSkill
        = new CombatSkill(OffensiveSkillPreset);
        public static readonly CombatSkill SupportCombatSkill
        = new CombatSkill(SupportSkillPreset);
        public static readonly CombatSkill TeamCombatSkill
        = new CombatSkill(TeamSkillPreset);

        public static readonly IVanguardSkill RevengeVanguardSkill
        = new VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType.Counter);
        public static readonly IVanguardSkill PunishVanguardSkill
        = new VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType.Punish);

        public static IVanguardSkill GetVanguardSkill(EnumsVanguardEffects.VanguardEffectType type)
        {
            return type switch
            {
                EnumsVanguardEffects.VanguardEffectType.Counter => RevengeVanguardSkill,
                EnumsVanguardEffects.VanguardEffectType.Punish => PunishVanguardSkill,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        private sealed class PresetSkill : IFullSkill
        {
            public PresetSkill(IEffect effect, EnumsSkill.TeamTargeting teamTargeting)
            {
                _effect = effect;
                TeamTargeting = teamTargeting;
            }

            private readonly IEffect _effect;

            public int SkillCost => 0;
            public EnumsSkill.TeamTargeting TeamTargeting { get; }
            public IEffect GetMainEffectArchetype() => _effect;

            public bool IgnoreSelf => true;
            public float LuckModifier => -1;
            public bool CanCrit() => false;

            public string GetSkillName() => "PRESET - " + TeamTargeting;

            public Sprite GetSkillIcon() => null;

            public IEnumerable<PerformEffectValues> GetEffects() => null;
            public IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => null;
        }

        private sealed class VanguardPresetSkill : IVanguardSkill
        {
            public VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType vanguardEffectType)
            {
                MainVanguardType = vanguardEffectType;
            }

            public int SkillCost => 0;
            public EnumsSkill.TeamTargeting TeamTargeting => EnumsSkill.TeamTargeting.Self;
            public IEffect GetMainEffectArchetype() => null;
            public IEnumerable<PerformEffectValues> GetEffects() => null;

            public bool IgnoreSelf => false;

            public float LuckModifier => -1;

            public EnumsVanguardEffects.VanguardEffectType MainVanguardType { get; }

            public IEnumerable<PerformEffectValues> GetCounterEffects() => null;
            public bool HasCounterEffects() => false;

            public IEnumerable<PerformEffectValues> GetPunishEffects() => null;
            public bool HasPunishEffects() => false;
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

            public void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
            {
                entities.Extract(out var performer, out var target);
                Debug.Log(EffectTag + $" - P[{performer.CombatCharacterName}] >> T[{target.CombatCharacterName}]");
            }

            public string GetEffectValueTootLip(CombatStats performerStats, ref float effectValue)
            {
                return LocalizeMath.LocalizePercentValue(effectValue);
            }

            public float CalculateEffectByStatValue(CombatStats performerStats, float effectValue)
            {
                return 1;
            }

            public bool IsPercentSuffix() => true;
            public bool IsPercentTooltip() => true;
        }
    }
    
}
