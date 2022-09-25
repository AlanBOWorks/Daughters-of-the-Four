using System.Collections.Generic;
using UnityEngine;

namespace CombatSystem.Skills
{
    public class CombinedSkill 
    {
        public CombinedSkill(IFullSkill primary, IFullSkill secondary)
        {
            PrimarySkill = primary;
            SecondarySkill = secondary;
        }

        public IFullSkill PrimarySkill;
        public IFullSkill SecondarySkill;

    }
}
