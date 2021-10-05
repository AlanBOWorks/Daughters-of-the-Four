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

        [SerializeField] private List<GameObject> spawnObjectsOnStartGame = new List<GameObject>(0);
        [SerializeField] private List<GameObject> spawnObjectsOnLoad = new List<GameObject>(0);

        public List<GameObject> SpawnObjectsOnStartGame => spawnObjectsOnStartGame;
        public List<GameObject> SpawnObjectsOnLoad => spawnObjectsOnLoad;

        public void OnGameStartInstantiateObjects()
        {
            foreach (var gameObject in spawnObjectsOnStartGame)
            {
                Object.Instantiate(gameObject);
            }
        }

        public void OnLoadGameInstantiateObjects()
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
            spawnObjectsOnStartGame = loader.SpawnObjectsOnStartGame;
            spawnObjectsOnLoad = loader.SpawnObjectsOnLoad;
        }


        public SOnGameStartLoader asset;
        [ShowInInspector] 
        public List<GameObject> spawnObjectsOnStartGame;
        [ShowInInspector]
        public List<GameObject> spawnObjectsOnLoad;


    }
}
