using System;
using System.Collections.Generic;
using Characters;
using CombatConditions;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatEffects
{

    public abstract class SEffectBase : ScriptableObject, IEffectBase
    {
        [SerializeField, PreviewField, GUIColor(.4f, .4f, .4f)] 
        private Sprite effectIcon;
        public abstract void DoEffect(SkillArguments arguments,CombatingEntity target, float effectModifier = 1);
        public abstract void DoEffect(CombatingEntity target, float effectModifier);

        public Sprite GetEffectIcon() => effectIcon;

        /// <summary>
        /// Checks if the effect has fail in the random check
        /// </summary>
        protected static bool FailRandom(float effectModifier)
        {
            return Random.value > effectModifier || effectModifier <= 0;
        }

        public enum EffectTarget
        {
            Target,
            /// <summary>
            /// The target team excluding the Target
            /// </summary>
            TargetTeamExcluded,
            TargetTeam,
            Self,
            SelfTeam,
            SelfTeamNotIncluded,
            All
        }

        protected void RenameAsset()
        {
            string assetPath = AssetDatabase.GetAssetPath(this.GetInstanceID());
            AssetDatabase.RenameAsset(assetPath, name);
        }
    }


    [Serializable]
    public class EffectParamsBase 
    {
        [TitleGroup("Preset"), PropertyOrder(-100)]
        [SerializeField] protected SEffectBase effectPreset;

        [TitleGroup("Stats")]
        [Range(-10, 10), SuffixLabel("%00")]
        public float power = 1;
        [Tooltip("If the effect will apply a small variation into the [Power] value")]
        public bool applyRandomness = true;

        [TitleGroup("Targeting")]
        [SerializeField] 
        protected SEffectBase.EffectTarget effectTarget 
            = SEffectBase.EffectTarget.Target;

        public SEffectBase Preset => effectPreset;
        public SEffectBase.EffectTarget GetEffectTarget() => effectTarget;
        public bool CanPerformRandom() => applyRandomness;


    }
    [Serializable]
    public class EffectParams : EffectParamsBase, IEffect
    {
        [TitleGroup("Conditions")] 
        [SerializeField, ShowIf("HasEffects")]
        private ConditionParam effectCondition;

        [SerializeField, ShowIf("HasCondition")]
        private FailEffectParams[] onFailEffects = new FailEffectParams[0];


        public Sprite GetEffectIcon() => effectPreset.GetEffectIcon();

        protected bool HasEffects() => effectPreset != null;
        protected bool HasCondition() => effectCondition.HasCondition();

        public void DoEffect(SkillArguments arguments,CombatingEntity target, float randomModifier)
        {
            var user = arguments.User;
            bool canApplyEffect = true;
            float effectPower = power * randomModifier;
            if (effectCondition.HasCondition())
            {
                canApplyEffect = effectCondition.CanApply(user,target);
            }

            if (canApplyEffect)
            {
                float powerAddition = 0;
                user.PassivesHolder.EffectFilters.DoFilterOnAction(effectPreset,ref powerAddition);
                target.PassivesHolder.EffectFilters.DoFilterOnReaction(effectPreset,ref powerAddition);

                effectPower *= 1 + powerAddition;
                if(effectPower < 0) effectPower = 0;

                effectPreset.DoEffect(arguments, target, effectPower);
            }
            else
            {
                // If it can apply the effect simply go to fail state and calculate from there;
                // It's not necessary apply the target.passives since there's no effect to calculate
                var failEffects = onFailEffects;
                foreach (var failEffect in failEffects)
                {
                    failEffect.DoEffect(arguments, target, randomModifier);
                }
            }

        }


        public void DoDirectEffect(CombatingEntity target, float randomModifier)
        {
            float powerVariation = power;
            if (applyRandomness)
                powerVariation *= randomModifier;
            effectPreset.DoEffect(target,powerVariation);
        }

        public void OnValidateEffects()
        {
            if(HasEffects()) return;
            onFailEffects = new FailEffectParams[0];
        }

        public bool CanApplyOnTarget(CombatingEntity entity)
        {
            return entity.IsConscious();
        }
    }

    [Serializable]
    public class FailEffectParams : EffectParamsBase
    {

        public void DoEffect(SkillArguments arguments, CombatingEntity target, float randomModifier)
        {
            var user = arguments.User;
            var targets = UtilsTargets.GetEffectTargets(user, target, effectTarget);
            foreach (CombatingEntity failTarget in targets)
            {
                effectPreset.DoEffect(arguments,failTarget,randomModifier);
            }
        }

    }

    public interface IEffect : IEffectBase
    {
        SEffectBase.EffectTarget GetEffectTarget();

        bool CanPerformRandom();
        bool CanApplyOnTarget(CombatingEntity entity);

        Sprite GetEffectIcon();
    }

    public interface IEffectBase
    { 
        void DoEffect(SkillArguments arguments, CombatingEntity target, float randomModifier);
    }



}
