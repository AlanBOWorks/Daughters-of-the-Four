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
                    return;
                }
                
                UCharacterHolder holder = spawner.SpawnEntity(prefab);
                entity.Holder = holder;
                holder.transform.position = spawnTransform.position;
            }
        }


        public void OnFinish(CombatingTeam removeEnemies)
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
