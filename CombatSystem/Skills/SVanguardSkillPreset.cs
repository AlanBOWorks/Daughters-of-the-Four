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
        [Title("Effects")]
        [InfoBox("Target Type [Target]: enemy who did the Offensive is the Target\n" +
                 "Target Type [Performer]: the main vanguard is the Performer")]
        [SerializeField]
        private VanguardEffectValues[] vanguardEffects = new VanguardEffectValues[0];

        [ShowInInspector, DisableInEditorMode]
        private VanguardEffect _vanguardEffectPreset;



        private void OnEnable()
        {
            _vanguardEffectPreset = new VanguardEffect(this);
        }


        public override IEffect GetMainEffectArchetype() => _vanguardEffectPreset;
        public override bool IgnoreSelf() => false;

        public override IEnumerable<PerformEffectValues> GetEffects()
        {
            yield return new PerformEffectValues(_vanguardEffectPreset,1,EnumsEffect.TargetType.Performer);
        }

        public override EnumsSkill.Archetype Archetype => EnumsSkill.Archetype.Self;
        public override EnumsSkill.TargetType TargetType => EnumsSkill.TargetType.Direct;

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
                var effects = _skill.vanguardEffects;
                var vanguardEffectsHolder = performer.Team.VanguardEffectsHolder;
                foreach (var values in effects)
                {
                    var type = values.responseType;
                    var effectValues = values.effectValues;
                    vanguardEffectsHolder.AddEffect(_skill,type,effectValues.GenerateValues());
                }
            }
        }

        [Serializable]
        private struct VanguardEffectValues
        {
            public EnumsVanguardEffects.VanguardEffectType responseType;
            public PresetEffectValues effectValues;


        }
    }
}
