using CombatSystem.Entity;
using CombatSystem.Localization;
using CombatSystem.Stats;
using UnityEngine;

namespace CombatSystem.Skills.Effects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Team/Guarding")]
    public class STeamGuarding : SEffect, ITeamEffect
    {
        private const string GuardingEffectTag = EffectTags.GuardingEffectTag;
        private const string GuardingSmallPrefix = EffectTags.GuardingEffectPrefix;

        public override string EffectTag => GuardingEffectTag;
        public override string EffectSmallPrefix => GuardingSmallPrefix;
        public override EnumsEffect.ConcreteType EffectType => EnumsEffect.ConcreteType.Guarding;


        public override void DoEffect(EntityPairInteraction entities, float effectValue)
        {
            entities.Extract(out var performer, out var target);
            performer.Team.GuardHandler.SetGuarder(target);
        }
    }
}
