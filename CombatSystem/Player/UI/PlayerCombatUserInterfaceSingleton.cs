using CombatSystem.Team;

namespace CombatSystem.Player.UI
{
    /// <summary>
    /// Singleton to extract visual references for COMBAT
    /// </summary>
    public static class PlayerCombatUserInterfaceSingleton
    {
        public static IOppositionTeamStructureRead<CombatPlayerTeamFeedBack> CombatTeemFeedBacks { get; internal set; }

    }
}
