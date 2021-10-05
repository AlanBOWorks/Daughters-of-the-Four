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
            Events = new PlayerEvents();

            EntitySkillRequestHandler = new EntityRandomController(); //TODO make it a player controller

#if UNITY_EDITOR
            Debug.Log("[Player Combat Singleton] instantiated");
            
#endif
        }

        private PlayerCombatSingleton()
        { }

        public static PlayerCombatSingleton GetInstance() => Instance;

        [ShowInInspector] 
        public static readonly PlayerCharactersHolder CharactersHolder;

        [ShowInInspector] 
        public static readonly PlayerEvents Events;

        [ShowInInspector]
        public static readonly IEntitySkillRequestHandler EntitySkillRequestHandler;


        public static void InjectInCombatSystem()
        {
#if UNITY_EDITOR
            Debug.Log("Injecting Player [Singleton]'s Events into Combat [Singleton]");
#endif

            CombatSystemSingleton.EntitiesFixedEvents.SubscribePlayerEvents(Events);
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
