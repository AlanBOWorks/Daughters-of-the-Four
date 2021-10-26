using CombatEntity;
using CombatSystem;
using CombatSystem.Events;
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
            PlayerEvents.SubscribeListener(VirtualSkillsInjector);
            PlayerEvents.SubscribeListener(SkillSelectionsQueue);

#if UNITY_EDITOR
            Debug.Log("[Player Combat Singleton] instantiated");
            PlayerEvents.SubscribeListener(new DebugSkillEvents());
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

        public static void SubscribeEventListener(object listener)
        {
            PlayerEvents.SubscribeListener(listener);
        }


        public static void InjectInCombatSystem()
        {
#if UNITY_EDITOR
            Debug.Log("Injecting Player [Singleton]'s Events into Combat [Singleton]");
#endif

            CombatSystemSingleton.EventsHolder.SubscribeListener(PlayerEvents);
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
