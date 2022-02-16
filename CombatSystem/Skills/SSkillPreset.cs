using System;
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
    public class SSkillPreset : ScriptableObject, IFullSkill
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
            var eventsHolder = CombatSystemSingleton.EventsHolder;
            foreach (var effect in effects)
            {
                effectWrapper.currentEffectValues = effect;
                effect.DoEffect(in performer, in target);
                eventsHolder.OnEffectPerform(in performer, in holderReference, in target, effectWrapper);
            }
        }


        [Serializable]
        private sealed class EffectWrapper : IEffect
        {
            public EffectValues currentEffectValues;
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
    }
}
