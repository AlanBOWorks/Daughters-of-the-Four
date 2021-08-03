using _CombatSystem;
using Characters;
using Passives;
using Sirenix.OdinInspector;


namespace _Team
{
    public class CombatingTeam : CharacterArchetypesList<CombatingEntity>, ITeamCombatControlStats
    {
        public CombatingTeam(ITeamCombatControlHolder holder, int amountOfEntities = AmountOfArchetypes)
            : base(amountOfEntities)
        {
            State = new TeamCombatState(this);
            if (holder != null)
            {
                StatsHolder = new TeamCombatStatsHolder(State, holder);
            }
            else
                StatsHolder = new TeamCombatStatsHolder(State);

            DeathHandler = new TeamMemberDeathHandler(this);
        }

        [ShowInInspector] public readonly TeamCombatState State;
        [ShowInInspector] public readonly TeamCombatStatsHolder StatsHolder;
        [ShowInInspector] public TeamMemberDeathHandler DeathHandler;

        public ICharacterBasicStats GetCurrentStats()
            => StatsHolder.GetCurrentStats();

        public FilterPassivesHolder GetCurrentPassives()
            => StatsHolder.GetCurrentPassives();

        public bool IsInDangerState()
            => State.IsInDanger();

        public void InjectAura(SAuraPassive aura)
        {
            StatsHolder.InjectAura(aura);
        }
    }
}
