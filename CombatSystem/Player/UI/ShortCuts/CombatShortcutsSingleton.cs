using UnityEngine.InputSystem;

namespace CombatSystem.Player.UI
{
    public static class CombatShortcutsSingleton
    {
        static CombatShortcutsSingleton()
        {
            ShortcutCommandNames = DefaultNamesHolder;
        }

        public static IShortcutCommandStructureRead<InputActionReference> InputActions;

        public static IShortcutCommandStructureRead<string> ShortcutCommandNames;


        private static readonly ShortCutCommandNamesHolder DefaultNamesHolder = new ShortCutCommandNamesHolder(
            new[] { "Q", "W", "E", "R", "A", "S", "D" },
            "↑", "↓",
            new[] { "1", "2", "3" });
    }
}
