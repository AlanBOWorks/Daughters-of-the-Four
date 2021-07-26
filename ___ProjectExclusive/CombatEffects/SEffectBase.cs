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
        [Range(-10, 10), SuffixLabel("%00")]
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
        [SerializeField, ShowIf("HasEffects")]
        private ConditionParam effectCondition;

        [SerializeField, ShowIf("HasCondition")]
        private FailEffectParams[] onFailEffects = new FailEffectParams[0];

        protected bool HasEffects() => effectPreset != null;
        protected bool HasCondition() => effectCondition.HasCondition();

        public void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier)
        {
            bool canApplyEffect = true;
            float powerVariation = power * randomModifier;
            float originalPowerVariation = powerVariation;
            EffectArguments effectArguments
                = new EffectArguments(user, target, effectPreset);
            if (effectCondition.HasCondition())
            {
                canApplyEffect = effectCondition.CanApplyCondition(ref effectArguments);
            }

            if (canApplyEffect)
            {
                DoPassiveVariation(user.PassivesHolder.ActionFilterPassives);
                DoPassiveVariation(target.PassivesHolder.ReactionFilterPassives);
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

            void DoPassiveVariation(IEnumerable<SPassiveFilterPreset> passives)
            {
                foreach (SPassiveFilterPreset passive in passives)
                {
                    passive.DoPassiveFilter(
                        ref effectArguments, 
                        ref powerVariation, 
                        originalPowerVariation);
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

    public interface IEffect : IEffectBase
    {
        SEffectBase.EffectTarget GetEffectTarget();
        bool CanPerformRandom();
    }

    public interface IEffectBase
    { 
        void DoEffect(CombatingEntity user, CombatingEntity target, float randomModifier);
    }


    public struct EffectArguments
    {
        public readonly CombatingEntity User;
        public readonly CombatingEntity Target;
        public readonly SEffectBase Effect;

        public EffectArguments(CombatingEntity user, CombatingEntity target, SEffectBase effect)
        {
            User = user;
            Target = target;
            Effect = effect;
        }
    }

}
