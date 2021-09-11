using System;
using _CombatSystem;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Skills;
using Stats;


namespace _Team
{
    public class CombatingTeam : CharacterArchetypesList<CombatingEntity>, ITeamCombatControlStats
    {
        public CombatingTeam(ITeamCombatControlHolder holder,bool isPlayerTeam, int amountOfEntities = AmountOfArchetypes)
            : base(amountOfEntities)
        {
            IsPlayerTeam = isPlayerTeam;
            control = new CombatTeamControl();
            if (holder != null)
            {
                StatsHolder = new TeamCombatStatsHolder(control, holder);
            }
            else
                StatsHolder = new TeamCombatStatsHolder(control);

            knockOutHandler = new MemberKnockOutHandler(this);
        }

        public readonly bool IsPlayerTeam;
        [ShowInInspector] public readonly CombatTeamControl control;
        [ShowInInspector] public readonly TeamCombatStatsHolder StatsHolder;
        [ShowInInspector] public MemberKnockOutHandler knockOutHandler;

        public IBasicStatsData<float> GetCurrentStanceValue() 
            => StatsHolder.GetCurrentStanceValue();

        public CombatingEntity GenerateAndPrepareEntity(ICharacterCombatProvider variable, EnumTeam.GroupPositioning entityPosition)
        {
            // x----- CombatData
            CombatStatsHolder combatData = variable.GenerateCombatData();

            // x----- Area
            CharacterCombatAreasData areaData = new CharacterCombatAreasData(entityPosition, variable.RangeType);

            // X----- Critical Buff
            SCriticalBuffPreset criticalBuff = variable.GetCriticalBuff();
            if (criticalBuff == null)
            {
                var defaultCriticalBuffs
                    = CombatSystemSingleton.ParamsVariable.ArchetypesBackupOnNullCriticalBuffs;
                criticalBuff = UtilsCharacterArchetypes.GetElement(
                    defaultCriticalBuffs, entityPosition);
            }

            var entityParams = new EntityInvokerParams(combatData, areaData, criticalBuff, IsPlayerTeam);

            // Instantiate
            CombatingEntity entity = new CombatingEntity(
                variable.CharacterName,
                variable.CharacterPrefab,
                entityParams);

            // x----- Skills
            variable.GenerateCombatSkills(entity);

            entity.Injection(this);
            entity.CombatStats.Initialization();
            entity.CombatSkills.Initialization();

            return entity;

        }
    }
}
