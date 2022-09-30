using System.Collections.Generic;
using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;

namespace CombatSystem.Skills.VanguardEffects
{
    public sealed class VanguardCounterEffectsHolder : VanguardEffectsHolder
    {
        public VanguardCounterEffectsHolder(CombatEntity mainResponsibleEntity) : base(mainResponsibleEntity)
        {
        }
        
        protected override IEnumerable<PerformEffectValues> GetVanguardEffects(IVanguardSkill skill)
        {
            return skill.GetCounterEffects();
        }
    }
}
