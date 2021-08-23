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

            knockOutHandler = new MemberKnockOutHandler(this);
        }

        [ShowInInspector] public readonly TeamCombatState State;
        [ShowInInspector] public readonly TeamCombatStatsHolder StatsHolder;
        [ShowInInspector] public MemberKnockOutHandler knockOutHandler;



        public bool IsInDangerState()
            => State.IsInDanger();
        public ICharacterBasicStatsData GetCurrentStanceValue() 
            => StatsHolder.GetCurrentStanceValue();

    }
}
