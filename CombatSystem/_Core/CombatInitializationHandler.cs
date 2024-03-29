
using System;
using System.Collections;
using System.Collections.Generic;
using CombatSystem.AI;
using CombatSystem.Entity;
using CombatSystem.Player;
using CombatSystem.Team;
using MEC;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils_Project;

namespace CombatSystem._Core
{
    public static class CombatInitializationHandler
    {

        private const string OnNullPlayerTeamAsset = AssetPaths.ScriptablesPlayerTeams + 
                                                "OnNull[Player Predefined Team].asset";


        public static void StartCombat(
            ICombatTeamProvider playerTeam,
            ICombatTeamProvider enemyTeam)
        {
            if(CombatSystemSingleton.GetIsCombatActive())
                throw new AccessViolationException("Trying to access combat while mid Combat");

            if (playerTeam == null || playerTeam.MembersCount < 1)
            {
                playerTeam = AssetDatabase.LoadAssetAtPath<SPlayerPresetTeam>(OnNullPlayerTeamAsset);
            }

            Timing.RunCoroutine(_InstantiationCoroutine());
            //Made into a coroutine so instantiation racing is less troublesome
            IEnumerator<float> _InstantiationCoroutine() 
            {
                // This is just in case these weren't instantiated
                var playerSingleton = PlayerCombatSingleton.Instance;
                var enemySingleton = EnemyCombatSingleton.Instance;
                AssetPrefabInstantiationHandler.FirstCombatObjectsInstantiation();
                yield return Timing.WaitForOneFrame;

                PrepareTeams(
                    playerTeam,
                    enemyTeam);
                yield return Timing.WaitForOneFrame;

                PrepareTeamControllers();
                //InitialStatsPreparations();
            }

        }



        private static void PrepareTeams(
            ICombatTeamProvider playerTeam,
            ICombatTeamProvider enemyTeam)
        {
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


        private sealed class CombatMembersHolder : IReadOnlyCollection<CombatEntity>
        {
            public CombatMembersHolder(CombatTeam playerTeam, CombatTeam enemyTeam)
            {
                _playerTeam = playerTeam.GetAllMembers();
                _enemyTeam = enemyTeam.GetAllMembers();
            }

            private readonly IReadOnlyCollection<CombatEntity> _playerTeam;
            private readonly IReadOnlyCollection<CombatEntity> _enemyTeam;

            public IEnumerator<CombatEntity> GetEnumerator()
            {
                var playerMembers = _playerTeam;
                foreach (var member in playerMembers)
                {
                    yield return member;
                }

                var enemyMembers = _enemyTeam;
                foreach (var member in enemyMembers)
                {
                    yield return member;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public int Count => _playerTeam.Count + _enemyTeam.Count;

        }
    }

    public sealed class CombatInitializationEditorWindow : OdinEditorWindow
    {
        [Title("Player")]
        public SPlayerPresetTeam playerTesterTeam;
        [Title("Adversary")]
        public SEnemyPredefinedTeam oppositionTeam;


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

        [Button, EnableIf("CanFinish"), DisableInEditorMode]
        private void FinishCombat(UtilsCombatFinish.FinishType type = UtilsCombatFinish.FinishType.PlayerWon)
        {
            UtilsCombatFinish.FinishCombat(type);
        }

        private static void TestStartCombat(SPlayerPresetTeam playerTeam, SEnemyPredefinedTeam enemyTeam)
        {
            PlayerCombatSingleton.SelectedCharactersHolder.AddPredefinedTeam(playerTeam);
            TestStartCombat(enemyTeam);
        }
        private static void TestStartCombat(SEnemyPredefinedTeam enemyTeam)
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
