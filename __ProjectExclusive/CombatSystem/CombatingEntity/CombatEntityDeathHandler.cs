using System.Collections.Generic;
using CombatSystem;
using UnityEngine;

namespace CombatEntity
{
    public sealed class CombatEntityDeathHandler
    {
        public CombatEntityDeathHandler()
        {
            _deathQueue = new List<CombatingEntity>();
            _deathPool = new List<CombatingEntity>();
        }

        private readonly List<CombatingEntity> _deathQueue;
        private readonly List<CombatingEntity> _deathPool;

        public void EnQueueEntity(CombatingEntity entity)
        {
            if(_deathQueue.Contains(entity)) return; // Multi hits that goes below zero could add more than one

            _deathQueue.Add(entity);
        }

        public void Revive(CombatingEntity entity)
        {
            if (_deathQueue.Contains(entity))
            {
                _deathQueue.Remove(entity);
                return;
            }

            _deathPool.Remove(entity);
            // TODO call revive for team and system
        }

        // Problem: effects needs the entity being alive to do their effects before the skill's behaviour ends
        // Solution: enqueue the dead entity in a queue and calls it after the skill ends performing
        // Possibles problems: could have incompatibilities with Revive and/or event calls related to Health
        public void HandleDeaths()
        {
            for (var i = _deathQueue.Count - 1; i >= 0; i--)
            {
                var entity = _deathQueue[i];
                _deathPool.Add(entity);
                CombatSystemSingleton.EventsHolder.OnMemberDeath(entity.Team,entity);
            }
        }
    }
}
