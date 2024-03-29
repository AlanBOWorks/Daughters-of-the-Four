using System;
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

        [Title("Core")]
        [SerializeField] private PrefabValues[] coreInstantiateObjects;
        [SerializeField] private SInstantiationScriptable[] coreInstantiateScriptableObjects;
        [Title("OnCombat")]
        [SerializeField] private PrefabValues[] combatInstantiateObjects;


        public void CoreInstantiationRequiredObjects(out GameObject holder)
        {
#if UNITY_EDITOR
            Debug.Log("------ Instantiation (CORE) : " + AssetDatabase.GetAssetPath(this));
#endif
            string holderName = "----- CORE System [HOLDER] ------";
            InstantiateHolder(holderName, out holder, out var holderTransform);
            InstantiateObjects(holderTransform, coreInstantiateObjects);

            foreach (var scriptableObject in coreInstantiateScriptableObjects)
            {
                scriptableObject.DoInstantiation();
            }
        }

        public void CombatInstantiateRequiredObjects()
        {
#if UNITY_EDITOR
            Debug.Log("------ Instantiation (COMBAT) : " + AssetDatabase.GetAssetPath(this));
#endif
            string holderName = "----- Combat System [HOLDER] ------";
            InstantiateHolder(holderName,out var holder, out var holderTransform);
            InstantiateObjects(holderTransform,combatInstantiateObjects);
            
            CombatSystemSingleton.CombatHolderNotDestroyReference = holder;
        }

        private static void InstantiateHolder(string holderName, out GameObject holder, out Transform holderTransform)
        {
            holder = new GameObject(holderName);
            holderTransform = holder.transform;
            DontDestroyOnLoad(holder);
        }
        private static void InstantiateObjects(Transform parent, PrefabValues[] prefabs)
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

    public abstract class SInstantiationScriptable : ScriptableObject
    {
        public abstract void DoInstantiation();
    }
}
