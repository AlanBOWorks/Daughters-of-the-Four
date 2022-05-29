namespace CombatSystem.Skills.Effects
{
    public static class EnumsEffect
    {
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

        public enum ConcreteArchetype
        {
            None,

            Damage,

            Recovery,
            Protection,

            Buff,
            DeBuff,

            Control,
            Stance
        }
    }
}
