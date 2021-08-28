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
        [SerializeField] private CombatStatsFull onAttackStats;
        [HorizontalGroup("Neutral")]
        [SerializeField] private CombatStatsFull onNeutralStats;
        [HorizontalGroup("Defend")]
        [SerializeField] private CombatStatsFull onDefendingStats;

        public string ControlName => controlName;
        public float GetLoseThreshold() => loseControlThreshold;
        public float GetReviveTime() => reviveTime;
        public int GetBurstControlLength() => controlBurstLength;
        public int GetBurstCounterAmount() => counterBurstAmount;

        public ICharacterArchetypesData<float> GetControlLosePoints() => controlLoses;


        IBasicStats<float> IStanceData<IBasicStats<float>>.AttackingStance
            => onAttackStats;
        IBasicStats<float> IStanceData<IBasicStats<float>>.NeutralStance
            => onNeutralStats;
        IBasicStats<float> IStanceData<IBasicStats<float>>.DefendingStance
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

        public float Vanguard => frontLiner;
        public float Attacker => midLiner;
        public float Support => backLiner;
    }

    public interface ITeamCombatControlHolder : 
        IStanceData<IBasicStats<float>>

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

    public interface ITeamCombatControlStats : IStanceElement<IBasicStatsData<float>>
    { }
}
