using System.Collections.Generic;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatSkills
{
    [CreateAssetMenu(fileName = "N [Skill Preset]",
        menuName = "Combat/Skills/Preset")]
    public class SSkill : ScriptableObject,ISkill
    {
        [Title("UI")]
        [SerializeField, GUIColor(.3f,.6f,.9f)]
        private string skillName = "NULL_SKILL";

        [SerializeField, PreviewField, GUIColor(.2f, .2f, .2f)]
        private Sprite specialSprite;

        [Title("Params")]
        [SerializeField, EnumPaging, GUIColor(.7f,.2f,.2f)]
        private EnumSkills.TargetType skillTargetType = EnumSkills.TargetType.Self;


        [SerializeField,
         Tooltip("Effect reference for a special case scenario where the main effect is not the descriptive effect")]
        private SSkillComponentEffect specialDescriptiveEffect;

        [SerializeField]
        private int cooldownAmount = 1;

        [SerializeField, ShowIf("CanCrit")]
        private float critVariation;

        [Title("Effects")]
        [BoxGroup("Main Effect"), GUIColor(1f,.8f,.8f)]
        [SerializeField, ToggleLeft]
        private bool isMainEffectAfterListEffects;
        [BoxGroup("Main Effect"), GUIColor(1f, .8f, .8f)]
        [SerializeField]
        private EffectParameter mainEffect = new EffectParameter();

        [SerializeField]
        private List<EffectParameter> effects = new List<EffectParameter>();

        public void UpdateTargetType(EnumSkills.TargetType type)
        {
            skillTargetType = type;
        }

        public string GetSkillName() => skillName;
        public Sprite GetIcon() => specialSprite;
        public EnumSkills.TargetType GetTargetType() => skillTargetType;
        public int GetCooldownAmount() => cooldownAmount;

        public bool CanCrit()
        {
            if (mainEffect.canCrit) return true;

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
                preset = effect,
                effectValue = 1,
                targetType = EnumEffects.TargetType.Target
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

        [Button(ButtonSizes.Large), GUIColor(.3f, .6f, .9f)]
        private void UpdateAssetName()
        {
            name = GetTargetType().ToString().ToUpper() + " - " +skillName + " [Skill]";
            UtilsAssets.UpdateAssetName(this);
        }
#endif


    }


}
