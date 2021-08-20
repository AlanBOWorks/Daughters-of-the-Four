using Characters;
using Skills;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "COUNTER Burst Team Control [Effect]",
        menuName = "Combat/Effects/Team/Counter Burst Team Control")]
    public class SEffectCounterBurstTeamControl : SEffectBase
    {
        public override void DoEffect(SkillArguments arguments, CombatingEntity target, float counterBurstAmount = 1)
        {
            DoEffect(target, counterBurstAmount);
        }


        public override void DoEffect(CombatingEntity target, float counterBurstAmount)
        {
            var targetTeam = target.CharacterGroup.Team;
            UtilsCombatStats.DoCounterBurstControl(targetTeam, counterBurstAmount);
        }
    }
}
