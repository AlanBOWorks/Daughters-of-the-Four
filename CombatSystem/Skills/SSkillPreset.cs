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
    public class SSkillPreset : ScriptableObject, IFullSkill
    {

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

        [Title("Effects"),
         InfoBox("Null: mainEffect will be taken from [effects].first element", "IsMainEffectNull")] 
        [SerializeField] private SEffect mainEffectReference;
        
        [SerializeField]
        private PresetEffectValues[] effects = new PresetEffectValues[0];

        private bool IsMainEffectNull() => !mainEffectReference;
        public bool HasEffects() => effects.Length > 0;

        public string GetSkillName() => skillName;
        public Sprite GetSkillIcon() => skillIcon;

        public IEnumerable<PerformEffectValues> GetEffects()
        {
            for (int i = 0; i < effects.Length; i++)
            {
                yield return effects[i].GenerateValues();
            }
        }

        public int SkillCost => skillCost;
        public EnumsSkill.TargetType TargetType => targetType;
        public EnumsSkill.Archetype Archetype => archetype;
        internal PresetEffectValues[] GetEffectValues() => effects;


        public IEffect GetMainEffectArchetype()
        {
            IEffect mainEffect = mainEffectReference;
            if (mainEffect == null && HasEffects())
                mainEffect = effects[0].GetPreset();


            return mainEffect;
        }

        public bool IgnoreSelf() => ignoreSelf && archetype != EnumsSkill.Archetype.Self;
        



        [Button]
        private void UpdateAssetName()
        {
            string generatedName = skillName;
            generatedName = archetype.ToString().ToUpper() + " - " + generatedName;
            generatedName += " [SkillPreset]";

            UtilsAssets.UpdateAssetNameWithID(this, generatedName);
        }



        [Serializable]
        internal struct PresetEffectValues : IEffectPreset
        {
            [SerializeField]
            private SEffect effect;
            [SerializeField]
            private float effectValue;
            [SerializeField]
            private EnumsEffect.TargetType targetType;

            public IEffect GetPreset() => effect;
            public EnumsEffect.TargetType TargetType => targetType;
            public float GetValue() => effectValue;

            public PerformEffectValues GenerateValues() => new PerformEffectValues(effect,in effectValue,in targetType);
        }
    }


}
