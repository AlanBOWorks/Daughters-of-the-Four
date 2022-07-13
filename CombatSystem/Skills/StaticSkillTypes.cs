using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills
{
    public static class StaticSkillTypes
    {
        public static readonly IFullSkill OffensiveSkillPreset
            = new PresetSkill(DebugEffectTypes.OffensiveEffect, EnumsSkill.TeamTargeting.Offensive, EnumsSkill.TargetType.Direct);
        public static readonly IFullSkill SupportSkillPreset
            = new PresetSkill(DebugEffectTypes.SupportEffect,EnumsSkill.TeamTargeting.Support,EnumsSkill.TargetType.Direct);
        public static readonly IFullSkill TeamSkillPreset
            = new PresetSkill(DebugEffectTypes.TeamEffect,EnumsSkill.TeamTargeting.Self,EnumsSkill.TargetType.Direct);

        public static readonly CombatSkill OffensiveCombatSkill
        = new CombatSkill(OffensiveSkillPreset);
        public static readonly CombatSkill SupportCombatSkill
        = new CombatSkill(SupportSkillPreset);
        public static readonly CombatSkill TeamCombatSkill
        = new CombatSkill(TeamSkillPreset);

        public static readonly IVanguardSkill RevengeVanguardSkill
        = new VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType.Revenge);
        public static readonly IVanguardSkill PunishVanguardSkill
        = new VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType.Punish);

        public static IVanguardSkill GetVanguardSkill(EnumsVanguardEffects.VanguardEffectType type)
        {
            return type switch
            {
                EnumsVanguardEffects.VanguardEffectType.Revenge => RevengeVanguardSkill,
                EnumsVanguardEffects.VanguardEffectType.Punish => PunishVanguardSkill,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
            };
        }
        private sealed class PresetSkill : IFullSkill
        {
            public PresetSkill(IEffect effect, EnumsSkill.TeamTargeting teamTargeting, EnumsSkill.TargetType targetType)
            {
                _effect = effect;
                TeamTargeting = teamTargeting;
                TargetType = targetType;
            }

            private readonly IEffect _effect;

            public int SkillCost => 0;
            public EnumsSkill.TeamTargeting TeamTargeting { get; }
            public EnumsSkill.TargetType TargetType { get; }
            public IEffect GetMainEffectArchetype() => _effect;

            public bool IgnoreSelf() => true;
            public float LuckModifier => -1;
            public bool CanCrit() => false;

            public string GetSkillName() => "PRESET - " + TeamTargeting + " [" + TargetType + "] "+ ToString();

            public Sprite GetSkillIcon() => null;

            public IEnumerable<PerformEffectValues> GetEffects() => null;
            public IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => null;
        }

        private sealed class VanguardPresetSkill : IVanguardSkill
        {
            public VanguardPresetSkill(EnumsVanguardEffects.VanguardEffectType vanguardEffectType)
            {
                _vanguardEffectType = vanguardEffectType;
            }

            private readonly EnumsVanguardEffects.VanguardEffectType _vanguardEffectType;

            public int SkillCost => 0;
            public EnumsSkill.TeamTargeting TeamTargeting => EnumsSkill.TeamTargeting.Self;
            public EnumsSkill.TargetType TargetType => EnumsSkill.TargetType.Direct;
            public IEffect GetMainEffectArchetype() => null;
            public IEnumerable<PerformEffectValues> GetEffects() => null;
            public IEnumerable<PerformEffectValues> GetPerformVanguardEffects() => null;
            public int VanguardEffectCount => 0;

            public bool IgnoreSelf() => false;
            public EnumsVanguardEffects.VanguardEffectType GetVanguardEffectType() => _vanguardEffectType;
            public bool IsMultiTrigger() => false;
            public PerformEffectValues GetVanguardEffectTooltip() => new PerformEffectValues();

            public float LuckModifier => -1;
            public bool CanCrit() => false;

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

            public void DoEffect(EntityPairInteraction entities, ref float effectValue)
            {
                entities.Extract(out var performer, out var target);
                Debug.Log(EffectTag + $" - P[{performer.CombatCharacterName}] >> T[{target.CombatCharacterName}]");
            }

            public string GetEffectValueTootLip(CombatStats performerStats, float effectValue)
            {
                return LocalizeEffects.LocalizePercentValue(effectValue);
            }
            public bool IsPercentSuffix() => true;
        }
    }
    
}
