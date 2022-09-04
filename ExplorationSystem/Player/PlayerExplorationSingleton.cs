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
    public sealed class PlayerExplorationSingleton : ITeamFlexStructureRead<PlayerRunTimeEntity>,
        ICombatTeamProvider
    {
        public static readonly PlayerExplorationSingleton Instance;
        public static ITeamFlexStructureRead<PlayerRunTimeEntity> GetCurrentSelectedTeam() => Instance;
        public static ICombatTeamProvider GetPlayerTeamProvider() => Instance;

        static PlayerExplorationSingleton()
        { 
            Instance = new PlayerExplorationSingleton();

            var themeAsset =
                AssetDatabase.LoadAssetAtPath<SExplorationThemeHolder>(SExplorationThemeHolder.AssetPath);
            ExplorationThemeHolder = themeAsset.GetDataHolder();

        }

        public void InjectTeam(ITeamFlexStructureRead<ICombatEntityProviderHolder> team)
        {
            VanguardType = HandleInstantiation(team.VanguardType.GetEntityProvider());
            AttackerType = HandleInstantiation(team.AttackerType.GetEntityProvider());
            SupportType = HandleInstantiation(team.SupportType.GetEntityProvider());
            FlexType = HandleInstantiation(team.FlexType.GetEntityProvider());
        }

        


        [Title("Player Entities")]
        [ShowInInspector, ShowIf("VanguardType"), HorizontalGroup("FrontLine")]
        public PlayerRunTimeEntity VanguardType { get; private set; }
        [ShowInInspector, ShowIf("AttackerType"), HorizontalGroup("FrontLine")]
        public PlayerRunTimeEntity AttackerType { get; private set; }
        [ShowInInspector, ShowIf("SupportType"), HorizontalGroup("BackLine")]
        public PlayerRunTimeEntity SupportType { get; private set; }
        [ShowInInspector, ShowIf("FlexType"), HorizontalGroup("BackLine")]
        public PlayerRunTimeEntity FlexType { get; private set; }
        public IEnumerable<ICombatEntityProvider> GetSelectedCharacters()
        {
            yield return VanguardType;
            yield return AttackerType;
            yield return SupportType;
            yield return FlexType;
        }
        private static PlayerRunTimeEntity HandleInstantiation(ICombatEntityProvider preset)
        {
            return preset == null ? null : new PlayerRunTimeEntity(preset);
        }

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
