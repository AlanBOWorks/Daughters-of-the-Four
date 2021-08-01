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
        [SerializeField, Range(-1, 0), SuffixLabel("%00")] 
        private float loseControlThreshold = TeamCombatStatsHolder.DefaultLoseThreshold;
        [SerializeField] 
        private TeamControlLoses controlLoses = new TeamControlLoses();
        [SerializeField,Range(0,100), SuffixLabel("tempoSec"), 
         Tooltip("1 sec equals apr. 1 initiative at 100 Speed")] 
        private float reviveTime = TeamCombatStatsHolder.DefaultReviveTime;

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
        public float GetReviveTime() => reviveTime;

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
        public static TeamControlLoses BackUpData = new TeamControlLoses();

        [SerializeField, Range(0, 1)] private float frontLiner = .4f;
        [SerializeField, Range(0, 1)] private float midLiner = .3f;
        [SerializeField, Range(0, 1)] private float backLiner = .25f;

        public float FrontLiner => frontLiner;
        public float MidLiner => midLiner;
        public float BackLiner => backLiner;
    }

    public interface ITeamCombatControlHolder : 
        IStanceArchetype<ICharacterBasicStats>, IStanceArchetype<FilterPassivesHolder>

    {
        string ControlName { get; }
        float GetLoseThreshold();
        float GetReviveTime();

        ICharacterArchetypesData<float> GetControlLosePoints();
    }

    public interface ITeamCombatControlStats
    {
        ICharacterBasicStats GetCurrentStats();
        FilterPassivesHolder GetCurrentPassives();
    }
}
