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
            control = new CombatTeamControl();
            if (holder != null)
            {
                StatsHolder = new TeamCombatStatsHolder(control, holder);
            }
            else
                StatsHolder = new TeamCombatStatsHolder(control);

            knockOutHandler = new MemberKnockOutHandler(this);

        }

        [ShowInInspector] public readonly CombatTeamControl control;
        [ShowInInspector] public readonly TeamCombatStatsHolder StatsHolder;
        [ShowInInspector] public MemberKnockOutHandler knockOutHandler;

        public IBasicStatsData<float> GetCurrentStanceValue() 
            => StatsHolder.GetCurrentStanceValue();

    }
}
