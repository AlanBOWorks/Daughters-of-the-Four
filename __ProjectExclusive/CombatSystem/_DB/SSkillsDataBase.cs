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
  

    public class SkillDataBase : OdinEditorWindow
    {
        private const string SkillsAssetsFolderPath = "Assets/ScriptableObjects/Combat/Skills/";
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



        [OnInspectorInit]
        private void LoadAssets()
        {
            Debug.Log("Loading SkillsDataBases......");

            InjectVanguardSkillsList(_vanguardSkills);
            InjectOffensiveSkillsList(_offensiveSkills);
            InjectSupportSkillsList(_supportSkills);
            InjectOthersSkillsList(_otherSkills);
        }
       
        private static void LoadAssets<T>(string folderPath, List<T> injection, bool clear = true) where T : Object
        {
            if (injection == null)
                injection = new List<T>();
            else if (clear)
                injection.Clear();

            var assetsPaths = Directory.GetFiles(folderPath);
            foreach (var assetPath in assetsPaths)
            {
                T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                    injection.Add(asset);
            }
        }


        public static void InjectVanguardSkillsList(List<SSkill> injectInList)
        {
            LoadAssets(VanguardSkillsFolderPath, injectInList);
        }

        public static void InjectOffensiveSkillsList(List<SSkill> injectInList)
        {
            LoadAssets(OffensiveSkillsFolderPath, injectInList);
        }

        public static void InjectSupportSkillsList(List<SSkill> injectInList)
        {
            LoadAssets(SupportSkillsFolderPath + "Buff/", injectInList, true);
            LoadAssets(SupportSkillsFolderPath + "Protection/", injectInList, false);
        }
        public static void InjectOthersSkillsList(List<SSkill> injectInList)
        {
            LoadAssets(OthersSkillsFolderPath, injectInList);
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
