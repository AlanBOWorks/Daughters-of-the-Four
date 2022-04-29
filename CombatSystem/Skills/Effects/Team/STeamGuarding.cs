using CombatSystem.Entity;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Team/Guarding")]
    public class STeamGuarding : SEffect
    {
        public override void DoEffect(in CombatEntity performer, in CombatEntity target, in float effectValue)
        {
            performer.Team.GuardHandler.SetGuarder(in target);
        }
    }
}
