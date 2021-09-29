using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatTeam;
using Stats;
using UnityEngine;

namespace CombatEffects
{

    public class SStanceSwitch : SEffect
    {
        [SerializeField] private EnumTeam.TeamStance targetStance;
        public override void DoEffect(SkillValuesHolders values, float effectModifier)
        {
            var target = values.Target;
            target.Team.OnStanceChange(targetStance);
            CombatSystemSingleton.EventsHolder.OnStanceChange(target.Team, targetStance); 
        }

    }
}
