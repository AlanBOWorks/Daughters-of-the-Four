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
        

        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            effectTarget.Team.OnStanceChange(targetStance);
            CombatSystemSingleton.EventsHolder.OnStanceChange(effectTarget.Team, targetStance);
        }
    }
}
