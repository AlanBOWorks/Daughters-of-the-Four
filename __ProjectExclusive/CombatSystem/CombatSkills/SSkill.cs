using System;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace CombatSkills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skills/Preset")]
    public class SSkill : ScriptableObject, ISkill
    {
        [SerializeField] 
        private Skill skillParameters;
        public Skill GetSkillPreset() => skillParameters;


        [Button(ButtonSizes.Large), GUIColor(.6f,.8f,.6f)]
        public void UpdateAssetName()
        {
            var path = AssetDatabase.GetAssetPath(this);
            string assetName = skillParameters.GetTargetType().ToString().ToUpper() + " - " 
                                                                                    + skillParameters.GetSkillName()
                                                                                    + " [Skill]";
            name = assetName;
            AssetDatabase.RenameAsset(path, name);
        }

        public string GetSkillName() => skillParameters.GetSkillName();
        public Sprite GetIcon() => skillParameters.GetIcon();
        public EnumSkills.TargetType GetTargetType() => skillParameters.GetTargetType();
        public int GetCooldownAmount() => skillParameters.GetCooldownAmount();
        public bool CanCrit() => skillParameters.CanCrit();
        public float GetCritVariation() => skillParameters.GetCritVariation();
        public IEffect GetDescriptiveEffect() => skillParameters.GetDescriptiveEffect();
        public EffectParameter[] GetEffects() => skillParameters.GetEffects();
    }


    [Serializable]
    public class Skill : ISkill
    {
        [Title("UI")]
        [SerializeField, GUIColor(.1f, .9f, 1)] 
        private string skillName = "NULL_SKILL";

        [SerializeField, PreviewField,GUIColor(.2f,.2f,.2f)] 
        private Sprite specialSprite;

        [Title("Params")] 
        [SerializeField] 
        private EnumSkills.TargetType skillTargetType = EnumSkills.TargetType.Self;

        [SerializeField,
         Tooltip("Effect reference for a special case scenario where the main effect is not the descriptive effect")] 
        private SEffect specialDescriptiveEffect;

        [SerializeField] 
        private int cooldownAmount = 1;

        [SerializeField, ShowIf("CanCrit")] 
        private float critVariation;


        [SerializeField]
        private EffectParameter[] effects = new EffectParameter[0];


        public void UpdateTargetType(EnumSkills.TargetType type)
        {
            skillTargetType = type;
        }

        public void UpdateName(string name)
        {
            skillName = name;
        }

        public string GetSkillName()
        {
            return skillName;
        }

        public Sprite GetIcon()
        {
            return specialSprite;
        }

        public EnumSkills.TargetType GetTargetType()
        {
            return skillTargetType;
        }

        public int GetCooldownAmount()
        {
            return cooldownAmount;
        }

        public bool CanCrit()
        {
            for (var i = 0; i < effects.Length; i++)
            {
                EffectParameter effect = effects[i];
                if (effect.canCrit) return true;
            }

            return false;
        }

        public float GetCritVariation()
        {
            return critVariation;
        }

        public IEffect GetDescriptiveEffect()
        {
            return specialDescriptiveEffect 
                ? specialDescriptiveEffect 
                : effects[0].effectPreset;
        }

        public EffectParameter[] GetEffects()
        {
            return effects;
        }
    }

}
