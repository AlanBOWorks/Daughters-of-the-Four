using Characters;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "Team Control [Effect]",
        menuName = "Combat/Effects/Team Control")]
    public class SEffectTeamControl : SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            //TODO add stats (control power)
            DoEffect(target,effectModifier);
        }

        public override void DoEffect(CombatingEntity target, float effectModifier)
        {
            UtilsCombatStats.AddTeamControl(target.CharacterGroup.Team,effectModifier);
        }
    }
}
