using System;
using System.Collections.Generic;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEditor;
using UnityEngine;

namespace CombatSkills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skills/Preset")]
    public class SSkill : ScriptableObject
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
        [SerializeField,EnumPaging] 
        private EnumSkills.TargetType skillTargetType = EnumSkills.TargetType.Self;


        [SerializeField,
         Tooltip("Effect reference for a special case scenario where the main effect is not the descriptive effect")] 
        private SSkillComponentEffect specialDescriptiveEffect;

        [SerializeField] 
        private int cooldownAmount = 1;

        [SerializeField, ShowIf("CanCrit")] 
        private float critVariation;

        [Title("Effects")]
        [BoxGroup("Main Effect")]
        [SerializeField,ToggleLeft] 
        private bool isMainEffectAfterListEffects;
        [BoxGroup("Main Effect")]
        [SerializeField]
        private EffectParameter mainEffect = new EffectParameter();

        [SerializeField]
        private List<EffectParameter> effects = new List<EffectParameter>();

        public void UpdateTargetType(EnumSkills.TargetType type)
        {
            skillTargetType = type;
        }

        public void UpdateName(string name)
        {
            skillName = name;
        }

        public string GetSkillName() => skillName;
        public Sprite GetIcon() => specialSprite;
        public EnumSkills.TargetType GetTargetType() => skillTargetType;
        public int GetCooldownAmount() => cooldownAmount;

        public bool CanCrit()
        {
            for (var i = 0; i < effects.Count; i++)
            {
                EffectParameter effect = effects[i];
                if (effect.canCrit) return true;
            }

            return false;
        }
        public float GetCritVariation() => critVariation;

        public ISkillComponent GetDescriptiveEffect()
        {
            return specialDescriptiveEffect 
                ? specialDescriptiveEffect 
                : effects[0].preset;
        }

        public bool IsMainEffectAfterListEffects => isMainEffectAfterListEffects;
        public EffectParameter GetMainEffect() => mainEffect;

        public List<EffectParameter> GetEffects() => effects;

#if UNITY_EDITOR
        [Button]
        private void AddEffect(SEffect effect)
        {
            var addition = new EffectParameter
            {
                preset = effect, effectValue = 1, targetType = EnumEffects.TargetType.Target
            };
            effects.Add(addition);
        }

        [Button]
        private void AddBuff(SBuff buff)
        {
            var addition = new EffectParameter
            {
                preset = buff,
                effectValue = 1,
                targetType = EnumEffects.TargetType.Target,
                buffType = EnumStats.BuffType.Buff
            };
            effects.Add(addition);
        }
#endif
    }

}
