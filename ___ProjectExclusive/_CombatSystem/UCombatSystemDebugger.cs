using System;
using _Team;
using Characters;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace _CombatSystem
{
    public class UCombatSystemDebugger : MonoBehaviour, ICombatAfterPreparationListener
    {
#if UNITY_EDITOR
        [ShowInInspector,DisableInEditorMode,TabGroup("Combat")]
        private CombatSystemSingleton _system = CombatSystemSingleton.Instance;
        [ShowInInspector, DisableInEditorMode, TabGroup("Prefabs")]
        private CharacterSystemSingleton _characters = CharacterSystemSingleton.Instance;

        [ShowInInspector, DisableInEditorMode]
        private TeamDataTracker _teamTracker;
        [ShowInInspector, DisableInEditorMode]
        private TeamDataTracker _enemyTeamTracker;

        private void Awake()
        {
            _teamTracker = new TeamDataTracker();
            _enemyTeamTracker = new TeamDataTracker();
            CombatSystemSingleton.Invoker.SubscribeListener(this);
        }

        private class TeamDataTracker
        {
            public TeamCombatControlHandler controlHandler;
            public TeamCombatStatsHolder Stats;
        }


        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
            _teamTracker.controlHandler = playerEntities.ControlHandler;
            _teamTracker.Stats = playerEntities.StatsHolder;

            _enemyTeamTracker.controlHandler = enemyEntities.ControlHandler;
            _enemyTeamTracker.Stats = enemyEntities.StatsHolder;
        }
#else
        private void Awake()
        {
            Destroy(this);
        }

        public void OnAfterPreparation(CombatingTeam playerEntities, CombatingTeam enemyEntities, CharacterArchetypesList<CombatingEntity> allEntities)
        {
        }
#endif

    }
}
