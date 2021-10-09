using System;
using __ProjectExclusive.Player;
using CombatSystem;
using Sirenix.OdinInspector;
using UnityEngine;

namespace __ProjectExclusive
{
    public class UOnGameStartLoader : MonoBehaviour
    {
#if UNITY_EDITOR
        [InfoBox(Notification, "thisIsACheckBoxJustToShowThisInfo")] 
        public bool thisIsACheckBoxJustToShowThisInfo = true;
        private const string Notification = "In the Build this will be invoked in the start Game (since this will " +
                                           "be invoked in the first scene preparing all).\n _\n " +
                                           "For debugging is not neccesary. Just call the utility method:\n " +
                                           "GameStateLoader.LoadPrepareInstances()";
        [SerializeField]
        private SOnGameStartLoader _asset;

#endif

        private void Awake()
        {
            GameStateLoader.LoadPrepareInstances();
            Destroy(this);
        }

        
    }

    public sealed class GameStateLoader
    {
        static GameStateLoader()
        {
#if UNITY_EDITOR
            Debug.Log("Creating Singleton Instances...");
#endif

            // Instantiate CombatSingleton
            CombatSystemSingleton.GetInstance();
            // Instantiate PlayerCombatSingleton
            // vv It's no really necessary to use this, but it's looks better and I think I could do something stupid
            // thinking the there's something missing in here. 
            PlayerCombatSingleton.GetInstance(); 

            // Do Injections
            PlayerCombatSingleton.InjectInCombatSystem();

#if UNITY_EDITOR
            Debug.Log("Load Prefabs and Assets...");
#endif
            // Load Assets
            var loader = SOnGameStartLoader.LoadOrCreateStartLoader();
            LoaderAsset = loader;

            loader.OnStartApplication();

            // Todo change this after introducing OnStartGAme/OnLoadGame
            loader.OnGameStart();
            loader.OnLoadGame();
        }

        private static readonly GameStateLoader Instance = new GameStateLoader();
        public static readonly SOnGameStartLoader LoaderAsset;

        public static void LoadPrepareInstances()
        {
            var instance = Instance;
        }

    }
}
