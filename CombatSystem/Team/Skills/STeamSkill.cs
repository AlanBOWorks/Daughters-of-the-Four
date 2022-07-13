using CombatSystem.Skills;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatSystem.Team
{
    [CreateAssetMenu(fileName = "N" + AssetSuffix,
        menuName = "Combat/Skill/Team Preset", order = -50)]
    public class STeamSkill : ScriptableObject, ITeamSkillPreset
    {
        [Title("Info")]
        [SerializeField] private string skillPresetName = "NULL";

        [Title("Values")]
        [SerializeField]
        private SSkillPreset.PresetEffectValues[] effects = new SSkillPreset.PresetEffectValues[0];

        public string GetSkillPresetName() => skillPresetName;
        internal SSkillPreset.PresetEffectValues[] GetEffects() => effects;

        private const string AssetSuffix = " [Team Skill]";
        [Button]
        private void UpdateAssetName()
        {
            string assetName = skillPresetName + AssetSuffix;
            UtilsAssets.UpdateAssetNameWithID(this, assetName);
        }
    }

    public interface ITeamSkillPreset
    {
        string GetSkillPresetName();
    }
}
