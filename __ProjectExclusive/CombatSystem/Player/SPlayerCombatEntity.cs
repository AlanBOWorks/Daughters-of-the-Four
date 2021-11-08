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

        [SerializeField,ShowIf("characterBackground"), InlineEditor(InlineEditorObjectFieldModes.Foldout,Expanded = true),
        ListDrawerSettings(NumberOfItemsPerPage = 1, ShowItemCount = true, ShowIndexLabels = true)]
        private List<SCombatEntityUpgradeablePreset> combatPresets 
            = new List<SCombatEntityUpgradeablePreset>();

    }
}
