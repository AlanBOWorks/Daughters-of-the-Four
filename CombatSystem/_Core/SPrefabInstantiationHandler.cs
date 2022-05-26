using System;
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

        [SerializeField] private PrefabValues[] coreInstantiateObjects;
        [SerializeField] private PrefabValues[] combatInstantiateObjects;

        public void CoreInstantiationRequiredObjects(out GameObject holder)
        {
#if UNITY_EDITOR
            Debug.Log("------ Instantiation (CORE) : " + AssetDatabase.GetAssetPath(this));
#endif
            string holderName = "----- CORE System [HOLDER] ------";
            InstantiateHolder(in holderName, out holder, out var holderTransform);
            InstantiateObjects(in holderTransform, in coreInstantiateObjects);

        }

        public void CombatInstantiateRequiredObjects()
        {
#if UNITY_EDITOR
            Debug.Log("------ Instantiation (COMBAT) : " + AssetDatabase.GetAssetPath(this));
#endif
            string holderName = "----- Combat System [HOLDER] ------";
            InstantiateHolder(in holderName,out var holder, out var holderTransform);
            InstantiateObjects(in holderTransform,in combatInstantiateObjects);
            
            CombatSystemSingleton.CombatHolderNotDestroyReference = holder;
        }

        private static void InstantiateHolder(in string holderName, out GameObject holder, out Transform holderTransform)
        {
            holder = new GameObject(holderName);
            holderTransform = holder.transform;
            DontDestroyOnLoad(holder);
        }
        private static void InstantiateObjects(in Transform parent, in PrefabValues[] prefabs)
        {
            foreach (var values in prefabs)
            {
                var prefab = values.prefab;
                var gameObject = Instantiate(prefab, parent);
                if(values.disableOnInstantiation)
                    gameObject.SetActive(false);
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

        [Serializable]
        private struct PrefabValues
        {
            public GameObject prefab;
            public bool disableOnInstantiation;
        }
    }

    internal sealed class AssetPrefabInstantiationHandler
    {
        private static GameObject _coreInstantiationReference;

        [RuntimeInitializeOnLoadMethod]
        public static void FirstCoreObjectsInstantiation()
        {
            if(!Application.isPlaying) return;
            if(_coreInstantiationReference) return;

            SPrefabInstantiationHandler instantiationHandler = SPrefabInstantiationHandler.GetAsset();
            instantiationHandler.CoreInstantiationRequiredObjects(out var holder);
            _coreInstantiationReference = holder;
        }

        public static void FirstCombatObjectsInstantiation()
        {
            if(CombatSystemSingleton.CombatHolderNotDestroyReference) return;

            SPrefabInstantiationHandler instantiationHandler = SPrefabInstantiationHandler.GetAsset();
            instantiationHandler.CombatInstantiateRequiredObjects();
        }
    }
}
