using System;
using CombatSystem._Core;
using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Team
{
    public class UTeamTransformPositionsHandler : UTeamFullGroupStructure<Transform>
    {
        [SerializeField] private bool isPlayer;

        private void Start()
        {
            HandleSingleton();
        }

        private void HandleSingleton()
        {
            HandleExistingReference();
            DoInjection();


            void HandleElement(ref UTeamFullGroupStructure<Transform> element)
            {
                var forDestruction = element;
                element = this;

                if(forDestruction) Destroy(forDestruction);
            }

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
