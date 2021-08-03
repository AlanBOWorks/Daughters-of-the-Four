using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Burst Team Control [Effect]",
        menuName = "Combat/Effects/Burst Team Control")]
    public class SEffectBurstTeamControl : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float burstAmount = 1)
        {
            DoEffect(target, burstAmount);
        }

        public override void DoEffect(CombatingEntity target, float burstAmount)
        {
            var targetTeam = target.CharacterGroup.Team;
            UtilsCombatStats.DoBurstControl(targetTeam, burstAmount);
        }
    }
}
