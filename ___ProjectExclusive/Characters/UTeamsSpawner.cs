using System;
using System.Collections.Generic;
using Characters;
using _CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class UTeamsSpawner : MonoBehaviour, ICharacterFaction<ICharacterArchetypes<Transform>>,
        ICombatPreparationListener, ICombatFinishListener
    {
        [TitleGroup("Spawn Points")]
        [SerializeField] private TeamTransforms playerFaction = 
            new TeamTransforms();
        [SerializeField] private TeamTransforms enemyFaction =
            new TeamTransforms();
        [Title("Back Up")]
        [SerializeField] private GameObject onNullSpawnPrefab;

        public ICharacterArchetypes<Transform> PlayerFaction => playerFaction;
        public ICharacterArchetypes<Transform> EnemyFaction => enemyFaction;


        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }

        [TitleGroup("Params")]
        public bool spawnEntities = true;
        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            EntityHolderSpawner spawner = CharacterSystemSingleton.Spawner;
            AddEntities(playerEntities,playerFaction);
            AddEntities(enemyEntities,enemyFaction);
            
            void AddEntities(CombatingTeam team, TeamTransforms transforms)
            {
#if UNITY_EDITOR
                if (team.Count != CharacterArchetypes.AmountOfArchetypes)
                {
                    throw new NotImplementedException("Can't spawn all entities",
                        new IndexOutOfRangeException($"Not enough elements: {team.Count}"));
                } 
#endif

                InvokeEntity(team.FrontLiner,transforms.FrontLiner);
                InvokeEntity(team.MidLiner, transforms.MidLiner);
                InvokeEntity(team.BackLiner, transforms.BackLiner);
            }
            void InvokeEntity(CombatingEntity entity, Transform spawnTransform)
            {
                if(!spawnEntities) return;
                GameObject prefab = entity.InstantiationPrefab;
                if(prefab == null)
                {
#if UNITY_EDITOR
                    Debug.LogWarning("Invoking NULL prefab"); 
#endif
                    prefab = onNullSpawnPrefab;
                }
                
                UCharacterHolder holder = spawner.SpawnEntity(prefab);
                holder.Injection(entity);
                Transform holderTransform = holder.transform;
                holderTransform.position = spawnTransform.position;
                holderTransform.rotation = spawnTransform.rotation;
            }
        }


        public void OnCombatFinish(CombatingTeam removeEnemies)
        {
            EntityHolderSpawner spawner = CharacterSystemSingleton.Spawner;
            foreach (CombatingEntity enemy in removeEnemies)
            {
                spawner.DeSpawn(enemy);
            }
        }
    }


    [Serializable]
    internal class TeamTransforms : SerializableCharacterArchetypes<Transform>
    { }

}
