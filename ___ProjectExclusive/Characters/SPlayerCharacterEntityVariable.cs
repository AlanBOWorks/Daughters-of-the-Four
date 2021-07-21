using System;
using ___ProjectExclusive;
using ___ProjectExclusive.Animators;
using _Player;
using Sirenix.OdinInspector;
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
        private PlayerCharacterCombatStats initialStats = new PlayerCharacterCombatStats();
        public PlayerCharacterCombatStats InitialStats => initialStats;

        [HorizontalGroup("Combat Stats"),Tooltip("Additional stats that grows the character's power per level")]
        [SerializeField, GUIColor(.5f, .9f, .8f)]
        private PlayerCharacterCombatStats growStats = new PlayerCharacterCombatStats(0);
        public PlayerCharacterCombatStats GrowStats => growStats;


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

        public override CharacterCombatData GenerateCombatData()
        {
            return UtilsStats.GenerateCombatData(this);
        }
    }

    public interface IPlayerCharacterStats
    {
        CharacterUpgradeStats UpgradedStats { get; }
        PlayerCharacterCombatStats InitialStats { get; }
        PlayerCharacterCombatStats GrowStats { get; }
    }
}
