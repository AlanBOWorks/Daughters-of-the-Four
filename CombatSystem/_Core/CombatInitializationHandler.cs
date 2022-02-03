
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


            var combatStatesHandler = CombatSystemSingleton.CombatStatesHandler;
            combatStatesHandler.OnCombatPrepares(allMembers, playerCombatTeam, enemyCombatTeam);
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
