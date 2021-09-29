using CombatEntity;
using CombatSystem;
using Sirenix.OdinInspector;
using Stats;
using CombatTeam;

namespace __ProjectExclusive.Player
{
    public sealed class PlayerCharactersHolder : ITeamStructure<PlayableCharacterEntity>,
        ITeamProvider
    {
       
        public void InjectMember(SCombatEntityUpgradeablePreset preset, EnumTeam.Role role)
        {
            UtilsTeam.InjectElement(this,role,new PlayableCharacterEntity(preset));
        }

        public void InjectMembers(ITeamStructureRead<SCombatEntityUpgradeablePreset> presets)
        {
            UtilsTeam.InjectElements(this,presets,GenerateEntity);

            PlayableCharacterEntity GenerateEntity(SCombatEntityUpgradeablePreset parsePreset)
            {
                return parsePreset == null 
                    ? null 
                    : new PlayableCharacterEntity(parsePreset);
            }
        }

        [ShowInInspector]
        public PlayableCharacterEntity Vanguard { get; set; }
        [ShowInInspector]
        public PlayableCharacterEntity Attacker { get; set; }
        [ShowInInspector]
        public PlayableCharacterEntity Support { get; set; }

        ICombatEntityProvider ITeamStructureRead<ICombatEntityProvider>.Attacker => Attacker;
        ICombatEntityProvider ITeamStructureRead<ICombatEntityProvider>.Support => Support;
        ICombatEntityProvider ITeamStructureRead<ICombatEntityProvider>.Vanguard => Support;
    }

    public class PlayableCharacterEntity : ICombatEntityProvider
    {
        public PlayableCharacterEntity(SCombatEntityUpgradeablePreset preset)
        {
            CharacterPreset = preset;
            UpgradedStats = new MasterStats();
        }

        public readonly SCombatEntityUpgradeablePreset CharacterPreset;
        public readonly MasterStats UpgradedStats;

        public void LoadUpgrades(IMasterStatsRead<float> upgrades)
        {
            UtilStats.OverrideStats(UpgradedStats,upgrades);
        }

        public CombatStatsHolder GenerateStatsHolder()
        {
            var growStats = CharacterPreset.GrowStats;
            var baseStats = CharacterPreset.BaseStats;

            // Stats calculation for leveling
            BaseStats combatingStats = new BaseStats(growStats);
            UtilStats.MultiplyStats(combatingStats, combatingStats, UpgradedStats);
            // Base is added at the end because the multiplier shouldn't be applied to it
            UtilStats.SumStats(combatingStats,baseStats);

            return new CombatStatsHolder(combatingStats);
        }

        public CombatingAreaData GenerateAreaData()
        {
            return new CombatingAreaData(CharacterPreset.GetAreaData());
        }

    }
}
