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

        [Title("Control")]
        [SerializeField, Range(-1, 0)] 
        private float loseControlThreshold = -.6f;
        [SerializeField] 
        private TeamControlLoses controlLoses = new TeamControlLoses();

        [Title("Stats")]
        [HorizontalGroup("Attack")]
        [SerializeField] private CharacterCombatStatsFull onAttackStats;
        [HorizontalGroup("Neutral")]
        [SerializeField] private CharacterCombatStatsFull onNeutralStats;
        [HorizontalGroup("Defend")]
        [SerializeField] private CharacterCombatStatsFull onDefendingStats;

        [HorizontalGroup("Attack")]
        [SerializeField] private FilterPassivesHolder onAttackPassives;
        [HorizontalGroup("Neutral")]
        [SerializeField] private FilterPassivesHolder onNeutralPassives;
        [HorizontalGroup("Defend")]
        [SerializeField] private FilterPassivesHolder onDefendingPassives;
        public string ControlName => controlName;
        public float GetLoseThreshold() => loseControlThreshold;
        public ICharacterArchetypesData<float> GetControlLosePoints() => controlLoses;



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




        private const string ControlHandlerPrefix = " - Team Control Passives [Preset]";
        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = controlName + ControlHandlerPrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }

    [Serializable]
    public class TeamControlLoses : ICharacterArchetypesData<float>
    {
        [SerializeField, Range(0, 1)] private float frontLiner = .7f;
        [SerializeField, Range(0, 1)] private float midLiner = .5f;
        [SerializeField, Range(0, 1)] private float backLiner = .4f;

        public float FrontLiner => frontLiner;
        public float MidLiner => midLiner;
        public float BackLiner => backLiner;
    }

    public interface ITeamCombatControlHolder : 
        IStanceArchetype<ICharacterBasicStats>, IStanceArchetype<FilterPassivesHolder>

    {
        string ControlName { get; }
        float GetLoseThreshold();
        ICharacterArchetypesData<float> GetControlLosePoints();
    }

    public interface ITeamCombatControlStats
    {
        ICharacterBasicStats GetCurrentStats();
        FilterPassivesHolder GetCurrentPassives();
    }
}
