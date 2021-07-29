using System;
using System.Collections.Generic;
using _Team;
using Characters;
using UnityEngine;

namespace _CombatSystem
{
    public class RoundCheckHandler : List<CombatingEntity>, ICombatPreparationListener, ITempoListener, ISkippedTempoListener
    {
        private CharacterArchetypesList<CombatingEntity> _allEntities;
      

        public void OnBeforeStart(CombatingTeam playerEntities, CombatingTeam enemyEntities,
            CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _allEntities = allEntities;
            RefreshList();
        }

        private void RefreshList()
        {
            Clear();
            AddRange(_allEntities);
        }

        public void OnInitiativeTrigger(CombatingEntity entity)
        {}

        public void OnDoMoreActions(CombatingEntity entity)
        {}

        public void OnFinisAllActions(CombatingEntity entity)
        {
            DoCheckRoundFinish(entity);
        }

        private void DoCheckRoundFinish(CombatingEntity entity)
        {
            if (Count > 1) //1 means is the last entity
            {
                Remove(entity);
            }
            else
            {
                var lastEntity = this[0];
                if(lastEntity != entity) return;

                var tempoHandler = CombatSystemSingleton.TempoHandler;
                tempoHandler.OnRoundCompleted(_allEntities,lastEntity);
                RefreshList();

#if UNITY_EDITOR
                Debug.Log($"Last entity: {entity.CharacterName}");
#endif
            }

        }

        public void OnSkippedEntity(CombatingEntity entity)
        {
            DoCheckRoundFinish(entity);
        }
    }
}
