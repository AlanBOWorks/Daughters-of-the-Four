using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEditor;
using UnityEngine;
using Utils;

namespace __ProjectExclusive.Player
{
    [CreateAssetMenu(fileName = "N [Player Combat Entity]",
        menuName = "Combat/Entity/Player Combined Preset")]
    public class SPlayerCombatEntity : ScriptableObject
    {
        [Title("Differentiation")]
        [SerializeField] private SCharacterBackground characterBackground;

        [InfoBox("To create Assets it's necessary to have a folder with the name [DataHolder] in this level of" +
                 "the Asset"), GUIColor(.7f, 1f, .8f)]
        [SerializeField,DisableIf("characterBackground"), ShowIf("characterBackground")]
        private List<SCombatEntityUpgradeablePreset> combatPresets 
            = new List<SCombatEntityUpgradeablePreset>();

        
        [Space]
        [ShowInInspector,ListDrawerSettings(IsReadOnly = true,NumberOfItemsPerPage = 1, Expanded = true)]
        internal List<PreviewPreset> _combatPresetPreview;

        
        [ResponsiveButtonGroup(DefaultButtonSize = ButtonSizes.Large), GUIColor(.2f,.8f,.5f)]
        private void CreatePreset()
        {
            var creationPath = UtilsAssets.GetAssetFolder(this);
            creationPath += "DataHolder/";
            SCombatEntityUpgradeablePreset generatedPreset 
                = UtilsAssets.CreateAsset<SCombatEntityUpgradeablePreset>(creationPath);
            combatPresets.Add(generatedPreset);
            UpdatePresetName();

            if (_combatPresetPreview == null)
                ShowPresetPreview();
            else
            {
                _combatPresetPreview.Add(new PreviewPreset(this,generatedPreset));
            }

            void UpdatePresetName()
            {
                var characterName = characterBackground.GetCharacterName();
                generatedPreset.UpdateAssetName(characterName);
            }
        }

        private void RemovePreset(SCombatEntityUpgradeablePreset preset)
        {
            for (var i = 0; i < combatPresets.Count; i++)
            {
                var combatPreset = combatPresets[i];
                if (combatPreset != preset) continue;

                combatPresets.RemoveAt(i);
                _combatPresetPreview.RemoveAt(i);

                UtilsAssets.Destroy(preset);
                break;
            }
        }

        [ResponsiveButtonGroup, ShowIf("_combatPresetPreview")]
        private void ForceShowPreset() => ShowPresetPreview();


        [OnInspectorInit]
        internal void ShowPresetPreview()
        {
            _combatPresetPreview = new List<PreviewPreset>(combatPresets.Count);
            foreach (var preset in combatPresets)
            {
                _combatPresetPreview.Add(new PreviewPreset(this,preset));
            }
        }

        internal class PreviewPreset : ICombatEntityUpgradableReflection
        {
            public PreviewPreset(SPlayerCombatEntity entityHolder, SCombatEntityUpgradeablePreset preset)
            {
                _entityHolder = entityHolder;
                _preset = preset;
                preset.DoReflection(this);

                _combatSkillPreset = preset.SkillProvider;
            }

            private readonly SPlayerCombatEntity _entityHolder;
            [ShowInInspector,DisableIf("_preset")]
            private readonly SCombatEntityUpgradeablePreset _preset;

            [Title("Area")] 
            [ShowInInspector]
            private AreaData _areaData;

            [ShowInInspector,HorizontalGroup(Title = "___________ Stats ______________________")]
            private BaseStats _baseStats;

            [ShowInInspector,HorizontalGroup()] 
            private BaseStats _growStats;

            [Title("Skills")]
            [ShowInInspector] 
            private ITeamStanceStructureRead<ICollection<SkillProviderParams>> _combatSkillPreset;

            public AreaData AreaDataHolder { set => _areaData = value; }
            public BaseStats BaseStats { set => _baseStats = value; }
            public BaseStats GrowStats { set => _growStats = value; }

            [Button, GUIColor(.7f,.8f,1)]
            private void UpdatePresetName()
            {
                var characterName = _entityHolder.characterBackground.GetCharacterName();
                _preset.UpdateAssetName(characterName);
            }

            [Button,GUIColor(.9f,.2f,.1f)]
            private void RemoveMe(string confirmationCheck = "Write: [Delete] to confirm")
            {
                if(!confirmationCheck.Equals("Delete"))
                {
                    Debug.LogError("Written confirmation must be [Delete] without the '[' ']'");
                    return;
                }

                _entityHolder.RemovePreset(_preset);
                Debug.Log("Preset was removed.");
            }
        }
    }
}
