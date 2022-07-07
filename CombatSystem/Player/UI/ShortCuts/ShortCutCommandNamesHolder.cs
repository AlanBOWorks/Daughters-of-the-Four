using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class ShortCutCommandNamesHolder : IShortcutCommandStructureRead<string>
    {
        public ShortCutCommandNamesHolder(IReadOnlyList<string> skillShortCuts, 
            string switchPreviousEntityShortCutElement,
            string switchNextEntityShortCutElement,
            string[] stanceSwitchShortcuts)
        {
            SkillShortCuts = skillShortCuts;
            SwitchPreviousEntityShortCutElement = switchPreviousEntityShortCutElement;
            SwitchNextEntityShortCutElement = switchNextEntityShortCutElement;

            SupportStanceShortCutElement = stanceSwitchShortcuts[EnumShortCuts.SupportStanceIndex];
            AttackStanceShortCutElement = stanceSwitchShortcuts[EnumShortCuts.AttackerStanceIndex];
            DefendStanceShortCutElement = stanceSwitchShortcuts[EnumShortCuts.DefendStanceIndex];
        }


        public IReadOnlyList<string> SkillShortCuts { get; }
        public string SwitchPreviousEntityShortCutElement { get; }
        public string SwitchNextEntityShortCutElement { get; }
        public string SupportStanceShortCutElement { get; }
        public string AttackStanceShortCutElement { get; }
        public string DefendStanceShortCutElement { get; }
    }
}
