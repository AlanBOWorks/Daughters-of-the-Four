using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skill/Single Preset")]
    public class SSkillPreset : ScriptableObject, IFullSkill, IEnumerable<IEffect>
    {
        private static readonly EffectWrapper EffectWrapperHelper = new EffectWrapper();

        [Title("ToolTips")]
        [SerializeField]
        private string skillName = "NULL";

        [SerializeField, PreviewField] 
        private Sprite skillIcon;

        [Title("Values")]
        [SerializeField]
        private int skillCost;

        [Title("Targeting")]
        [SerializeField] private bool ignoreSelf;
        [SerializeField] 
        private EnumsSkill.Archetype archetype 
            = EnumsSkill.Archetype.Offensive;

        [SerializeField] private EnumsSkill.TargetType targetType 
            = EnumsSkill.TargetType.Direct;

        [Title("Effects")]
        [SerializeField]
        private EffectValues[] effects = new EffectValues[0];



        public string GetSkillName() => skillName;
        public Sprite GetSkillIcon() => skillIcon;
        public int SkillCost => skillCost;
        public EnumsSkill.TargetType TargetType => targetType;
        public EnumsSkill.Archetype Archetype => archetype;

        public void DoSkill(in CombatEntity performer, in CombatEntity target, in CombatSkill holderReference)
        {
            var exclusion = (IgnoreSelf()) ? performer : null;
            UtilsSkillEffect.DoEffectsOnTarget(this, performer, exclusion, holderReference);
        }

        public bool IgnoreSelf() => ignoreSelf && archetype != EnumsSkill.Archetype.Self;
        public IEnumerable<IEffect> GetEffects()
        {
            foreach (var effect in effects)
            {
                EffectWrapperHelper.currentEffectValues = effect;
                yield return EffectWrapperHelper;
            }
        }


        // This is a wrapper for avoiding boxing
        // During the iterator, the EffectValue.Current should be injected in this (currentEffectValues) and
        // used as a IEffect by the it
        [Serializable]
        private sealed class EffectWrapper : IEffect
        {
            public EffectValues currentEffectValues;
            public SEffect GetPreset() => currentEffectValues.GetPreset();

            public EnumsEffect.TargetType TargetType => currentEffectValues.targetType;

            public void DoEffect(in CombatEntity performer)
            {
                currentEffectValues.DoEffect(in performer);
            }
        }


        [Button]
        private void UpdateAssetName()
        {
            string generatedName = skillName;
            generatedName = archetype.ToString().ToUpper() + " - " + generatedName;
            generatedName += " [SkillPreset]";

            UtilsAssets.UpdateAssetNameWithID(this, generatedName);
        }

        public IEnumerator<IEffect> GetEnumerator()
        {
            foreach (var effect in effects)
            {
                EffectWrapperHelper.currentEffectValues = effect;
                yield return EffectWrapperHelper;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    [Serializable]
    internal struct EffectValues : IEffect
    {
        public SEffect effect;
        public float effectValue;
        public EnumsEffect.TargetType targetType;

        public SEffect GetPreset() => effect;

        public EnumsEffect.TargetType TargetType => targetType;

        public void DoEffect(in CombatEntity performer)
        {
            var effectTargets =
                UtilsTarget.GetEffectTargets(targetType);
            foreach (var effectTarget in effectTargets)
            {
                effect.DoEffect(in performer, in effectTarget, in effectValue);
            }
        }
    }
}
