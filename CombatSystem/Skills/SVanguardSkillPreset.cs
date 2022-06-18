using System;
using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using CombatSystem.Team;
using CombatSystem.Team.VanguardEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N [Vanguard Skill Preset]",
        menuName = "Combat/Skill/Vanguard Preset")]
    public class SVanguardSkillPreset : SSkillPresetBase, IVanguardSkill
    {
        [TitleGroup("Values")]
        [SerializeField]
        private bool ignoreSelf = true;

        [TitleGroup("Values")]
        [InfoBox("Target Type [Target]: enemy who did the Offensive is the Target\n" +
                 "Target Type [Performer]: the main vanguard is the Performer")]
        [SerializeField]
        private EnumsVanguardEffects.VanguardEffectType responseType;


        private VanguardEffect _vanguardEffectPreset;


        private void OnEnable()
        {
            _vanguardEffectPreset = new VanguardEffect(this);
        }


        public override IEffect GetMainEffectArchetype() => _vanguardEffectPreset;
        public override bool IgnoreSelf() => ignoreSelf;



        public override IEnumerable<PerformEffectValues> GetEffects()
        {
            yield return GenerateVanguardValues();
        }

        public override IEnumerable<PerformEffectValues> GetEffectsFeedBacks()
        {
            foreach (var effect in effects)
            {
                yield return effect.GenerateValues();
            }
        }

        public override EnumsSkill.Archetype Archetype => EnumsSkill.Archetype.Self;
        public override EnumsSkill.TargetType TargetType => EnumsSkill.TargetType.Direct;


        public PerformEffectValues GenerateVanguardValues() =>
            new PerformEffectValues(_vanguardEffectPreset, 1, EnumsEffect.TargetType.Performer);
        public PerformEffectValues GetVanguardEffectTooltip() => GenerateVanguardValues();



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

            public void DoEffect(CombatEntity performer, CombatEntity target, float effectValue)
            {
                var vanguardEffectsHolder = performer.Team.VanguardEffectsHolder;
                var type = _skill.responseType;

                vanguardEffectsHolder.AddEffect(_skill,type);
            }
        }
    }
}
