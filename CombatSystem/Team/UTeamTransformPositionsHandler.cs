using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public class UTeamTransformPositionsHandler : UTeamFullGroupStructure<Transform>
    {
        [SerializeField] private bool isNullTypeBackUp;
        [SerializeField] private bool isPlayer;

        private void Start()
        {
            HandleSingleton();
        }

        private void HandleSingleton()
        {
            if (isNullTypeBackUp)
            {
                var prefabPool = CombatSystemSingleton.EntityPrefabsPoolHandler;

                if (isPlayer)
                    prefabPool.PlayerOnNullPositionReference = this;
                else
                    prefabPool.EnemyOnNullPositionReference = this;

                gameObject.SetActive(false);
                return;
            }

            HandleExistingReference();
            DoInjection();


            void HandleExistingReference()
            {
                var singletonReference = (isPlayer) 
                ? CombatSystemSingleton.PlayerPositionTransformReferences
                : CombatSystemSingleton.EnemyPositionTransformReferences;
                if (singletonReference)
                    Destroy(singletonReference);
            }

            void DoInjection()
            {
                if (isPlayer)
                    CombatSystemSingleton.PlayerPositionTransformReferences = this;
                else
                    CombatSystemSingleton.EnemyPositionTransformReferences = this;
            }
        }
        
    }
}
