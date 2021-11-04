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
        [SerializeField, 
         ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden), TableList(IsReadOnly = true, DrawScrollView = false)]
        private List<SSkill> vanguardSkills = new List<SSkill>();
        [TabGroup("ATTACKER")]
        [SerializeField,
         ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden), TableList(IsReadOnly = true, DrawScrollView = false)]
        private List<SSkill> attackerSkills = new List<SSkill>();
        [TabGroup("SUPPORT")]
        [SerializeField,
         ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden), TableList(IsReadOnly = true, DrawScrollView = false)]
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
        private const string SkillsAssetsFolderPath = "Assets/ScriptableObjects/Combat/Skills/";
        private const string AssetDataBaseFolderPath = SkillsAssetsFolderPath + "_DB/";
        private const string VanguardSkillsFolderPath = SkillsAssetsFolderPath + "Vanguard/";
        private const string OffensiveSkillsFolderPath = SkillsAssetsFolderPath + "Offensive/";
        private const string SupportSkillsFolderPath = SkillsAssetsFolderPath + "Support/";
        private const string OthersSkillsFolderPath = SkillsAssetsFolderPath + "Others/";

        [TabGroup("Vanguard Skills"), ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden),
         TableList(IsReadOnly = true, DrawScrollView = false, ShowIndexLabels = true)]
        private List<SSkill> _vanguardSkills;
        [TabGroup("Offensive Skills"), ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden),
         TableList(IsReadOnly = true,DrawScrollView = false, ShowIndexLabels = true)]
        private List<SSkill> _offensiveSkills;
        [TabGroup("Support Skills"), ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden), 
         TableList(IsReadOnly = true, DrawScrollView = false, ShowIndexLabels = true)]
        private List<SSkill> _supportSkills;
        [TabGroup("Others Skills"), ShowInInspector, InlineEditor(InlineEditorObjectFieldModes.Hidden),
         TableList(IsReadOnly = true, DrawScrollView = false, ShowIndexLabels = true)]
        private List<SSkill> _otherSkills;


        [TabGroup("Fav Skills"), 
            ShowInInspector,InlineEditor(InlineEditorObjectFieldModes.Hidden), ListDrawerSettings(NumberOfItemsPerPage = 1)]
        private List<SSkillsDataBase> _favoriteSkills;

        [OnInspectorInit]
        private void LoadAssets()
        {
            Debug.Log("Loading SkillsDataBases......");
            LoadAssets(AssetDataBaseFolderPath,ref _favoriteSkills);
            LoadAssets(VanguardSkillsFolderPath,ref _vanguardSkills);
            LoadAssets(OffensiveSkillsFolderPath,ref _offensiveSkills);
            LoadAssets(OthersSkillsFolderPath,ref _otherSkills);

            LoadAssets(SupportSkillsFolderPath + "Buff/", ref _supportSkills, true);
            LoadAssets(SupportSkillsFolderPath + "Protection/", ref _supportSkills, false);

        }
        private static void LoadAssets<T>(string folderPath,ref List<T> injection, bool clear = true) where T : Object
        {
            if(injection == null) 
                injection = new List<T>();
            else if(clear)
                injection.Clear();

            var assetsPaths = Directory.GetFiles(folderPath);
            foreach (var assetPath in assetsPaths)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if(asset != null)
                    injection.Add(asset);
            }
        }

        [Button(ButtonSizes.Large)]
        private void CreateSkillsDataBaseAsset()
        {
            SSkillsDataBase element = CreateInstance<SSkillsDataBase>();
            string assetPath = AssetDataBaseFolderPath + "N [SkillsDataBase] - ID_" + element.GetInstanceID() + ".asset";
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
