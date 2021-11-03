using System.Collections.Generic;
using System.IO;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using Utils;

namespace __ProjectExclusive.CombatSystem._DB
{
    [CreateAssetMenu(fileName = "N [Skills DataBase]",
        menuName = "Combat/Skills/DataBase",order = -200)]
    public class SSkillsDataBase : ScriptableObject, ITeamRoleStructureRead<List<SSkill>>
    {
        
        [SerializeField, GUIColor(.3f, .5f, .7f)]
        private string dataBaseName = "NULL";
        public string GetDataBaseName() => dataBaseName;

        [TabGroup("VANGUARD")]
        [SerializeField,InlineEditor(InlineEditorObjectFieldModes.Foldout),ListDrawerSettings(NumberOfItemsPerPage = 2)] 
        private List<SSkill> vanguardSkills = new List<SSkill>();
        [TabGroup("ATTACKER")]
        [SerializeField,InlineEditor(InlineEditorObjectFieldModes.Foldout),ListDrawerSettings(NumberOfItemsPerPage = 2)] 
        private List<SSkill> attackerSkills = new List<SSkill>();
        [TabGroup("SUPPORT")]
        [SerializeField,InlineEditor(InlineEditorObjectFieldModes.Foldout),ListDrawerSettings(NumberOfItemsPerPage = 2)] 
        private List<SSkill> supportSkills = new List<SSkill>();

        public List<SSkill> Vanguard => vanguardSkills;
        public List<SSkill> Attacker => attackerSkills;
        public List<SSkill> Support => supportSkills;


        [Button,GUIColor(.3f,.5f,.7f)]
        private void UpdateAssetName()
        {
            name = dataBaseName + " [Skills DataBase] - ID_" + GetInstanceID();
            UtilsAssets.UpdateAssetName(this);
        }

    }

    public class SkillDataBase : OdinEditorWindow
    {
        private const string AssetFolderPath = "Assets/ScriptableObjects/Combat/Skills/_DB/";

        [ShowInInspector,ShowIf("_skills"),InlineEditor(InlineEditorObjectFieldModes.Hidden), ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private List<SSkillsDataBase> _skills;

        [OnInspectorInit]
        internal void LoadAssets()
        {
            Debug.Log("Loading SkillsDataBases......");
            if(_skills == null)
                _skills = new List<SSkillsDataBase>();
            else
                _skills.Clear();

            var assetsPaths = Directory.GetFiles(AssetFolderPath);
            foreach (var assetPath in assetsPaths)
            {
                var asset = AssetDatabase.LoadAssetAtPath<SSkillsDataBase>(assetPath);
                if(asset != null)
                    _skills.Add(asset);
            }
        }

        [Button(ButtonSizes.Large)]
        private void CreateSkillsDataBaseAsset()
        {
            SSkillsDataBase element = CreateInstance<SSkillsDataBase>();
            string assetPath = AssetFolderPath + "N [SkillsDataBase] - ID_" + element.GetInstanceID() + ".asset";
            AssetDatabase.CreateAsset(element,assetPath);
        }


        [MenuItem("Project Tools/Skills DataBases")]
        private static void OpenWindow()
        {
            var windowElement = GetWindow<SkillDataBase>();
            windowElement.LoadAssets();
            windowElement.Show();

        }
    }
}
