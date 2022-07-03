using System;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Skills.Effects;
using CombatSystem.Stats;
using UnityEngine;
using Utils;

namespace CombatSystem.VFX
{
    [Serializable]
    public sealed class ParticlesSpawnHandler : IEffectUsageListener
    {
        [SerializeField] private SStatsPrefabsHolder particlesHolder;

        public void OnCombatPrimaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            OnCombatSecondaryEffectPerform(performer,target,in values); //todo make it by animator
        }

        public void OnCombatSecondaryEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            var effect = values.Effect;
            var effectPrefab = effect.GetSecondaryParticlesPrefab();
            if (!effectPrefab)
            {
                var secondaryParticlesHolder = particlesHolder.GetReferences();
                var effectType = effect.EffectType;
                effectPrefab = UtilsStructureEffect.GetElement(effectType, secondaryParticlesHolder);
            }

            if(!effectPrefab) return;

            SpawnParticles(target,effectPrefab);
        }

        public void OnCombatVanguardEffectPerform(CombatEntity performer, CombatEntity target, in PerformEffectValues values)
        {
            OnCombatSecondaryEffectPerform(performer,target, in values);
        }

        private const float LocalPositionCenterOffset = 0.5f;
        private const float LocalScaleOffset = .2f; 
        private void SpawnParticles(CombatEntity target, GameObject particles)
        {
            var positions = target.Body.GetPositions();
            var targetPosition = positions.PivotRootType;
            var particlesObject = UtilsInstantiation.InstantiationTransformRandomness(
                particles, targetPosition,
                LocalPositionCenterOffset, LocalScaleOffset);

            CombatParticleDeSpawnHandler.HandleDeSpawnParticles(particlesObject);
        }
    }
}
