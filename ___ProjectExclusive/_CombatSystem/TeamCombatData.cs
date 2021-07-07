using Characters;

namespace _CombatSystem
{
    public class TeamCombatData
    {
        public TeamCombatData(CombatingTeam team)
        {
            Team = team;
            State = States.Neutral;
        }

        public readonly CombatingTeam Team;

        public float ControlAmount;
        public States State;

        public enum States
        {
            Attacking,
            Neutral,
            Defending
        }
    }

}
