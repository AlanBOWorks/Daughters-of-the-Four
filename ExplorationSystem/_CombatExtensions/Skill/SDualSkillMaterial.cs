using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ExplorationSystem._CombatExtensions
{
    [CreateAssetMenu(fileName = "N " + AssetPrefix, menuName = "Combat/Exploration/Dual Material")]
    public class SDualSkillMaterial : ScriptableObject, IDualSkillMaterial
    {
        private const string AssetPrefix = "(Dual Material)";

        [SerializeField] 
        private string materialName;
        [SerializeField, PreviewField, GUIColor(.3f,.3f,.3f,.5f)] 
        private Sprite icon;


        [SerializeField]
        private EffectValues[] effects = new EffectValues[0];

        private void Awake()
        {
            materialName = name;
        }

        public string GetSkillName() => materialName;

        public Sprite GetSkillIcon() => icon;
        public IEffect GetMainEffectArchetype() => effects[0].effect;

        public IEnumerable<PerformEffectValues> GetEffects()
        {
            for (var i = 0; i < effects.Length; i++)
            {
                var effect = effects[i];
                yield return effect.GenerateValues();
            }
        }

        [Serializable]
        private struct EffectValues
        {
            [SuffixLabel("$SuffixByType")]
            public SEffect effect;
            public float effectValue;
            public EnumsEffect.TargetType targetType;

            public PerformEffectValues GenerateValues()
            {
                return new PerformEffectValues(effect,effectValue,targetType);
            }


            private string SuffixByType()
            {
                return LocalizeEffects.GetEffectValueSuffix(effect);
            }
        }
    }

    public interface IDualSkillMaterial : ISkillInfoHolder, IEffectsHolder { }
}
