using CombatSystem.Entity;
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


        public abstract void DoEffect(EntityPairInteraction entities, float effectValue);
        public abstract string EffectTag { get; }
        public abstract string EffectSmallPrefix { get; }
        public abstract EnumsEffect.ConcreteType EffectType { get; }

        public const string EffectPrefix = "Effect";

    }
}
