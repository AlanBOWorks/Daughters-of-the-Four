using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class UtilsAssets 
    {
        public static void UpdateAssetNameWithID(ScriptableObject asset, string name)
        {
            name += " - " + asset.GetInstanceID();
            UpdateAssetName(asset,name);
        }

        public static void UpdateAssetName(ScriptableObject asset, string name)
        {
            asset.name = name;
            UpdateAssetName(asset);
        }

        public static void UpdateAssetName(ScriptableObject asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(path, asset.name);
        }

        public static void UpdateAssetName(SceneAsset asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.RenameAsset(path, asset.name);
        }

        /// <summary>
        /// [.asset]
        /// </summary>
        public const string AssetExtension = ".asset";

        public static void CheckIfPathIsCorrect(string path, ScriptableObject asset)
        {
            Debug.Log("Manual Path: " + path);
            string getPath = AssetDatabase.GetAssetPath(asset);
            Debug.Log("Get Path: " + getPath);

            bool isCorrect = (path == getPath);
            string logText = $"Are Equals: {isCorrect}";

            if (isCorrect)
                Debug.Log(logText);
            else
                Debug.LogError(logText);
        }

        public static string GetAssetFolder(ScriptableObject asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            int removeAt = path.Length - asset.name.Length - AssetExtension.Length;
            return path.Remove(removeAt);
        }
        public static T GetAsset<T>(T asset) where T : ScriptableObject
        {
            var path = AssetDatabase.GetAssetPath(asset);
            return AssetDatabase.LoadAssetAtPath<T>(path);
        }
    
        /// <param name="addIdToName">The instantiation ID</param>
        /// <param name="addAssetExtension">If you want to add the '.asset'</param>
        /// <returns></returns>
        public static TScriptableObject CreateAsset<TScriptableObject>(string folderPath, string assetName,bool addIdToName, bool addAssetExtension) 
            where TScriptableObject : ScriptableObject
        {
            TScriptableObject generatedAsset = ScriptableObject.CreateInstance<TScriptableObject>();

            string generatedAssetName = assetName;
            string creationPath = folderPath + assetName;

            if (addIdToName)
            {
                string idName = generatedAsset.GetInstanceID().ToString();
                creationPath += idName;
                generatedAssetName += idName;
            }
            if (addAssetExtension == true)
                creationPath += AssetExtension;
         
            AssetDatabase.CreateAsset(generatedAsset,creationPath);
            generatedAsset.name = generatedAssetName;
            return generatedAsset;
        }


        public static TScriptableObject CreateAsset<TScriptableObject>(string folderPath) 
            where TScriptableObject : ScriptableObject
        {
            TScriptableObject generatedAsset = ScriptableObject.CreateInstance<TScriptableObject>();
            string assetName = generatedAsset.GetInstanceID().ToString();

            var creationPath = folderPath + assetName + AssetExtension;
            AssetDatabase.CreateAsset(generatedAsset, creationPath);
            return generatedAsset;
        }



        public static void Destroy(ScriptableObject asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);
            AssetDatabase.DeleteAsset(path);
        }
    }
}
