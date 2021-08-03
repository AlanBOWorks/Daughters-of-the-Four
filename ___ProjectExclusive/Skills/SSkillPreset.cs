using System;
using ___ProjectExclusive;
using Characters;
using CombatEffects;
using Sirenix.OdinInspector;
using Stats;
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
    public class SSkillPreset : SEffectSetPreset
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

        [TitleGroup("Stats"), Range(0, 100), SuffixLabel("actions")]
        [SerializeField]
        private int cooldownCost = 1;
        public int CoolDownCost => cooldownCost;

        protected override string ValidationName(IEffect mainEffect)
        {
            return $" - [{cooldownCost}] - " + base.ValidationName(mainEffect);
        }
    }


    public abstract class SEffectSetPreset : ScriptableObject
    {
        [TitleGroup("Details")]
        [SerializeField, Delayed]
        protected string skillName = "NULL";

        [TitleGroup("Details")]
        [SerializeField] private Sprite icon = null;

        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;

        [Title("MainCondition")] 
        [SerializeField]
        private ConditionalUse conditionalUse;

        [TitleGroup("Effects")]
        [SerializeField]
        private EffectParams[] effects = new EffectParams[1];


        public string SkillName => skillName;
        public Sprite Icon => icon;
        public EffectParams[] GetEffects => effects;
        public IEffect GetMainEffect() => effects[0];


        public bool CanBeUse(CombatingEntity user)
        {
            return conditionalUse.CanBeUse(user);
        }

        /// <summary>
        /// Use this for doing the effect directly without animations nor waits
        /// </summary>
        public void DoDirectEffects(CombatingEntity user, CombatingEntity target)
        {
            DoEffects(user, target, 1);
        }
        
        public void DoEffects(CombatingEntity user, CombatingEntity target, float modifier)
        {
            DoEffects(user, target, modifier, effects);
        }

        protected void DoEffects(CombatingEntity user, CombatingEntity target, float modifier,
            EffectParams[] targetEffects)
        {
            float randomValue = Random.value;
            bool isCritical =
                UtilsCombatStats.IsCriticalPerformance(this, randomValue);
            UtilsCombatStats.UpdateRandomness(ref randomValue, isCritical);
            float effectsModifier = randomValue * modifier;
            foreach (EffectParams effect in targetEffects)
            {
                var targets = UtilsTargets.GetEffectTargets(user, target, effect.GetEffectTarget());
                foreach (CombatingEntity effectTarget in targets)
                {
                    effect.DoDirectEffect(effectTarget, effectsModifier);
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

        [Serializable]
        private struct ConditionalUse
        {
            public SSkillUseConditionBase useCondition;
            public float conditionCheck;
            public bool inverseCondition;

            public bool CanBeUse(CombatingEntity user)
            {
                if (useCondition == null) return true;

                bool canBeUse = useCondition.CanUseSkill(user, conditionCheck);
                return canBeUse ^ inverseCondition;
            }
        }
    }
}
