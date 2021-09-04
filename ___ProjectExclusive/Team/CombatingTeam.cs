using _CombatSystem;
using Characters;
using Passives;
using Sirenix.OdinInspector;
using Stats;


namespace _Team
{
    public class CombatingTeam : CharacterArchetypesList<CombatingEntity>, ITeamCombatControlStats
    {
        public CombatingTeam(ITeamCombatControlHolder holder, int amountOfEntities = AmountOfArchetypes)
            : base(amountOfEntities)
        {
            ControlHandler = new TeamCombatControlHandler();
            if (holder != null)
            {
                StatsHolder = new TeamCombatStatsHolder(ControlHandler, holder);
            }
            else
                StatsHolder = new TeamCombatStatsHolder(ControlHandler);

            knockOutHandler = new MemberKnockOutHandler(this);

        }

        [ShowInInspector] public readonly TeamCombatControlHandler ControlHandler;
        [ShowInInspector] public readonly TeamCombatStatsHolder StatsHolder;
        [ShowInInspector] public MemberKnockOutHandler knockOutHandler;

        public IBasicStatsData<float> GetCurrentStanceValue() 
            => StatsHolder.GetCurrentStanceValue();

    }
}
