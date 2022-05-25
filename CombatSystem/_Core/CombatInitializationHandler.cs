
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem._Core
{
    public static class CombatInitializationHandler
    {


        public static void StartCombat(
            ICombatTeamProvider playerTeam,
            ICombatTeamProvider enemyTeam)
        {
            // This is just in case these weren't instantiated
            var playerSingleton = PlayerCombatSingleton.Instance;
            var enemySingleton = EnemyCombatSingleton.Instance;
            AssetPrefabInstantiationHandler.FirstCombatObjectsInstantiation();

            PrepareTeams(
                playerTeam,
                enemyTeam);

            PrepareTeamControllers();
            //InitialStatsPreparations();
        }



        private static void PrepareTeams(
            ICombatTeamProvider playerTeam,
            ICombatTeamProvider enemyTeam)
        {
            var systemEvents = CombatSystemSingleton.EventsHolder;

            var playerCombatTeam = new CombatTeam(true,playerTeam);
            var enemyCombatTeam = new CombatTeam(false,enemyTeam);
            var allMembers = new CombatMembersHolder(playerCombatTeam, enemyCombatTeam);

            playerCombatTeam.Injection(enemyCombatTeam);
            enemyCombatTeam.Injection(playerCombatTeam);

            var teamsHolder = CombatSystemSingleton.TeamsHolder;
            teamsHolder.PlayerTeamType = playerCombatTeam;
            teamsHolder.EnemyTeamType = enemyCombatTeam;


            CombatSystemSingleton.AllMembersCollection = allMembers;

            InstantiateModels();

            // INVOCATION
            var combatStatesHandler = CombatSystemSingleton.CombatPreparationStatesHandler;
            combatStatesHandler.OnCombatPrepares(allMembers, playerCombatTeam, enemyCombatTeam);




            void InstantiateModels()
            {
                var combatEntitiesPrefabPool = CombatSystemSingleton.EntityPrefabsPoolHandler;
                combatEntitiesPrefabPool.HandleTeams(playerCombatTeam, enemyCombatTeam);
            }

           
        }

        private static void PrepareTeamControllers()
        {
            var teamControllersHolder = CombatSystemSingleton.TeamControllers;
            var playerController = PlayerCombatSingleton.PlayerTeamController;
            var enemyController = EnemyCombatSingleton.TeamController;

            teamControllersHolder.PlayerTeamType = playerController;
            teamControllersHolder.EnemyTeamType = enemyController;
        }


        public static void InitialStatsPreparations()
        {
            var playerTeam = CombatSystemSingleton.PlayerTeam;
            var enemyTeam = CombatSystemSingleton.OppositionTeam;

            InitialStatsPreparation(in playerTeam);
            InitialStatsPreparation(in enemyTeam);

            void InitialStatsPreparation(in CombatTeam team)
            {
                foreach (var member in team)
                {
                    member.Stats.SetToFullInitiative();
                }
            }
        }


        private sealed class CombatMembersHolder : IReadOnlyList<CombatEntity>
        {
            public CombatMembersHolder(CombatTeam playerTeam, CombatTeam enemyTeam)
            {
                _playerTeam = playerTeam;
                _enemyTeam = enemyTeam;
            }

            private readonly CombatTeam _playerTeam;
            private readonly CombatTeam _enemyTeam;

            public IEnumerator<CombatEntity> GetEnumerator()
            {
                foreach (var member in _playerTeam)
                {
                    yield return member;
                }

                foreach (var member in _enemyTeam)
                {
                    yield return member;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Count => _playerTeam.Count + _enemyTeam.Count;

            public CombatEntity this[int index]
            {
                get
                {
                    int playerCount = _playerTeam.Count;
                    return index < playerCount 
                        ? _playerTeam[index] 
                        : _enemyTeam[index - playerCount];
                }
            }
        }
    }

    public sealed class CombatInitializationEditorWindow : OdinEditorWindow
    {
        [Title("Player")]
        public SPlayerPresetTeam playerTesterTeam;
        [Title("Adversary")]
        public SPredefinedTeam oppositionTeam;


        [MenuItem("Combat/COMBAT Initialization [Helper]", priority = -20)]
        private static void OpenWindow()
        {
            GetWindow<CombatInitializationEditorWindow>().Show();
        }

        private bool InvalidSetup() => !playerTesterTeam || !oppositionTeam;
        private bool CanFinish() => CombatSystemSingleton.GetIsCombatActive();


        [Button(ButtonSizes.Large), GUIColor(.3f,.7f,.8f), DisableIf("InvalidSetup"), DisableInEditorMode,
        InfoBox("Invalid setup (some teams are <b>NULL</b>)",InfoMessageType.Error, "InvalidSetup")]
        private void TestCombat()
        {
            TestStartCombat(playerTesterTeam, oppositionTeam);
        }

        [Button, DisableIf("CanFinish"), DisableInEditorMode]
        private void FinishCombat(bool isWin = true)
        {
            CombatFinishHandler.FinishCombat(isWin);
        }

        private static void TestStartCombat(SPlayerPresetTeam playerTeam, SPredefinedTeam enemyTeam)
        {
            PlayerCombatSingleton.SelectedCharactersHolder.AddTeam(playerTeam);
            TestStartCombat(enemyTeam);
        }
        private static void TestStartCombat(SPredefinedTeam enemyTeam)
        {
            var playerCharacters = PlayerCombatSingleton.SelectedCharactersHolder;
            if (!playerCharacters.IsValid())
            {
                Debug.LogError("Player characters weren't selected");
                return;
            }

            CombatInitializationHandler.StartCombat(playerCharacters, enemyTeam);
        }
    }
}
