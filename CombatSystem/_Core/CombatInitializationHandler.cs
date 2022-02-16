
using System.Collections.Generic;
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
            PrepareTeams(
                playerTeam.GetSelectedCharacters(),
                enemyTeam.GetSelectedCharacters());

            PrepareTeamControllers();
        }



        private static void PrepareTeams(
            IReadOnlyCollection<ICombatEntityProvider> playerTeam,
            IReadOnlyCollection<ICombatEntityProvider> enemyTeam)
        {
            var playerCombatTeam = new CombatTeam(playerTeam);
            var enemyCombatTeam = new CombatTeam(enemyTeam);
            var allMembers = new CombatMembersHolder(playerCombatTeam, enemyCombatTeam);

            playerCombatTeam.Injection(enemyCombatTeam);
            enemyCombatTeam.Injection(playerCombatTeam);

            CombatSystemSingleton.PlayerTeam = playerCombatTeam;
            CombatSystemSingleton.OppositionTeam = enemyCombatTeam;


            CombatSystemSingleton.AllMembersCollection = allMembers;


            var combatStatesHandler = CombatSystemSingleton.CombatPreparationStatesHandler;
            combatStatesHandler.OnCombatPrepares(allMembers, playerCombatTeam, enemyCombatTeam);
            InstantiateModels();


            void InstantiateModels()
            {
                var teamPositionHandler = CombatSystemSingleton.PositionHandler;
                HandlePositions(playerCombatTeam, teamPositionHandler.PlayerTeamType);
                HandlePositions(enemyCombatTeam, teamPositionHandler.EnemyTeamType);

                void HandlePositions(CombatTeam team, ITeamPositionHandler handler)
                {
                    foreach (CombatEntity member in team)
                    {
                        var provider = member.Provider;
                        handler.ProvideInstantiationPoint(in provider, out var position, out var rotation);

                        GameObject instantiatedGameObject;
                        GameObject copyReference = provider.GetVisualPrefab();
                        if (copyReference == null)
                            instantiatedGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                        else
                            instantiatedGameObject = Object.Instantiate(copyReference);

                        var instantiatedTransform = instantiatedGameObject.transform;
                        instantiatedTransform.position = position;
                        instantiatedTransform.rotation = rotation;
                        instantiatedGameObject.name = provider.GetEntityName() + "(Clone)";

                        member.InstantiationReference = instantiatedGameObject;
                    }
                }
            }
        }

        private static void PrepareTeamControllers()
        {
            var teamControllersHolder = CombatSystemSingleton.TeamControllers;
            var playerController = PlayerCombatSingleton.PlayerTeamController;

            teamControllersHolder.PlayerTeamType = playerController;
        }



        private sealed class CombatMembersHolder : List<CombatEntity>
        {
            public CombatMembersHolder(CombatTeam playerTeam, CombatTeam enemyTeam)
            {
                AddMembers(in playerTeam);
                AddMembers(in enemyTeam);

                void AddMembers(in CombatTeam team)
                {
                    foreach (var member in team)
                    {
                        Add(member);
                    }
                }
            }

        }
    }

    public sealed class CombatInitializationEditorWindow : OdinEditorWindow
    {
        [Title("Player"),HorizontalGroup()]
        public SPlayerPresetTeam playerTesterTeam;
        [Title("Adversary"), HorizontalGroup()]
        public SPredefinedTeam oppositionTeam;


        [MenuItem("Combat/COMBAT Initialization [Helper]", priority = -20)]
        private static void OpenWindow()
        {
            GetWindow<CombatInitializationEditorWindow>().Show();
        }

        private bool InvalidSetup() => !playerTesterTeam || !oppositionTeam;

        [Button(ButtonSizes.Large), GUIColor(.3f,.7f,.8f), DisableIf("InvalidSetup"), DisableInEditorMode,
        InfoBox("Invalid setup (some teams are <b>NULL</b>)",InfoMessageType.Error, "InvalidSetup")]
        private void TestCombat()
        {
            TestStartCombat(playerTesterTeam, oppositionTeam);
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
