using System;
using System.Collections.Generic;
using CombatSystem.Localization;
using CombatSystem.Skills.Effects;
using CombatSystem.Skills.VanguardEffects;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Skills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skill/Single Preset", order = -100)]
    public class SSkillPreset : SSkillPresetBase
    {
        [TitleGroup("Values")]
        [SerializeField] private bool ignoreSelf;

        [TitleGroup("Values")]
        [SerializeField]
        private EnumsSkill.TeamTargeting teamTargeting 
            = EnumsSkill.TeamTargeting.Offensive;

       
        [Title("References"),
         InfoBox("Main Effect Reference (Null): mainEffect will be taken from effects", 
             "IsMainEffectNull", 
             InfoMessageType = InfoMessageType.Warning)]
        [SerializeField] private SEffect mainEffectReference;
        private bool IsMainEffectNull() => !mainEffectReference;
        

        public override IEnumerable<PerformEffectValues> GetEffectsFeedBacks() => GetEffects();

        public override EnumsSkill.TeamTargeting TeamTargeting => teamTargeting;
        public override IEffect GetMainEffectArchetype()
        {
            IEffect mainEffect = mainEffectReference;
            if (mainEffect == null && HasEffects())
            {
                mainEffect = sharedTargetingEffects[0].GetPreset() ?? effects[0].GetPreset();
            }


            return mainEffect;
        }
        public override bool IgnoreSelf() => ignoreSelf && TeamTargeting != EnumsSkill.TeamTargeting.Self;


       
    }

    public abstract class SSkillPresetBase : ScriptableObject, IFullSkill
    {
        [TitleGroup("ToolTips")]
        [SerializeField]
        private string skillName = "NULL";

        [SerializeField] 
        private EnumCombat.QualityTier quality;


        [SerializeField, PreviewField, GUIColor(.1f,.1f,.1f,.9f)]
        private Sprite skillIcon;

        [TitleGroup("Values")] 
        [SerializeField, Range(-10,10)]
        private int skillCost = 1;

        [SerializeField, SuffixLabel("%"), Tooltip("0:  crit won't be calculated")]
        private LuckModifierType luckModifierType;

        private const float LowLuckModifier = .25f;
        private const float MidLuckModifier = .375f;
        private const float HighLuckModifier = .5f;
        private enum LuckModifierType
        {
            NoLuck = 0,
            Low,
            Mid,
            High
        }

        [TitleGroup("Effects")]
        [SerializeField] 
        private EnumsEffect.TargetType sharedTargeting = EnumsEffect.TargetType.Target;
        
        [TitleGroup("Effects")]
        [SerializeField, DisableIf("IsMostDesired")]
        private protected SharedTargetPresetEffectValues[] sharedTargetingEffects = new SharedTargetPresetEffectValues[0];
        [TitleGroup("Effects")]
        [SerializeField]
        private protected PresetEffectValues[] effects = new PresetEffectValues[0];

        [ShowInInspector,HideInEditorMode,DisableInPlayMode, NonSerialized]
        private IEnumerable<PerformEffectValues> _combatEffects;

        protected virtual void OnEnable()
        {
            _combatEffects = GetEffects();


            IEnumerable<PerformEffectValues> GetEffects()
            {
                if (!IsMostDesired())
                {
                    foreach (var effect in sharedTargetingEffects)
                    {
                        yield return effect.GenerateValues(sharedTargeting);
                    }
                }
                foreach (var effect in effects)
                {
                    yield return effect.GenerateValues();
                }
            }
        }

        protected virtual void OnDisable()
        {
            _combatEffects = null;
        }



        public string GetSkillName() => skillName;
        public EnumCombat.QualityTier GetSkillQuality() => quality;
        public Sprite GetSkillIcon() => skillIcon;
        public virtual IEnumerable<PerformEffectValues> GetEffects() => _combatEffects;


        public abstract IEnumerable<PerformEffectValues> GetEffectsFeedBacks();
        public int SkillCost => skillCost;
        public abstract EnumsSkill.TeamTargeting TeamTargeting { get; }



        public abstract IEffect GetMainEffectArchetype();
        public abstract bool IgnoreSelf();

        public float LuckModifier
        {
            get
            {
                return luckModifierType switch
                {
                    LuckModifierType.Low => LowLuckModifier,
                    LuckModifierType.Mid => MidLuckModifier,
                    LuckModifierType.High => HighLuckModifier,
                    _ => 0
                };
            }
        }
        public bool HasEffects() => sharedTargetingEffects.Length > 0 || effects.Length > 0;

        protected virtual string GetAssetPrefix() => " [SkillPreset]";

        private bool IsMostDesired() => sharedTargeting == EnumsEffect.TargetType.MostDesired;
        protected virtual string GenerateAssetName()
        {
            string generatedName = skillName;
            string skillTargetingText = (IsMostDesired()) ? null : sharedTargeting.ToString();
            generatedName = $"#{TeamTargeting.ToString().ToUpper()} - {generatedName} - #{skillTargetingText} #{quality} -";
            generatedName += GetAssetPrefix();
            return generatedName;
        }

        [Button]
        private void UpdateAssetName()
        {
            var generatedName = GenerateAssetName();
            UtilsAssets.UpdateAssetNameWithID(this, generatedName);
        }

        [Serializable]
        protected struct SharedTargetPresetEffectValues : IEffectPreset
        {
            [SerializeField]
            private SEffect effect;
            [SerializeField, SuffixLabel("$SuffixByType")]
            private float effectValue;

            public IEffect GetPreset() => effect;
            public float GetValue() => effectValue;
            public EnumsEffect.TargetType TargetType => EnumsEffect.TargetType.MostDesired;
            
            public readonly PerformEffectValues GenerateValues(EnumsEffect.TargetType targetType) 
                => new PerformEffectValues(effect, effectValue, targetType);

            private string SuffixByType()
            {
                return LocalizeEffects.GetEffectValueSuffix(effect);
            }
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

            public readonly PerformEffectValues GenerateValues() 
                => new PerformEffectValues(effect, effectValue, targetType);

            private string SuffixByType()
            {
                return LocalizeEffects.GetEffectValueSuffix(effect);
            }
        }
    }


    public interface IVanguardSkill : ISkill
    {
        EnumsVanguardEffects.VanguardEffectType GetVanguardEffectType();
        IEnumerable<PerformEffectValues> GetPerformVanguardEffects();
        int VanguardEffectCount { get; }
    }

    public interface IAttackerSkill : ISkill { }

    public interface ISupportSkill : ISkill { }
}
