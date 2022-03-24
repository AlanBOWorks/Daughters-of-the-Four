using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Utils;

namespace CombatSystem._Core
{
    [CreateAssetMenu(fileName = AssetName,
        menuName = "Combat/_Core/" + AssetName, order = -100)]
    public class SPrefabInstantiationHandler : ScriptableObject
    {
        private const string AssetFolderPath = "Assets/ScriptableObjects/_Core/Instantiations/";
        private const string AssetName = "Prefab Instantiations [Handler]";

        [SerializeField] private GameObject[] instantiateObjects;


        public void InstantiateRequiredObjects()
        {
            Debug.Log("------ Instantiation : " + AssetDatabase.GetAssetPath(this));

            GameObject holder = new GameObject("----- Combat System [HOLDER] ------");
            var holderTransform = holder.transform;
            DontDestroyOnLoad(holder);

            CombatSystemSingleton.CombatHolderNotDestroyReference = holder;
            foreach (var gameObject in instantiateObjects)
            {
                Instantiate(gameObject, holderTransform);
            }
        }

        [Button]
        private void DebugAssetPath()
        {
            const string path = AssetFolderPath + AssetName + UtilsAssets.AssetExtension;
            UtilsAssets.CheckIfPathIsCorrect(path,this);
        }

        public static SPrefabInstantiationHandler GetAsset()
        {
            const string path = AssetFolderPath + AssetName + UtilsAssets.AssetExtension;
            return AssetDatabase.LoadAssetAtPath<SPrefabInstantiationHandler>(path);
        }
    }

    internal sealed class PrefabInstantiationHandler 
    {
        public void FirstInstantiation()
        {
            if(CombatSystemSingleton.CombatHolderNotDestroyReference) return;

            SPrefabInstantiationHandler instantiationHandler = SPrefabInstantiationHandler.GetAsset();
            instantiationHandler.InstantiateRequiredObjects();
        }
    }
}
