using System;
using ___ProjectExclusive;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace _Team
{
    [CreateAssetMenu(fileName = "N - Team Control Passive [Preset]",
        menuName = "Combat/Team/Control Passive")]
    public class STeamControlStatsPreset : ScriptableObject, ITeamCombatControlHolder
    {
        [Title("Tooltips")] [SerializeField] private string controlName = "NULL";

        [HorizontalGroup("Stats")]
        [SerializeField] private CharacterCombatStatsFull onAttackStats;
        [HorizontalGroup("Stats")]
        [SerializeField] private CharacterCombatStatsFull onNeutralStats;
        [HorizontalGroup("Stats")]
        [SerializeField] private CharacterCombatStatsFull onDefendingStats;

        [HorizontalGroup("Passives")]
        [SerializeField] private FilterPassivesHolder onAttackPassives;
        [HorizontalGroup("Passives")]
        [SerializeField] private FilterPassivesHolder onNeutralPassives;
        [HorizontalGroup("Passives")]
        [SerializeField] private FilterPassivesHolder onDefendingPassives;
        public string ControlName => controlName;

        private const string ControlHandlerPrefix = " - Team Control Passives [Preset]";

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = controlName + ControlHandlerPrefix;
            UtilsGame.UpdateAssetName(this);
        }

        ICharacterBasicStats IStanceArchetype<ICharacterBasicStats>.GetAttacking()
            => onAttackStats;
        ICharacterBasicStats IStanceArchetype<ICharacterBasicStats>.GetNeutral()
            => onNeutralStats;
        ICharacterBasicStats IStanceArchetype<ICharacterBasicStats>.GetDefending()
            => onDefendingStats;

        FilterPassivesHolder IStanceArchetype<FilterPassivesHolder>.GetAttacking()
            => onAttackPassives;
        FilterPassivesHolder IStanceArchetype<FilterPassivesHolder>.GetNeutral()
            => onNeutralPassives;
        FilterPassivesHolder IStanceArchetype<FilterPassivesHolder>.GetDefending()
            => onDefendingPassives;
    }

    public interface ITeamCombatControlHolder : 
        IStanceArchetype<ICharacterBasicStats>, IStanceArchetype<FilterPassivesHolder>

    {
        string ControlName { get; }
    }

    public interface ITeamCombatControlStats
    {
        ICharacterBasicStats GetCurrentStats();
        FilterPassivesHolder GetCurrentPassives();
    }
}
