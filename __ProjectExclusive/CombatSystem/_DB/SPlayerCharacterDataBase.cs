using System.Collections.Generic;
using __ProjectExclusive.Player;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem._DB
{
    public class SPlayerCharacterDataBase : ScriptableObject
    {
        [SerializeField]
        private List<SPlayerCombatEntity> characters = new List<SPlayerCombatEntity>();

        public List<SPlayerCombatEntity> GetCharacters() => characters;
    }


    internal class PlayerCharactersDataBase : OdinEditorWindow
    {
        private const string AssetPath = "Assets/DB/";
        private const string AssetName = "PlayerCharacterDataBaseTool.asset";
        private const string FullAssetPath = AssetPath + AssetName;

        [ShowInInspector, HideIf("_assetDataBase")] 
        private SPlayerCharacterDataBase _assetDataBase;

        [ShowInInspector, ShowIf("_assetDataBase")]
        [InlineEditor(InlineEditorModes.GUIAndHeader, Expanded = true)]
        private List<SPlayerCombatEntity> _assets;

        private void UploadPreviews(List<SPlayerCombatEntity> entities)
        {
            _assets = entities;
        }

        [OnInspectorInit]
        internal void LoadAsset()
        {
            _assetDataBase = AssetDatabase.LoadAssetAtPath<SPlayerCharacterDataBase>(FullAssetPath);
            UploadPreviews(_assetDataBase.GetCharacters());

            EditorUtility.SetDirty(_assetDataBase);
        }

        [Button, HideIf("_assetDataBase")]
        private void CreateAssetDatabaseHolder()
        {
            SPlayerCharacterDataBase dataBaseAsset = CreateInstance<SPlayerCharacterDataBase>();
            dataBaseAsset.name = AssetName;

            AssetDatabase.CreateAsset(dataBaseAsset,FullAssetPath);
            _assetDataBase = dataBaseAsset;
        }


        [MenuItem("Project Tools/Player Characters Tool")]
        private static void OpenWindow()
        {
            var windowElement = GetWindow<PlayerCharactersDataBase>();
            windowElement.LoadAsset();
            windowElement.Show();
            
        }
    }
}
