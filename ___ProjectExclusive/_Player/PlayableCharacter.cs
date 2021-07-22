using Characters;
using Passives;
using Skills;
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
        public PlayerCharacterCombatStats InitialStats => Variable.InitialStats;
        public PlayerCharacterCombatStats GrowStats => Variable.GrowStats;
        public readonly PassivesHolder PassivesHolder;

        public PlayableCharacter(SPlayerCharacterEntityVariable variable)
        {
            Variable = variable;
            UpgradedStats = new CharacterUpgradeStats(variable.UpgradedStats);
            PassivesHolder = new PassivesHolder(variable.GetPassivesHolder());
        }
        // TODO make an constructor or injector for the SaveFile.JSON

        public string CharacterName => Variable.CharacterName;
        public GameObject CharacterPrefab => Variable.CharacterPrefab;
        public CharacterArchetypes.RangeType RangeType => Variable.RangeType;
        public CharacterCombatData GenerateCombatData()
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
        public PassivesHolder GetPassivesHolder() => PassivesHolder;
    }
}
