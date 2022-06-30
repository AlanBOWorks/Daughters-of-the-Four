using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public sealed class ShortCutCommandNamesHolder : IShortcutCommandStructureRead<string>
    {
        public ShortCutCommandNamesHolder(IReadOnlyList<string> skillShortCuts, string switchEntityShortCutElement)
        {
            SkillShortCuts = skillShortCuts;
            SwitchEntityShortCutElement = switchEntityShortCutElement;
        }

        private ShortCutCommandNamesHolder(IEnumerable<string> skillShortCuts, string switchEntityShortCutElement):
         this(skillShortCuts as IReadOnlyList<string>,switchEntityShortCutElement)
        { }

        public IReadOnlyList<string> SkillShortCuts { get; }
        public string SwitchEntityShortCutElement { get; }
    }
}
