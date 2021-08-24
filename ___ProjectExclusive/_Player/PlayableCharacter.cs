using Characters;
using Passives;
using Skills;
using Stats;
using UnityEngine;

namespace _Player
{

    /// <summary>
    /// Used to load characters from file or preset and being an volatile data structure for the
    /// game loop
    /// </summary>
    public class PlayableCharacter : ICharacterCombatProvider, IPlayerCharacterStats
    {
        public readonly SPlayerCharacterEntityVariable Variable;
        public CharacterUpgradeStats UpgradedStats { get; }
        public PlayerCombatStats InitialStats => Variable.InitialStats;
        public PlayerCombatStats GrowStats => Variable.GrowStats;
      

        public PlayableCharacter(SPlayerCharacterEntityVariable variable)
        {
            Variable = variable;
            UpgradedStats = new CharacterUpgradeStats(variable.UpgradedStats);
        }
        // TODO make an constructor or injector for the SaveFile.JSON

        public string CharacterName => Variable.CharacterName;
        public GameObject CharacterPrefab => Variable.CharacterPrefab;
        public CharacterArchetypes.RangeType RangeType => Variable.RangeType;
        public CombatStatsHolder GenerateCombatData()
        {
            return UtilsStats.GenerateCombatData(this);
        }

        public CombatSkills GenerateCombatSkills(CombatingEntity injection)
        {
            /* TODO when made a skills variations change this to a new CombatSkills(shared, unique)
             (such level increment,
            / different  skill slot equipped or added upgrades to the skills)
            */
            
            return Variable.GenerateCombatSkills(injection);
        }

        public SCriticalBuffPreset  GetCriticalBuff() => Variable.GetCriticalBuff();
    }
}
