using System;
using ___ProjectExclusive;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace _Team
{
    [CreateAssetMenu(fileName = "N"+ ControlHandlerPrefix,
        menuName = "Combat/Team/Team Control Stats")]
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

        [SerializeField, Range(0, 10), SuffixLabel("Rounds")] private int controlBurstLength = 4;
        [SerializeField, Range(0, 10), SuffixLabel("Rounds")] private int counterBurstAmount = 2;


        [Title("Stats")]
        [HorizontalGroup("Attack")]
        [SerializeField] private CharacterCombatStatsFull onAttackStats;
        [HorizontalGroup("Neutral")]
        [SerializeField] private CharacterCombatStatsFull onNeutralStats;
        [HorizontalGroup("Defend")]
        [SerializeField] private CharacterCombatStatsFull onDefendingStats;

        public string ControlName => controlName;
        public float GetLoseThreshold() => loseControlThreshold;
        public float GetReviveTime() => reviveTime;
        public int GetBurstControlLength() => controlBurstLength;
        public int GetBurstCounterAmount() => counterBurstAmount;

        public ICharacterArchetypesData<float> GetControlLosePoints() => controlLoses;


        ICharacterBasicStats IStanceData<ICharacterBasicStats>.AttackingStance
            => onAttackStats;
        ICharacterBasicStats IStanceData<ICharacterBasicStats>.NeutralStance
            => onNeutralStats;
        ICharacterBasicStats IStanceData<ICharacterBasicStats>.DefendingStance
            => onDefendingStats;



        private const string ControlHandlerPrefix = " - Team Control Stats [Preset]";
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
        IStanceData<ICharacterBasicStats>

    {
        string ControlName { get; }
        float GetLoseThreshold();
        float GetReviveTime();
        /// <summary>
        /// How many rounds a BurstControl a team can hold
        /// </summary>
        /// <returns></returns>
        int GetBurstControlLength();
        /// <summary>
        /// How much a CounterBurst a team can reduce
        /// </summary>
        /// <returns></returns>
        int GetBurstCounterAmount();

        ICharacterArchetypesData<float> GetControlLosePoints();
    }

    public interface ITeamCombatControlStats : IStanceElement<ICharacterBasicStatsData>
    { }
}
