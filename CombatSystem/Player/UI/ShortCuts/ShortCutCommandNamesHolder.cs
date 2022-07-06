using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class ShortCutCommandNamesHolder : IShortcutCommandStructureRead<string>
    {
        public ShortCutCommandNamesHolder(IReadOnlyList<string> skillShortCuts, 
            string switchPreviousEntityShortCutElement,
            string switchNextEntityShortCutElement)
        {
            SkillShortCuts = skillShortCuts;
            SwitchPreviousEntityShortCutElement = switchPreviousEntityShortCutElement;
            SwitchNextEntityShortCutElement = switchNextEntityShortCutElement;
        }

        private ShortCutCommandNamesHolder(IEnumerable<string> skillShortCuts, 
            string switchPreviousEntityShortCutElement,
            string switchNextEntityShortCutElement) :
         this(skillShortCuts as IReadOnlyList<string>,
             switchPreviousEntityShortCutElement,
             switchNextEntityShortCutElement)
        { }

        public IReadOnlyList<string> SkillShortCuts { get; }
        public string SwitchPreviousEntityShortCutElement { get; }
        public string SwitchNextEntityShortCutElement { get; }
    }
}
