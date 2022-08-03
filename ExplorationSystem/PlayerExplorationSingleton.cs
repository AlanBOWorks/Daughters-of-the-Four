using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace ExplorationSystem
{
    public sealed class PlayerExplorationSingleton : ITeamFlexStructureRead<PlayerRunTimeEntity>
    {
        public static readonly PlayerExplorationSingleton Instance;

        static PlayerExplorationSingleton()
        {
            Instance = new PlayerExplorationSingleton();
        }

        public void InjectTeam(ITeamFlexStructureRead<ICombatEntityProviderHolder> team)
        {
            VanguardType = HandleInstantiation(team.VanguardType.GetEntityProvider());
            AttackerType = HandleInstantiation(team.AttackerType.GetEntityProvider());
            SupportType = HandleInstantiation(team.SupportType.GetEntityProvider());
            FlexType = HandleInstantiation(team.FlexType.GetEntityProvider());
        }

        [ShowInInspector, ShowIf("VanguardType")]
        public PlayerRunTimeEntity VanguardType { get; private set; }
        [ShowInInspector, ShowIf("AttackerType")]
        public PlayerRunTimeEntity AttackerType { get; private set; }
        [ShowInInspector, ShowIf("SupportType")]
        public PlayerRunTimeEntity SupportType { get; private set; }
        [ShowInInspector, ShowIf("FlexType")]
        public PlayerRunTimeEntity FlexType { get; private set; }

        private static PlayerRunTimeEntity HandleInstantiation(ICombatEntityProvider preset)
        {
            return preset == null ? null : new PlayerRunTimeEntity(preset);
        }
    }

    public sealed class PlayerExplorationSingletonWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private PlayerExplorationSingleton _explorationSingleton;


        [MenuItem("Game/Debug/Player Exploration [Singleton]")]
        private static void ShowWindow()
        {
            var window = GetWindow<PlayerExplorationSingletonWindow>();
            window._explorationSingleton = PlayerExplorationSingleton.Instance;
        }
    }
}
