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
        [SerializeField, GUIColor(.3f,.6f,.9f),VerticalGroup("UI"), TableColumnWidth(120)]
        private string skillName = "NULL_SKILL";

        [SerializeField, PreviewField(ObjectFieldAlignment.Center), GUIColor(.2f, .2f, .2f),TableColumnWidth(20)]
        private Sprite specialSprite;

        [Title("Params")]
        [SerializeField, EnumPaging, GUIColor(.7f,.2f,.2f)]
        [VerticalGroup("Params")]
        private EnumSkills.TargetType skillTargetType = EnumSkills.TargetType.Offensive;
        [VerticalGroup("Params")]
        [SerializeField]
        private EnumEffects.TargetType effectTargetType = EnumEffects.TargetType.Target;

        [VerticalGroup("Params"), TableColumnWidth(150)]
        [SerializeField,
         Tooltip("Effect reference for a special case scenario where the main effect is not the descriptive effect")]
        private SSkillComponentEffect specialDescriptiveEffect;

        [VerticalGroup("Params")]
        [SerializeField]
        private int useCost = 1;

        [VerticalGroup("Params")]
        [SerializeField, ShowIf("CanCrit")]
        private float critVariation;

        [Title("Effects")]
        [VerticalGroup("MainEffect"),TableColumnWidth(150)]
        [GUIColor(1f,.8f,.8f)]
        [SerializeField, ToggleLeft]
        private bool isMainEffectAfterListEffects;
        [VerticalGroup("MainEffect")]
        [GUIColor(1f, .8f, .8f)]
        [SerializeField]
        private EffectParameter mainEffect = new EffectParameter();

        [VerticalGroup("Secondary Effect"),TableColumnWidth(150)]
        [SerializeField]
        private List<EffectParameter> effects = new List<EffectParameter>();

        public void UpdateTargetType(EnumSkills.TargetType type)
        {
            skillTargetType = type;
        }

        public string GetSkillName() => skillName;
        public Sprite GetIcon() => specialSprite;
        public EnumSkills.TargetType GetTargetType() => skillTargetType;
        public EnumEffects.TargetType GetEffectTargetType() => effectTargetType;

        public int GetUseCost() => useCost;

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
        [TableColumnWidth(100)]
        [Button,VerticalGroup("Actions")]
        private void AddEffect(SEffect effect)
        {
            var addition = new EffectParameter
            {
                preset = effect,
                effectValue = 1,
            };
            effects.Add(addition);
        }

        [Button, VerticalGroup("Actions")]
        private void AddBuff(SBuff buff)
        {
            var addition = new EffectParameter
            {
                preset = buff,
                effectValue = 1,
                buffType = EnumStats.BuffType.Buff
            };
            effects.Add(addition);
        }

        [Button(ButtonSizes.Large), GUIColor(.3f, .6f, .9f), VerticalGroup("Actions")]
        private void UpdateAssetName()
        {
            name = GetTargetType().ToString().ToUpper() + " - " +skillName + " [Skill]";
            UtilsAssets.UpdateAssetName(this);
        }
#endif


    }


}
