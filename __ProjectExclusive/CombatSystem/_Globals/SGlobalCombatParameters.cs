using System;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace CombatSystem
{
    [CreateAssetMenu(fileName = "Global Combat Params [Preset]",
        menuName = "Combat/Global/Parameters")]
    public class SGlobalCombatParameters : ScriptableObject
    {
        private const string AssetPath = "Assets/ScriptableObjects/Globals/";
        private const string AssetName = "Global Combat Params [Preset].Asset";
        public const string AssetLoadPath = AssetPath + AssetName;

        [SerializeField]
        private GlobalCombatParameters parameters = new GlobalCombatParameters();
        public GlobalCombatParameters Parameters => parameters;
    }

    [Serializable]
    public class GlobalCombatParameters : ITeamRoleStructureRead<SSharedSkillSet>
    {
        [Title("Prefabs")]
        [SerializeField] private UEntityHolder provisionalEntityHolderPrefab;

        public UEntityHolder GetProvisionalEntityHolderPrefab() => provisionalEntityHolderPrefab;

        [Title("Skills")] 
        [SerializeField] private SSkill waitSkill;
        [SerializeField] private SSharedSkillSet vanguardSkills;
        [SerializeField] private SSharedSkillSet attackerSkills;
        [SerializeField] private SSharedSkillSet supportSKills;


        public SSkill WaitSkill => waitSkill;
        public SSharedSkillSet Vanguard => vanguardSkills;
        public SSharedSkillSet Attacker => attackerSkills;
        public SSharedSkillSet Support => supportSKills;
    }
    
    public sealed class GlobalCombatParametersSingleton
    {
        internal static readonly GlobalCombatParametersSingleton Instance = new GlobalCombatParametersSingleton();

        static GlobalCombatParametersSingleton()
        {
            Asset =
                AssetDatabase.LoadAssetAtPath<SGlobalCombatParameters>(SGlobalCombatParameters.AssetLoadPath);
            Parameters = Asset.Parameters;
            ProvisionalEntityHolderPrefab = Parameters.GetProvisionalEntityHolderPrefab();

            EditorUtility.SetDirty(Asset);
        }
        private GlobalCombatParametersSingleton() {}

        [ShowInInspector, DisableIf("Asset")] 
        private static readonly SGlobalCombatParameters Asset;
        [ShowInInspector] 
        public static readonly GlobalCombatParameters Parameters;

        public static readonly UEntityHolder ProvisionalEntityHolderPrefab;
        public static ITeamRoleStructureRead<SSharedSkillSet> SharedSkills => Parameters;
    }

    internal class GlobalCombatParametersWindow : OdinEditorWindow
    {
        [MenuItem("Globals/Combat [Parameters]")]
        private static void OpenWindow()
        {
            _instance = GlobalCombatParametersSingleton.Instance;

            var window = GetWindow<GlobalCombatParametersWindow>();
            window.Show();
        }

        [ShowInInspector] private static GlobalCombatParametersSingleton _instance;
    }
}
