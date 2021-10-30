using CombatEntity;
using CombatSystem;
using CombatSystem.Events;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
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
            EntitySkillRequestHandler = new EntityRandomController(); //TODO make it a player controller

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


        public static void InjectInCombatSystem()
        {
#if UNITY_EDITOR
            Debug.Log("Injecting Player [Singleton]'s Events into Combat [Singleton]");
#endif

            CombatSystemSingleton.EventsHolder.Subscribe(PlayerEvents as ISkillEventListener);
            CombatSystemSingleton.EventsHolder.Subscribe(PlayerEvents as ITeamStateChangeListener<CombatingTeam>);
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
