using System;
using Characters;
using CombatConditions;
using Sirenix.OdinInspector;
using Skills;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CombatEffects
{

    public abstract class SEffectBase : ScriptableObject
    {
        public abstract void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1);

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
        [Range(0, 10), SuffixLabel("%00")]
        public float power = 1;
        [Tooltip("If the effect will apply a small variation into the [Power] value")]
        public bool applyRandomness = true;

        [TitleGroup("Targeting")]
        [SerializeField] 
        protected SEffectBase.EffectTarget effectTarget 
            = SEffectBase.EffectTarget.Target;


        public SEffectBase.EffectTarget GetEffectTarget() => effectTarget;
        public bool CanPerformRandom() => applyRandomness;
    }
    [Serializable]
    public class EffectParams : EffectParamsBase, IEffect
    {
        [TitleGroup("Conditions")]
        [SerializeField]
        private ConditionParam[] effectConditions = new ConditionParam[0];
        protected bool HasEffects() => effectConditions.Length > 0;
        [SerializeField, ShowIf("HasEffects")]
        private FailEffectParams[] onFailEffects = new FailEffectParams[0];

        public void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            bool canApplyEffect = true;
            float powerVariation = power * randomModifier;
            if (HasEffects())
            {
                CombatConditionArguments conditionArguments
                    = new CombatConditionArguments(user, target, effectPreset);

                canApplyEffect = UtilsConditions.CanApplyConditions(effectConditions, conditionArguments);
            }

            if (canApplyEffect)
            {
                effectPreset.DoEffect(user, target, powerVariation);
            }
            else
            {
                var failEffects = onFailEffects;
                foreach (var failEffect in failEffects)
                {
                    failEffect.DoEffect(user, target, randomModifier);
                }
            }
        }

        public void OnValidateEffects()
        {
            if(HasEffects()) return;
            onFailEffects = new FailEffectParams[0];
        }
    }

    [Serializable]
    public class FailEffectParams : EffectParamsBase
    {

        public void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            var targets = UtilsTargets.GetEffectTargets(user, target, effectTarget);
            foreach (CombatingEntity failTarget in targets)
            {
                effectPreset.DoEffect(user,failTarget,randomModifier);
            }
        }

    }

    public interface IEffect
    {
        void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier);
        SEffectBase.EffectTarget GetEffectTarget();
        bool CanPerformRandom();
    }
}
