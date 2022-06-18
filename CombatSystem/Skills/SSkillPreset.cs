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
        [TitleGroup("Values")]
        [SerializeField] private bool ignoreSelf;

        [TitleGroup("Values")]
        [SerializeField]
        private EnumsSkill.Archetype archetype 
            = EnumsSkill.Archetype.Offensive;

        [TitleGroup("Values")]
        [SerializeField]
        private EnumsSkill.TargetType targetType 
            = EnumsSkill.TargetType.Direct;

        [TitleGroup("Effects"),
         InfoBox("Null: mainEffect will be taken from [effects].first element", "IsMainEffectNull")]
        [SerializeField] private SEffect mainEffectReference;



        

        public override IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => GetEffects();

        public override EnumsSkill.TargetType TargetType => targetType;
        public override EnumsSkill.Archetype Archetype => archetype;
        public override IEffect GetMainEffectArchetype()
        {
            IEffect mainEffect = mainEffectReference;
            if (mainEffect == null && HasEffects())
                mainEffect = effects[0].GetPreset();


            return mainEffect;
        }
        public override bool IgnoreSelf() => ignoreSelf && Archetype != EnumsSkill.Archetype.Self;


       
    }

    public abstract class SSkillPresetBase : ScriptableObject, IFullSkill
    {
        [TitleGroup("ToolTips")]
        [SerializeField]
        private string skillName = "NULL";

        [SerializeField, PreviewField]
        private Sprite skillIcon;

        [TitleGroup("Values")]
        [SerializeField]
        private int skillCost = 1;

        [TitleGroup("Effects")]
        [SerializeField]
        private protected PresetEffectValues[] effects = new PresetEffectValues[0];


        public string GetSkillName() => skillName;
        public Sprite GetSkillIcon() => skillIcon;
        public virtual IEnumerable<PerformEffectValues> GetEffects()
        {
            for (int i = 0; i < effects.Length; i++)
            {
                yield return effects[i].GenerateValues();
            }
        }
        internal PresetEffectValues[] GetEffectValues() => effects;


        public abstract IEnumerable<PerformEffectValues> GetEffectsFeedBacks();
        public int SkillCost => skillCost;
        public abstract EnumsSkill.Archetype Archetype { get; }
        public abstract EnumsSkill.TargetType TargetType { get; }



        public abstract IEffect GetMainEffectArchetype();
        public abstract bool IgnoreSelf();
        public bool HasEffects() => effects.Length > 0;


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

            public readonly PerformEffectValues GenerateValues() => new PerformEffectValues(effect, effectValue, targetType);

            private string SuffixByType()
            {
                return LocalizeEffects.GetEffectValueSuffix(effect);
            }
        }
    }
}
