using CombatSystem.Team;

namespace CombatSystem.Player.UI
{
    public static class PlayerCombatUserInterfaceSingleton
    {
        public static IOppositionTeamStructureRead<CombatPlayerTeamFeedBack> CombatTeemFeedBacks { get; internal set; }

    }
}
