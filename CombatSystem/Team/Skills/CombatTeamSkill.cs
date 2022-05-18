namespace CombatSystem.Team
{
    public sealed class CombatTeamSkill
    {
        public CombatTeamSkill(ITeamSkillPreset preset)
        {
            Preset = preset;
        }

        public readonly ITeamSkillPreset Preset;

        
    }
}
