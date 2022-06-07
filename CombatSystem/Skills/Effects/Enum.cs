namespace CombatSystem.Skills.Effects
{
    public static class EnumsEffect
    {

        public static string TargetTypeTag = "T";
        public static string TargetTeamTypeTag = "TT";
        public static string PerformerTypeTag = "P";
        public static string PerformerTeamTypeTag = "PT";
        public static string TargetLineTypeTag = "TL";
        public static string PerformerLineTypeTag = "PL";
        public static string AllTypeTag ="A";

        public enum TargetType
        {
            Target,
            TargetTeam,

            Performer,
            PerformerTeam,

            TargetLine,
            PerformerLine,

            All
        }

        public enum Archetype
        {
            Others,
            Offensive,
            Support,
            Team
        }
    }
}
