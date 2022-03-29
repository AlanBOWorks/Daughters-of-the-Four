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
    public class SSkillPreset : ScriptableObject, IFullSkill, IEnumerator<IEffect>, IEnumerable<IEffect>
    {
        [SerializeField] 
        private string skillName = "NULL";

        [SerializeField, PreviewField] 
        private Sprite skillIcon;

        [SerializeField]
        private int skillCost;

        [SerializeField] 
        private EnumsSkill.Archetype archetype 
            = EnumsSkill.Archetype.Offensive;

        [SerializeField] private EnumsSkill.TargetType targetType 
            = EnumsSkill.TargetType.Direct;

        [SerializeField]
        private EffectValues[] effects = new EffectValues[0];

        [SerializeField,HideInInspector]
        private EffectWrapper effectWrapper = new EffectWrapper();


        public string GetSkillName() => skillName;
        public Sprite GetSkillIcon() => skillIcon;
        public int SkillCost => skillCost;
        public EnumsSkill.TargetType TargetType => targetType;
        public EnumsSkill.Archetype Archetype => archetype;

        public void DoSkill(in CombatEntity performer, in CombatEntity target, in CombatSkill holderReference)
        {
            UtilsSkillEffect.DoEffectsOnTarget(this, performer, target, holderReference);
        }

        // This is a wrapper for avoiding boxing
        // During the iterator, the EffectValue.Current should be injected in this (currentEffectValues) and
        // used as a IEffect by the it
        [Serializable]
        private sealed class EffectWrapper : IEffect
        {
            public EffectValues currentEffectValues;
            public EnumsEffect.TargetType TargetType => currentEffectValues.targetType;

            public void DoEffect(in CombatEntity performer, in CombatEntity target)
            {
                currentEffectValues.DoEffect(in performer, in target);
            }
        }

        [Serializable]
        private struct EffectValues : IEffect
        {
            public SEffect effect;
            public float effectValue;
            public EnumsEffect.TargetType targetType;
            public EnumsEffect.TargetType TargetType => targetType;

            public void DoEffect(in CombatEntity performer, in CombatEntity target)
            {
                var effectTargets =
                    UtilsTarget.GetEffectTargets(in performer, in target, targetType);
                foreach (var effectTarget in effectTargets)
                {
                    effect.DoEffect(in performer, in effectTarget, in effectValue);
                }
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


        private int _effectIndex = -1;
        public bool MoveNext()
        {
            _effectIndex++;
            return _effectIndex < effects.Length;
        }

        public void Reset()
        {
            _effectIndex = -1;
        }

        public IEffect Current
        {
            get
            {
                effectWrapper.currentEffectValues = effects[_effectIndex];
                return effectWrapper;
            }
        }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }

        public IEnumerator<IEffect> GetEnumerator()
        {
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
