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
            PlayerEvents = new SystemEventsHolder();

            EntitySkillRequestHandler = new EntityRandomController(); //TODO make it a player controller

#if UNITY_EDITOR
            Debug.Log("[Player Combat Singleton] instantiated");
            PlayerEvents.Subscribe(new DebugSkillEvents());
#endif
        }

        private PlayerCombatSingleton()
        { }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [ShowInInspector] 
        public static readonly PlayerCharactersHolder CharactersHolder;

        [ShowInInspector] 
        public static readonly SystemEventsHolder PlayerEvents;

        [ShowInInspector]
        public static readonly IEntitySkillRequestHandler EntitySkillRequestHandler;


        public static void InjectInCombatSystem()
        {
#if UNITY_EDITOR
            Debug.Log("Injecting Player [Singleton]'s Events into Combat [Singleton]");
#endif

            CombatSystemSingleton.EventsHolder.Subscribe(PlayerEvents);
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
