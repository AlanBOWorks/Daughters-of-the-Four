using System;
using System.Collections.Generic;
using _CombatSystem;
using _Team;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Characters
{
    public class UCombatTeamsSpawner : MonoBehaviour, ICharacterFaction<ICharacterArchetypesData<Transform>>,
        ICombatPreparationListener, ICombatFinishListener
    {
        [TitleGroup("Spawn Points")]
        [SerializeField] private TeamTransforms playerFaction = 
            new TeamTransforms();
        [SerializeField] private TeamTransforms enemyFaction =
            new TeamTransforms();
        [Title("Back Up")]
        [SerializeField] private GameObject onNullSpawnPrefab;

        public ICharacterArchetypesData<Transform> PlayerFaction => playerFaction;
        public ICharacterArchetypesData<Transform> EnemyFaction => enemyFaction;

        private CombatingTeam _playerTeam;
        private CombatingTeam _enemyTeam;

        private void Awake()
        {
            CombatSystemSingleton.Invoker.SubscribeListener(this);
            CharacterSystemSingleton.CombatSpawner = this;
        }

        [TitleGroup("Params")]
        public bool spawnEntities = true;
        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            EntityHolderSpawner spawner = CharacterSystemSingleton.CharactersSpawner;
            _playerTeam = playerEntities;
            _enemyTeam = enemyEntities;

            AddEntities(playerEntities,playerFaction);
            AddEntities(enemyEntities,enemyFaction);
            
            void AddEntities(CombatingTeam team, TeamTransforms transforms)
            {
#if UNITY_EDITOR
                if (team.Count != UtilsCharacterArchetypes.AmountOfArchetypesAmount)
                {
                    throw new NotImplementedException("Can't spawn all entities",
                        new IndexOutOfRangeException($"Not enough elements: {team.Count}"));
                } 
#endif
                UtilsCharacterArchetypes.DoParse(team,transforms,InvokeEntity);                
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



        public void OnCombatFinish(CombatingEntity lastEntity, bool isPlayerWin)
        {
            //TODO provisional with DeSpawn >> Change to animations instead
            EntityHolderSpawner spawner = CharacterSystemSingleton.CharactersSpawner;

            RemoveEntities(_enemyTeam);
            RemoveEntities(_playerTeam);


            void RemoveEntities(CombatingTeam team)
            {
                UtilsCharacterArchetypes.DoAction(team,RemoveEntity);
            }

            void RemoveEntity(CombatingEntity entity)
            {
                spawner.DeSpawn(entity);
            }

        }
    }


    [Serializable]
    internal class TeamTransforms : MonoCharacterArchetypes<Transform>
    { }

}
