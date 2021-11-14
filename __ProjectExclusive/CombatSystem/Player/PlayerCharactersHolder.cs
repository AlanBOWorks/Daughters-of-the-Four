using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using Sirenix.OdinInspector;
using Stats;
using CombatTeam;

namespace __ProjectExclusive.Player
{
    public sealed class PlayerCharactersHolder : ITeamRoleStructure<PlayableCharacterEntity>,
        ITeamProvider
    {
       
        public void InjectMember(SCombatEntityUpgradeablePreset preset, EnumTeam.Role role)
        {
            UtilsTeam.InjectElement(role,this, new PlayableCharacterEntity(preset));
        }

        public void InjectMembers(ITeamRoleStructureRead<SCombatEntityUpgradeablePreset> presets)
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

        ICombatEntityProvider ITeamRoleStructureRead<ICombatEntityProvider>.Attacker => Attacker;
        ICombatEntityProvider ITeamRoleStructureRead<ICombatEntityProvider>.Support => Support;
        ICombatEntityProvider ITeamRoleStructureRead<ICombatEntityProvider>.Vanguard => Vanguard;
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

        public string GetEntityName() => CharacterPreset.GetEntityName();
        public UEntityHolder GetEntityPrefab() => CharacterPreset.GetEntityPrefab();

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

        public AreaData GenerateAreaData()
        {
            return (CharacterPreset.GetAreaData());
        }

        public ITeamStanceStructureRead<ICollection<SkillProviderParams>> ProvideStanceSkills()
            => CharacterPreset;
    }
}
