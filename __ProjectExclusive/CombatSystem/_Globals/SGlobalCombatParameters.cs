using System;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem._Globals
{
    [CreateAssetMenu(fileName = "Global Combat Params [Preset]",
        menuName = "Combat/Global/Parameters")]
    public class SGlobalCombatParameters : ScriptableObject,ITeamStructureRead<SSharedSkillSet>
    {
        private const string AssetPath = "Assets/ScriptableObjects/Combat/__Globals/";
        private const string AssetName = "Global Combat Params [Preset].Asset";
        public const string AssetLoadPath = AssetPath + AssetName;

        [SerializeField] private SSharedSkillSet vanguardSkills;
        [SerializeField] private SSharedSkillSet attackerSkills;
        [SerializeField] private SSharedSkillSet supportSKills;

        public SSharedSkillSet Vanguard => vanguardSkills;
        public SSharedSkillSet Attacker => attackerSkills;
        public SSharedSkillSet Support => supportSKills;
    }

    
    public sealed class GlobalCombatParametersSingleton
    {
        internal static readonly GlobalCombatParametersSingleton Instance = new GlobalCombatParametersSingleton();

        static GlobalCombatParametersSingleton()
        {
            SharedSkills =
                AssetDatabase.LoadAssetAtPath<SGlobalCombatParameters>(SGlobalCombatParameters.AssetLoadPath);
        }
        private GlobalCombatParametersSingleton() {}

        [ShowInInspector]
        public static ITeamStructureRead<SSharedSkillSet> SharedSkills;
    }

    internal class GlobalCombatParametersWindow : OdinEditorWindow
    {
        [MenuItem("Debug/Global Parameters Singleton")]
        private static void OpenWindow()
        {
            _instance = GlobalCombatParametersSingleton.Instance;

            var window = GetWindow<GlobalCombatParametersWindow>();
            window.Show();
        }

        [ShowInInspector] private static GlobalCombatParametersSingleton _instance;
    }
}
