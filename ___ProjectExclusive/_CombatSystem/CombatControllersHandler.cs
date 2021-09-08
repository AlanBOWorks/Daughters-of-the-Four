using System.Collections.Generic;
using _Enemies;
using _Player;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CombatSystem
{
    public class CombatControllersHandler
    {

        [ShowInInspector, DisableInEditorMode]
        public ITempoTriggerHandler PlayerTempoHandler { get; private set; }
        [ShowInInspector, DisableInEditorMode]
        public ICombatEnemyController EnemyController { get; private set; }

        public void InjectPlayerEvents(ITempoTriggerHandler playerTriggerHandler)
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

        public void SubscribePlayerEvent(IPlayerTempoListener listener)
        {
            PlayerTempoHandler.TempoListeners.Add(listener);
        }

        public void SubscribePlayerEvent(IPlayerRoundListener listener)
        {
            PlayerTempoHandler.RoundListeners.Add(listener);
        }


        public void CallForControl(CombatingEntity entity)
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
