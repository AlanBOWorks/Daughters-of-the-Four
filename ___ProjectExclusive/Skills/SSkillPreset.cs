using System;
using System.Collections.Generic;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Skills
{
    /// <summary>
    /// Global presets to get generic values; <br></br>
    /// <seealso cref="Skill"/> are the specific skills for individual <seealso cref="SCharacterSkillsPreset"/>
    /// </summary>
    [CreateAssetMenu(fileName = "N (T) - SKILL L - [Preset]",
        menuName = "Combat/Skill/Skill Preset")]
    public class SSkillPreset : SSkillPresetBase
    {
        [TitleGroup("Stats"), Range(0, 100), SuffixLabel("actions")]
        [SerializeField]
        private int cooldownCost = 1;
        public int CoolDownCost => cooldownCost;

        protected override string ValidationName(IEffect mainEffect)
        {
            return " - [{ cooldownCost}] - " + base.ValidationName(mainEffect);
        }
    }

    public abstract class SSkillPresetBase : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField,Delayed] 
        private string skillName = "NULL";
        public string SkillName => skillName;

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;
        public Sprite Icon => icon;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;
        

        [TitleGroup("Targeting")]
        [SerializeField] protected bool canTargetSelf = false;
        public bool CanTargetSelf() => canTargetSelf;
        [SerializeField] protected SkillType skillType = SkillType.Support;
        public SkillType GetSkillType() => skillType;
        public enum SkillType
        {
            SelfOnly,
            Offensive,
            Support,
            Other = SelfOnly
        }


        [TitleGroup("Effects")]
        [SerializeField]
        private EffectParams[] effects = new EffectParams[1];
        public EffectParams[] GetEffects => effects;
        public IEffect GetMainEffect() => effects[0];

        public void DoEffects(CombatingEntity user, CombatingEntity target)
        {
            float randomValue = Random.value;
            bool isCritical =
                UtilsCombatStats.IsCriticalPerformance(user.CombatStats, this, randomValue);
            UtilsCombatStats.UpdateRandomness(ref randomValue,isCritical);
            foreach (EffectParams effect in effects)
            {
                effect.DoEffect(user,target, randomValue);
            }
        }

        public void DoEffects(CombatingEntity target)
        {
            float randomValue = Random.value;
            bool isCritical =
                UtilsCombatStats.IsCriticalPerformance(this, randomValue);
            UtilsCombatStats.UpdateRandomness(ref randomValue, isCritical);
            foreach (EffectParams effect in effects)
            {
                effect.DoDirectEffect(target, randomValue);
            }
        }


        [Button(ButtonSizes.Large)]
        protected virtual void UpdateAssetName()
        {
            ValidateEffects();
            var mainEffect = GetMainEffect();
            if (mainEffect != null)
            {
                string typeString = ValidationName(mainEffect);
                name = skillName + typeString + " - [SKILL Preset]";
            }
            UtilsGame.UpdateAssetName(this);
        }

        protected void ValidateEffects()
        {
            foreach (EffectParams effectParams in effects)
            {
                effectParams.OnValidateEffects();
            }
        }

        protected virtual string ValidationName(IEffect mainEffect)
        {
            return "(" + mainEffect.GetEffectTarget().ToString().ToUpper() + ") ";
        }
    }

}
