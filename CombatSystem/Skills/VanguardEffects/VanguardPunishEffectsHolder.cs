using System.Collections.Generic;
using CombatSystem.Entity;
using CombatSystem.Team;

namespace CombatSystem.Skills.VanguardEffects
{
    public sealed class VanguardPunishEffectsHolder : VanguardEffectsHolder
    {
        public VanguardPunishEffectsHolder(CombatEntity mainResponsibleEntity) : base(mainResponsibleEntity)
        {
        }

        public override void OnOffensiveDone(CombatEntity onTarget, CombatEntity attacker)
        {
            var mainVanguard = MainEntity;
            if (mainVanguard == null) return;

            var targetPosition = onTarget.PositioningType;
            switch (targetPosition)
            {
                case EnumTeam.Positioning.FrontLine:
                    return;
                case EnumTeam.Positioning.FlexLine:
                    if(onTarget.Team.DataValues.CurrentStance == EnumTeam.StanceFull.Defending)
                        return;
                    break;
            }


            PerformEffects(MainEntity, attacker);
        }

        protected override IEnumerable<PerformEffectValues> GetVanguardEffects(IVanguardSkill skill)
        {
            return skill.GetPunishEffects();
        }
    }

}
