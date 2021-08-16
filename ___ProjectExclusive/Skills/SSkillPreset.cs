using System;
using System.Collections.Generic;
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
        menuName = "Combat/Skill/Skill Preset", order = -100)]
    public class SSkillPreset : ScriptableObject
    {
        [TitleGroup("Details"), PropertyOrder(-100)]
        [SerializeField, Delayed]
        protected string skillName = "NULL";
        [TitleGroup("Details"), PreviewField, GUIColor(.4f, .4f, .4f)]
        [SerializeField] private Sprite icon = null;

        [TitleGroup("Stats"), Range(0, 100), SuffixLabel("actions")]
        [SerializeField]
        private int cooldownCost = 1;
        [TitleGroup("Stats")]
        public bool canCrit = true;
        [TitleGroup("Stats"), Range(-10, 10), SuffixLabel("00%"), ShowIf("canCrit")]
        public float criticalAddition = 0f;

        [TitleGroup("Targeting")]
        [SerializeField]
        protected bool canTargetSelf = false;
        [SerializeField]
        protected EnumSkills.TargetingType skillType = EnumSkills.TargetingType.Support;

        [Title("Main Condition"),PropertyOrder(90)] 
        [SerializeField]
        private ConditionalUse conditionalUse;
        [TitleGroup("Effects"),PropertyOrder(100)]
        [SerializeField]
        private EffectParams[] effects = new EffectParams[1];




        public string SkillName => skillName;
        public Sprite Icon => icon;
        public bool CanTargetSelf() => canTargetSelf;
        public EnumSkills.TargetingType GetSkillType() => skillType;
        public int CoolDownCost => cooldownCost;
        public IEffect GetMainEffect() => effects[0];





        public List<CombatingEntity> GetMainEffectTargets(CombatingEntity user,CombatingEntity target)
        {
            return UtilsTargets.GetEffectTargets(user, target, effects[0].GetEffectTarget());
        }

        protected virtual void DoEffect(ref DoSkillArguments arguments, int effectIndex)
        {
            var user = arguments.User;
            var target = arguments.Target;
            var isCritical = arguments.IsCritical;

            var effect = effects[effectIndex];
            var effectTargets = UtilsTargets.GetEffectTargets(user, target, effect.GetEffectTarget());
            float randomModifier;
            UpdateRandomness();
            foreach (CombatingEntity effectTarget in effectTargets)
            {
                if (effect.CanPerformRandom())
                    UpdateRandomness();
                else
                    randomModifier = 1;

                effect.DoEffect(user, effectTarget, randomModifier);
            }


            void UpdateRandomness()
            {
                if (isCritical)
                {
                    randomModifier = UtilsCombatStats.RandomHigh;
                    return;
                }
                randomModifier = UtilsCombatStats.CalculateRandomModifier(Random.value);
            }
        }

        

        public void DoMainEffect(ref DoSkillArguments arguments)
            => DoEffect(ref arguments,0);
        public void DoSecondaryEffects(ref DoSkillArguments arguments)
        {
            for (int i = 1; i < effects.Length; i++)
            {
                DoEffect(ref arguments, i);
            }
        }


        public bool CanBeUse(CombatingEntity user)
        {
            return conditionalUse.CanBeUse(user);
        }

        /// <summary>
        /// Use this for doing the effect directly without animations nor waits
        /// </summary>
        public virtual void DoDirectEffects(CombatingEntity user, CombatingEntity target)
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
            return $" - [{cooldownCost}] - (" + mainEffect.GetEffectTarget().ToString().ToUpper() + ") ";
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

    public struct DoSkillArguments
    {
        public readonly CombatingEntity User;
        public readonly CombatingEntity Target;
        public bool IsCritical;

        public DoSkillArguments(CombatingEntity user, CombatingEntity target, bool isCritical)
        {
            User = user;
            Target = target;
            IsCritical = isCritical;
        }
    }
}
