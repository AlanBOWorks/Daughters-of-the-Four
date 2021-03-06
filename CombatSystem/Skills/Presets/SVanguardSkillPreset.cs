using System;
using System.Collections.Generic;
using System.Globalization;
using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N" + VanguardAssetPrefix,
        menuName = "Combat/Skill/Vanguard Preset", order = -10)]
    public class SVanguardSkillPreset : SSkillPresetBase, IVanguardSkill
    {
        private const string VanguardAssetPrefix =  " [Vanguard Skill]";

        [TitleGroup("Values")]
        [SerializeField]
        private bool ignoreSelf = true;

        [TitleGroup("Values")]
        [InfoBox("Target Type [Target]: enemy who did the Offensive is the Target\n" +
                 "Target Type [Performer]: the main vanguard is the Performer")]
        [SerializeField]
        private EnumsVanguardEffects.VanguardEffectType responseType;
        private VanguardEffect _vanguardEffectPreset;

        protected override string GetAssetPrefix() => VanguardAssetPrefix;

        protected override void OnEnable()
        {
            base.OnEnable();
            _vanguardEffectPreset = new VanguardEffect(this);
        }


        public override IEffect GetMainEffectArchetype() => _vanguardEffectPreset;
        public override bool IgnoreSelf() => ignoreSelf;



        public PerformEffectValues GenerateVanguardValues() =>
            new PerformEffectValues(_vanguardEffectPreset, 1, EnumsEffect.TargetType.Performer);
        public override IEnumerable<PerformEffectValues> GetEffects()
        {
            yield return GenerateVanguardValues();
        }
        public override IEnumerable<PerformEffectValues> GetEffectsFeedBacks() 
            => GetPerformVanguardEffects();
        public IEnumerable<PerformEffectValues> GetPerformVanguardEffects() 
            => base.GetEffects();

        public int VanguardEffectCount => effects.Length;


        public override EnumsSkill.TeamTargeting TeamTargeting => EnumsSkill.TeamTargeting.Self;
        public EnumsVanguardEffects.VanguardEffectType GetVanguardEffectType() => responseType;
        


        protected override string GenerateAssetName()
        {
            return responseType.ToString().ToUpper() + " - " + base.GenerateAssetName();
        }

        private sealed class VanguardEffect : IEffect
        {
            public VanguardEffect(SVanguardSkillPreset preset)
            {
                _skill = preset;
                _skillIcon = preset.GetSkillIcon();
            }

            private readonly SVanguardSkillPreset _skill;
            [ShowInInspector]
            private readonly Sprite _skillIcon;

            private const string VanguardEffectTag = "Vanguard_Effect";
            private const string VanguardEffectPrefix = "Vgrd";

            public string EffectTag => VanguardEffectTag;
            public string EffectSmallPrefix => VanguardEffectPrefix;
            public EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.DefaultTeam;
            public Sprite GetIcon() => _skillIcon;

            // VanguardSkills uses this object as a primary effect, thus this secondaryParticles is never used
            public GameObject GetSecondaryParticlesPrefab()
            {
                return null;
            }

            public void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier)
            {
                var performer = entities.Performer;
                var vanguardEffectsHolder = performer.Team.VanguardEffectsHolder;

                vanguardEffectsHolder.AddEffect(_skill);
            }
            public string GetEffectValueTootLip(CombatStats performerStats, ref float effectValue)
            {
                return LocalizeEffects.LocalizePercentValue(effectValue);
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
