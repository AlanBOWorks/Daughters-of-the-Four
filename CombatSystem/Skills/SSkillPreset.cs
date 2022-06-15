using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skill/Single Preset")]
    public class SSkillPreset : SSkillPresetBase
    {
        [Title("Targeting")]
        [SerializeField] private bool ignoreSelf;

        [SerializeField] 
        private EnumsSkill.Archetype archetype 
            = EnumsSkill.Archetype.Offensive;

        [SerializeField] private EnumsSkill.TargetType targetType 
            = EnumsSkill.TargetType.Direct;

        [Title("Effects"),
         InfoBox("Null: mainEffect will be taken from [effects].first element", "IsMainEffectNull")]
        [SerializeField] private SEffect mainEffectReference;

        [SerializeField]
        private PresetEffectValues[] effects = new PresetEffectValues[0];


        public override IEnumerable<PerformEffectValues> GetEffects()
        {
            for (int i = 0; i < effects.Length; i++)
            {
                yield return effects[i].GenerateValues();
            }
        }

        public override EnumsSkill.TargetType TargetType => targetType;
        public override EnumsSkill.Archetype Archetype => archetype;
        internal PresetEffectValues[] GetEffectValues() => effects;
        public bool HasEffects() => effects.Length > 0;
        public override bool IgnoreSelf() => ignoreSelf && Archetype != EnumsSkill.Archetype.Self;


        public override IEffect GetMainEffectArchetype()
        {
            IEffect mainEffect = mainEffectReference;
            if (mainEffect == null && HasEffects())
                mainEffect = effects[0].GetPreset();


            return mainEffect;
        }
    }

    public abstract class SSkillPresetBase : ScriptableObject, IFullSkill
    {
        [Title("ToolTips")]
        [SerializeField]
        private string skillName = "NULL";

        [SerializeField, PreviewField]
        private Sprite skillIcon;

        [Title("Values")]
        [SerializeField]
        private int skillCost = 1;




        public string GetSkillName() => skillName;
        public Sprite GetSkillIcon() => skillIcon;
        public abstract IEnumerable<PerformEffectValues> GetEffects();
        public int SkillCost => skillCost;
        public abstract EnumsSkill.Archetype Archetype { get; }
        public abstract EnumsSkill.TargetType TargetType { get; }



        public abstract IEffect GetMainEffectArchetype();
        public abstract bool IgnoreSelf();


        [Button]
        private void UpdateAssetName()
        {
            string generatedName = skillName;
            generatedName = Archetype.ToString().ToUpper() + " - " + generatedName;
            generatedName += " [SkillPreset]";

            UtilsAssets.UpdateAssetNameWithID(this, generatedName);
        }


        [Serializable]
        internal struct PresetEffectValues : IEffectPreset
        {
            [SerializeField]
            private SEffect effect;
            [SerializeField, SuffixLabel("$SuffixByType")]
            private float effectValue;
            [SerializeField]
            private EnumsEffect.TargetType targetType;

            public IEffect GetPreset() => effect;
            public EnumsEffect.TargetType TargetType => targetType;
            public float GetValue() => effectValue;

            public PerformEffectValues GenerateValues() => new PerformEffectValues(effect, effectValue, targetType);

            private string SuffixByType()
            {
                return LocalizeEffects.GetEffectValueSuffix(effect);
            }


        }
    }
}
