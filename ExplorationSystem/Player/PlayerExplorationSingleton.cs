using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Common;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class PlayerExplorationSingleton 
    {
        public static readonly PlayerExplorationSingleton Instance;
        public static ITeamFlexStructureRead<PlayerRunTimeEntity> GetCurrentSelectedTeam() => Instance._playerTeam;
        public static ICombatTeamProvider GetPlayerTeamProvider() => Instance._playerTeam;

        static PlayerExplorationSingleton()
        { 
            Instance = new PlayerExplorationSingleton();

            var themeAsset =
                AssetDatabase.LoadAssetAtPath<SExplorationThemeHolder>(SExplorationThemeHolder.AssetPath);
            ExplorationThemeHolder = themeAsset.GetDataHolder();

        }

        public void InjectTeam(ITeamFlexStructureRead<ICombatEntityProviderHolder> team)
        {
            _playerTeam = new PlayerRunTimeTeam(team);
        }




        [Title("Entities")]
        [ShowInInspector, DisableInEditorMode]
        private PlayerRunTimeTeam _playerTeam;

        [Title("Theme")]
        [ShowInInspector, InlineEditor()]
        public static readonly IExplorationTypesStructureRead<IThemeHolder> ExplorationThemeHolder;



        private sealed class PlayerExplorationSingletonWindow : OdinEditorWindow
        {
            [ShowInInspector]
            private PlayerExplorationSingleton _explorationSingleton;


            [MenuItem("Game/Debug/Player Exploration [Singleton]", priority = -100)]
            private static void ShowWindow()
            {
                var window = GetWindow<PlayerExplorationSingletonWindow>();
                window._explorationSingleton = Instance;
            }
        }

       
    }

}
