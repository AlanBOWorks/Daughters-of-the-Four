namespace CombatSystem.Skills
{
    public static class EnumsSkill 
    {
        public enum TargetType
        {
            Direct,
            Area,
            /// <summary>
            /// For possibles targets it will behave as a [<see cref="Direct"/>] type, but on DoEffects
            /// will behave as an [<see cref="Area"/>] type (these could affect one or various targets at once)
            /// </summary>
            Hybrid
        }

        public enum Archetype
        {
            Self,
            Offensive,
            Support
        }
    }

    public static class EnumsEffect
    {
        public enum TargetType
        {
            Target,
            TargetTeam,
            /// <summary>
            /// Same as [<see cref="TargetTeam"/>] but [<see cref="Target"/>] is ignored
            /// </summary>
            TargetTeamExcluded,
            Performer,
            PerformerTeam,
            /// <summary>
            /// Same as [<see cref="PerformerTeam"/>] but [<see cref="Performer"/>] is ignored
            /// </summary>
            PerformerTeamExcluded,
            All
        }
    }
}
