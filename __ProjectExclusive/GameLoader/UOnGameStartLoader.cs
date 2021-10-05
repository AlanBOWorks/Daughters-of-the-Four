using System;
using __ProjectExclusive.Player;
using CombatSystem;
using UnityEngine;

namespace __ProjectExclusive
{
    public class UOnGameStartLoader : MonoBehaviour
    {
        private void Awake()
        {
            OnGameStartLoad();
            Destroy(this);
        }

        public static void OnGameStartLoad()
        {
            GameStateLoader.InvokeInstance();
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
            loader.OnGameStartInstantiateObjects();
            LoaderAsset = loader;
        }

        private static readonly GameStateLoader Instance = new GameStateLoader();
        public static readonly SOnGameStartLoader LoaderAsset;

        public static void InvokeInstance()
        {
            var instance = Instance;
        }
    }
}
