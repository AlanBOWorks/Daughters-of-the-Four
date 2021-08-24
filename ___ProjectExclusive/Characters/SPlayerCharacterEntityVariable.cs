using ___ProjectExclusive;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;

namespace Characters
{
    [CreateAssetMenu(fileName = "_Character Entity_ - N [Player Variable]",
        menuName = "Variable/Player/Character Entity")]
    public class SPlayerCharacterEntityVariable : SCharacterEntityVariable, IPlayerCharacterStats
    {
        [TitleGroup("Stats")]
        [SerializeField]
        private CharacterUpgradeStats upgradedStats = new CharacterUpgradeStats();
        public CharacterUpgradeStats UpgradedStats => upgradedStats;

        [HorizontalGroup("Combat Stats")]
        [SerializeField,Tooltip("Initial stats from level 0 (this values are constant)")]
        private PlayerCombatStats initialStats = new PlayerCombatStats();
        public PlayerCombatStats InitialStats => initialStats;

        [HorizontalGroup("Combat Stats"),Tooltip("Additional stats that grows the character's power per level")]
        [SerializeField, GUIColor(.5f, .9f, .8f)]
        private PlayerCombatStats growCombatStats = new PlayerCombatStats(0);
        public PlayerCombatStats GrowStats => growCombatStats;


        [Button("Inject in DataBase")]
        private void Awake()
        {
            DataBaseSingleton.Instance.PlayerDataBase.
                InjectVariable(this);
        }

        private void OnDestroy()
        {
            Debug.Log(this);
            DataBaseSingleton.Instance.PlayerDataBase.
                RemoveVariable(this);
        }

        public override CombatStatsHolder GenerateCombatData()
        {
            return UtilsStats.GenerateCombatData(this);
        }
    }

    public interface IPlayerCharacterStats
    {
        CharacterUpgradeStats UpgradedStats { get; }
        PlayerCombatStats InitialStats { get; }
        PlayerCombatStats GrowStats { get; }
    }
}
