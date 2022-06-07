using System;
using CombatSystem.Entity;
using CombatSystem.Skills;
using CombatSystem.Stats;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

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
                effectPrefab = UtilsStats.GetElement(effectType, secondaryParticlesHolder);
            }

            if(!effectPrefab) return;

            SpawnParticles(target,effectPrefab);
        }

        private void SpawnParticles(CombatEntity target, GameObject particles)
        {
            var targetTransform = target.Body.GetUIHoverHolder();
            var particlesObject = Object.Instantiate(particles, targetTransform.position, Quaternion.identity);

            CombatParticleDeSpawnHandler.HandleDeSpawnParticles(particlesObject);
        }
    }
}
