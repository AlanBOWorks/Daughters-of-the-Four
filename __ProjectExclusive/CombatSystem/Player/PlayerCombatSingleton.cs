using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.Events;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Stats;
using UnityEditor;
using UnityEngine;

namespace __ProjectExclusive.Player
{
    //It's a Singleton instead of a static class for debugging reasons
    public sealed class PlayerCombatSingleton
    {
        private static readonly PlayerCombatSingleton Instance = new PlayerCombatSingleton();

        static PlayerCombatSingleton()
        {
            CharactersHolder = new PlayerCharactersHolder();
            PlayerEvents = new PlayerEvents();



            VirtualSkillsInjector = new PlayerVirtualSkillsInjector();
            SkillSelectionsQueue = new PlayerSkillSelectionsQueue();

            EntitySkillRequestHandler = SkillSelectionsQueue;
            // For Debug: vvvv
            //EntitySkillRequestHandler = new EntityRandomController(); 

            // Subscriptions
            PlayerEvents.Subscribe(VirtualSkillsInjector);
            PlayerEvents.Subscribe((IVirtualSkillInteraction) SkillSelectionsQueue);
            PlayerEvents.Subscribe(SkillSelectionsQueue as IVirtualSkillTargetListener);

#if UNITY_EDITOR
            Debug.Log("[Player Combat Singleton] instantiated");
            PlayerEvents.Subscribe(new DebugSkillEvents());
#endif
        }

        private PlayerCombatSingleton()
        { }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [TabGroup("Player System")]
        [ShowInInspector, Title("Characters")] 
        public static readonly PlayerCharactersHolder CharactersHolder;

        [TabGroup("Player System")]
        [ShowInInspector, Title("Events")]
        public static readonly PlayerEvents PlayerEvents;


        [TabGroup("Player System")]
        [ShowInInspector]
        public static readonly PlayerSkillSelectionsQueue SkillSelectionsQueue;
        [TabGroup("Player System")]
        [ShowInInspector]
        public static readonly PlayerVirtualSkillsInjector VirtualSkillsInjector;


        [TabGroup("Combat System")]
        [ShowInInspector]
        public static readonly IEntitySkillRequestHandler EntitySkillRequestHandler;

        [TabGroup("Instantiations")] 
        public static GameObject CombatUiReference;

        [ShowInInspector,HorizontalGroup("Theme")]
        public static IMasterStatsRead<Color> CombatMasterStatColors;
        public static IBaseStatsRead<Color> CombatBaseStatsColors;
        public static ITeamRoleStructureRead<Color> CombatRoleStructureColors;
        public static ISkillTypesRead<Color> CombatSkillTypesColors;

        [ShowInInspector, HorizontalGroup("Theme")]
        public static IMasterStatsRead<Sprite> CombatMasterStatsIcons;
        public static IBaseStatsRead<Sprite> CombatBaseStatsIcons;
        public static ITeamRoleStructureRead<Sprite> CombatRolesIcons;
        public static ISkillTypesRead<Sprite> CombatSkillTypesIcons;

        public static void InjectInCombatSystem()
        {
#if UNITY_EDITOR
            Debug.Log("Injecting Player [Singleton]'s Events into Combat [Singleton]");
#endif
            
        }
    }

    internal class PlayerCombatSystemWindow : OdinEditorWindow
    {
        [ShowInInspector]
        private PlayerCombatSingleton _system = PlayerCombatSingleton.GetInstance();

        [MenuItem("Debug/Combat System (Player)")]
        private static void OpenWindow()
        {
            var window = GetWindow<PlayerCombatSystemWindow>();
            window.Show();
        }

    }
}
