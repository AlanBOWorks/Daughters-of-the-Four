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

    public abstract class SSkillPresetBase : SEffectSetPreset
    {
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
    }

    public abstract class SEffectSetPreset : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField, Delayed]
        protected string skillName = "NULL";
        public string SkillName => skillName;

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;
        public Sprite Icon => icon;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;


        [TitleGroup("Effects")]
        [SerializeField]
        private EffectParams[] effects = new EffectParams[1];
        public EffectParams[] GetEffects => effects;
        public IEffect GetMainEffect() => effects[0];

        /// <summary>
        /// Use this for doing the effect directly without animations nor waits
        /// </summary>
        public void DoDirectEffects(CombatingEntity user, CombatingEntity target)
            => DoDirectEffects(user, target, 1);
        public void DoDirectEffects(CombatingEntity user, CombatingEntity target, float modifier)
        {
            float randomValue = Random.value;
            bool isCritical =
                UtilsCombatStats.IsCriticalPerformance(this, randomValue);
            UtilsCombatStats.UpdateRandomness(ref randomValue, isCritical);
            float effectsModifier = randomValue * modifier;
            foreach (EffectParams effect in effects)
            {
                var targets = UtilsTargets.GetEffectTargets(user, target, effect.GetEffectTarget());
                foreach (CombatingEntity effectTarget in targets)
                {
                    effect.DoDirectEffect(effectTarget,effectsModifier);
                }
            }
        }


        [Button(ButtonSizes.Large)]
        protected virtual void UpdateAssetName()
        {
            ValidateEffects();
            var mainEffect = GetMainEffect();
            if (mainEffect != null)
            {
                name = FullAssetName(mainEffect);
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

        private const string PresetPrefix = " - [SKILL Preset]";
        protected virtual string FullAssetName(IEffect mainEffect)
        {
            return skillName + ValidationName(mainEffect) + PresetPrefix;
        }
        protected virtual string ValidationName(IEffect mainEffect)
        {
            return "(" + mainEffect.GetEffectTarget().ToString().ToUpper() + ") ";
        }
    }
}
