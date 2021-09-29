using UnityEditor;
using UnityEngine;

namespace Utils
{
    public static class UtilsAssets 
    {
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

        public const string AssetExtension = ".asset";
        public static string GetAssetFolder(ScriptableObject asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            int removeAt = path.Length - asset.name.Length - AssetExtension.Length;
            return path.Remove(removeAt);
        }
    
        public static T CreateAsset<T>(string folderPath, string assetName,bool addIdToName, bool addAssetExtension) where T : ScriptableObject
        {
            T generatedAsset = ScriptableObject.CreateInstance<T>();

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

        public static T CreateAsset<T>(string folderPath) where T : ScriptableObject
        {
            T generatedAsset = ScriptableObject.CreateInstance<T>();
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
