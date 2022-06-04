using CombatSystem.Entity;
using CombatSystem.Stats;
using Sirenix.OdinInspector;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    public abstract class SEffect : ScriptableObject, IEffect
    {
        [SerializeField,AssetsOnly] private GameObject secondaryParticlePrefab;
        public GameObject GetSecondaryParticlesPrefab() => secondaryParticlePrefab;


        public abstract void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue);
        public abstract string EffectTag { get; }
        public abstract string EffectSmallPrefix { get; }
        public abstract EnumStats.StatType EffectType { get; }

        public const string EffectPrefix = "Effect";

    }
}
