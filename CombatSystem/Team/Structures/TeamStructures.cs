namespace CombatSystem.Team
{
    public class TeamPosition<T> : ITeamPositionStructureRead<T>
    {
        public TeamPosition(ITeamPositionStructureRead<T> copyFrom)
        {
            FrontLineType = copyFrom.FrontLineType;
            MidLineType = copyFrom.MidLineType;
            BackLineType = copyFrom.BackLineType;
        }
        public T FrontLineType { get; set; }
        public T MidLineType { get; set; }
        public T BackLineType { get; set; }
    }
}
