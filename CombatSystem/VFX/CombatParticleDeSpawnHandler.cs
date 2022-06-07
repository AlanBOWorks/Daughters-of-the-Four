using System.Collections.Generic;
using MEC;
using UnityEngine;

namespace CombatSystem.VFX
{
    public static class CombatParticleDeSpawnHandler
    {
        private const float OnNullDeSpawnTime = 5f;
        private const float ParticlesOffsetDuration = .2f;
        public static void HandleDeSpawnParticles(GameObject holder)
        {
            var particlesSystem = holder.GetComponent<ParticleSystem>();
            if (particlesSystem)
            {
                HandleParticlesSystemDeSpawn(holder,particlesSystem);
                return;
            }

            Timing.RunCoroutine(_DeSpawnParticles(holder, OnNullDeSpawnTime));
        }

        private static void HandleParticlesSystemDeSpawn(GameObject holder, ParticleSystem particlesSystem)
        {
            float duration = particlesSystem.main.duration + ParticlesOffsetDuration;
            Timing.RunCoroutine(_DeSpawnParticles(holder, duration));
        }

        private static IEnumerator<float> _DeSpawnParticles(GameObject holder, float afterSeconds)
        {
            yield return Timing.WaitForSeconds(afterSeconds);
            UnityEngine.Object.Destroy(holder);
        }
    }

}
