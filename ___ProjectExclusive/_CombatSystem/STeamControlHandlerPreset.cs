using System;
using ___ProjectExclusive;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace _CombatSystem
{
    [CreateAssetMenu(fileName = "N - Team Control Passive [Preset]",
        menuName = "Combat/Team/Control Passive")]
    public class STeamControlHolderPreset : ScriptableObject, ITeamCombatControlHolder
    {
        [Title("Tooltips")] [SerializeField] private string controlName = "NULL";

        [HorizontalGroup("Stats")]
        [SerializeField] private CharacterCombatStatsFull onAttackStats;
        [HorizontalGroup("Stats")]
        [SerializeField] private CharacterCombatStatsFull onDefendingStats;

        [HorizontalGroup("Passives")]
        [SerializeField] private FilterPassivesHolder onAttackPassives;
        [HorizontalGroup("Passives")]
        [SerializeField] private FilterPassivesHolder onDefendingPassives;


        public string ControlName => controlName;
        public FilterPassivesHolder OnAttackPassives => onAttackPassives;
        public FilterPassivesHolder OnDefendingPassives => onDefendingPassives;
        public ICharacterFullStats OnAttackingStats => onAttackStats;
        public ICharacterFullStats OnDefendingStats => onDefendingStats;

        private const string ControlHandlerPrefix = " - Team Control Passives [Preset]";

        [Button(ButtonSizes.Large)]
        private void UpdateAssetName()
        {
            name = controlName + ControlHandlerPrefix;
            UtilsGame.UpdateAssetName(this);
        }
    }


    public class EmptyTeamControllerHolder : ITeamCombatControlFull
    {
        private const string DefaultControlName = "NULL";
        public static EmptyTeamControllerHolder EmptyTeamController = new EmptyTeamControllerHolder();

        public EmptyTeamControllerHolder()
        {
            ControlName = DefaultControlName;
            OnAttackPassives = new FilterPassivesHolder();
            OnDefendingPassives = OnAttackPassives;
            OnAttackingStats = UtilsStats.ZeroValuesFull;
            OnDefendingStats = OnAttackingStats;
        }

        public string ControlName { get; }
        public FilterPassivesHolder OnAttackPassives { get; }
        public FilterPassivesHolder OnDefendingPassives { get; }
        public ICharacterFullStats OnAttackingStats { get; }
        public ICharacterFullStats OnDefendingStats { get; }
        public ICharacterFullStats GetCurrentStats() => OnAttackingStats;
        public FilterPassivesHolder GetCurrentPassives() => OnAttackPassives;
    }

    public class CombatingTeamControlHolder : ITeamCombatControlFull
    {
        private readonly TeamCombatData _data;

        public string ControlName { get; }
        public FilterPassivesHolder OnAttackPassives { get; }
        public FilterPassivesHolder OnDefendingPassives { get; }
        private readonly FilterPassivesHolder _neutralPassives = FilterPassivesHolder.EmptyHolder;
        public ICharacterFullStats OnAttackingStats { get; }
        public ICharacterFullStats OnDefendingStats { get; }
        private readonly ICharacterFullStats _neutralStats = UtilsStats.ZeroValuesFull;


        public CombatingTeamControlHolder(TeamCombatData data,ITeamCombatControlHolder copyFrom)
        {
            _data = data;
            ControlName = copyFrom.ControlName;
            OnAttackPassives = copyFrom.OnAttackPassives;
            OnDefendingPassives = copyFrom.OnDefendingPassives;
            OnAttackingStats = copyFrom.OnAttackingStats;
            OnDefendingStats = copyFrom.OnDefendingStats;
        }

        public ICharacterFullStats GetCurrentStats()
        {
            return _data.stance switch
            {
                TeamCombatData.Stance.Neutral => _neutralStats,
                TeamCombatData.Stance.Attacking => OnAttackingStats,
                TeamCombatData.Stance.Defending => OnDefendingStats,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_data.stance}]")
            };
        }

        public FilterPassivesHolder GetCurrentPassives()
        {
            return _data.stance switch
            {
                TeamCombatData.Stance.Neutral => _neutralPassives,
                TeamCombatData.Stance.Attacking => OnAttackPassives,
                TeamCombatData.Stance.Defending => OnDefendingPassives,
                _ => throw new ArgumentException("TeamControl seems to trying to access an invalid" +
                                                 $"type of Team.State [{_data.stance}]")
            };
        }
    }

    public interface ITeamCombatControlFull : ITeamCombatControlHandler, ITeamCombatControlHolder
    { }

    public interface ITeamCombatControlHolder
    {
        string ControlName { get; }
        FilterPassivesHolder OnAttackPassives { get; }
        FilterPassivesHolder OnDefendingPassives { get; }
        ICharacterFullStats OnAttackingStats { get; }
        ICharacterFullStats OnDefendingStats { get; }
    }

    public interface ITeamCombatControlHandler
    {
        ICharacterFullStats GetCurrentStats();
        FilterPassivesHolder GetCurrentPassives();
    }
}
