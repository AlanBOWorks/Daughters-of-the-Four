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

        [ShowInInspector, ShowIf("_assetDataBase"), TabGroup("Assets")]
        private List<SPlayerCombatEntity> _assets;
        [ShowInInspector, ShowIf("_assetDataBase"), TabGroup("Preview"), PropertyOrder(-10)]
        private List<SPlayerCombatEntity.PreviewPreset>[] _previews;

        private void UploadPreviews(List<SPlayerCombatEntity> entities)
        {
            _assets = entities;
            _previews = new List<SPlayerCombatEntity.PreviewPreset>[entities.Count];
            for (var i = 0; i < entities.Count; i++)
            {
                var entity = entities[i];
                _previews[i] = entity._combatPresetPreview;
                entity.ShowPresetPreview();
            }
        }

        [OnInspectorInit]
        internal void LoadAsset()
        {
            _assetDataBase = AssetDatabase.LoadAssetAtPath<SPlayerCharacterDataBase>(FullAssetPath);
            UploadPreviews(_assetDataBase.GetCharacters());
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
