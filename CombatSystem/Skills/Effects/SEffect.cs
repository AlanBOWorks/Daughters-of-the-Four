using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public abstract class SEffect : ScriptableObject, IEffect
    {
        [SerializeField, AssetsOnly] private Sprite icon;
        [SerializeField,AssetsOnly] private GameObject secondaryParticlePrefab;

        public Sprite GetIcon() => icon;
        public GameObject GetSecondaryParticlesPrefab() => secondaryParticlePrefab;


        public abstract void DoEffect(EntityPairInteraction entities, ref float effectValue, ref float luckModifier);
        public abstract float CalculateEffectValue(CombatStats performerStats, float effectValue);
        public virtual string GetEffectValueTootLip(CombatStats performerStats, ref float effectValue)
        {
            effectValue = CalculateEffectValue(performerStats, effectValue);

            var effectTooltip = LocalizeEffects.LocalizeMathfValue(effectValue, IsPercentSuffix());
            return " <b>" + effectTooltip + "</b>";
        }
        public abstract bool IsPercentSuffix();
        public virtual bool IsPercentTooltip() => IsPercentSuffix();


        public abstract string EffectTag { get; }
        public abstract string EffectSmallPrefix { get; }
        public abstract EnumsEffect.ConcreteType EffectType { get; }

        public const string EffectPrefix = "Effect";

    }
}
