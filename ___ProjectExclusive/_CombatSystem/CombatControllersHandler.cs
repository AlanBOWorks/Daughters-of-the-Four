using System.Collections.Generic;
using _Enemies;
using _Player;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatControllersHandler : ITempoListener
    {

        [ShowInInspector, DisableInEditorMode]
        public PlayerCombatEvents PlayerTempoHandler { get; private set; }
        [ShowInInspector, DisableInEditorMode]
        public ICombatEnemyController EnemyController { get; private set; }

        public void InjectPlayerEvents(PlayerCombatEvents playerTriggerHandler)
        {
            PlayerTempoHandler = playerTriggerHandler;
        }

        public void Inject(ICombatEnemyController enemyController)
        {
            if (enemyController == null)
                EnemyController = CombatEnemyControllerRandom.GenericEnemyController;
            else
                EnemyController = enemyController;
        }


        private void CallForControl(CombatingEntity entity)
        {
            if (IsForPlayer(entity))
            {
                PlayerTempoHandler.OnDoMoreActions(entity);
            }
            else
            {
                EnemyController.DoControlOn(entity);
            }
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {
            if (IsForPlayer(entity)) 
                PlayerTempoHandler.OnInitiativeTrigger(entity);
        }

        public void OnDoMoreActions(CombatingEntity entity)
        {
            CallForControl(entity);
        }

        public void OnFinisAllActions(CombatingEntity entity)
        {
            if (IsForPlayer(entity))
                PlayerTempoHandler.OnFinisAllActions(entity);
        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            if (IsForPlayer(entity))
                PlayerTempoHandler.OnSkippedEntity(entity);
        }

        public void OnRoundCompleted(List<CombatingEntity> allEntities, CombatingEntity lastEntity)
        {
            PlayerTempoHandler.OnRoundCompleted(allEntities, lastEntity);
        }

        private static bool IsForPlayer(CombatingEntity entity)
        {
            return UtilsCharacter.IsAPlayerEntity(entity);
        }

    }
}
