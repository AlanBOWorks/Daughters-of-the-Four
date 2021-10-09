using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace __ProjectExclusive
{
    public class SOnGameStartLoader : ScriptableObject
    {
        private const string FolderPath = "Assets/ScriptableObjects/Globals/";
        private const string AssetName = "FirstLoadParameters.Asset";
        public const string FullAssetPath = FolderPath + AssetName;

        [SerializeField] private List<GameObject> spawnObjectsOnStartApplication = new List<GameObject>(0);
        [SerializeField] private List<GameObject> spawnObjectsOnStartGame = new List<GameObject>(0);
        [SerializeField] private List<GameObject> spawnObjectsOnLoad = new List<GameObject>(0);

        public List<GameObject> SpawnObjectsOnStartApplication => spawnObjectsOnStartApplication;
        public List<GameObject> SpawnObjectsOnStartGame => spawnObjectsOnStartGame;
        public List<GameObject> SpawnObjectsOnLoad => spawnObjectsOnLoad;

        public void OnStartApplication()
        {
            foreach (var gameObject in spawnObjectsOnStartApplication)
            {
                Instantiate(gameObject);
            }
        }

        public void OnGameStart()
        {
            foreach (var gameObject in spawnObjectsOnStartGame)
            {
                Instantiate(gameObject);
            }
        }

        public void OnLoadGame()
        {

        }



        public static SOnGameStartLoader LoadOrCreateStartLoader()
        {
            var loaderAsset = AssetDatabase.LoadAssetAtPath<SOnGameStartLoader>(FullAssetPath);
            if (loaderAsset != null) return loaderAsset;

            loaderAsset = CreateInstance<SOnGameStartLoader>();
            loaderAsset.name = AssetName;
            AssetDatabase.CreateAsset(loaderAsset,FullAssetPath);

            return loaderAsset;
        }
    }


    internal sealed class OnGameStartLoaderWindow : OdinEditorWindow
    {
        [MenuItem("Globals/On First Load [Parameters]")]
        private static void ShowWindow()
        {
            var window = GetWindow<OnGameStartLoaderWindow>();
            window.Load(SOnGameStartLoader.LoadOrCreateStartLoader());
            window.Show();
        }

        private void Load(SOnGameStartLoader loader)
        {
            asset = loader;
            onStartApplication = loader.SpawnObjectsOnStartApplication;
            onStartGame = loader.SpawnObjectsOnStartGame;
            onLoad = loader.SpawnObjectsOnLoad;

            EditorUtility.SetDirty(loader);
        }


        public SOnGameStartLoader asset;
        [ShowInInspector] 
        public List<GameObject> onStartApplication;
        [ShowInInspector] 
        public List<GameObject> onStartGame;
        [ShowInInspector]
        public List<GameObject> onLoad;


    }
}
