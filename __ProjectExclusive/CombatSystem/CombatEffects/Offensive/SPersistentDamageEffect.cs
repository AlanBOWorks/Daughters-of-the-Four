using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Stats;
using UnityEngine;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "OFFENSIVE - PersistentDamage [Effect]",
        menuName = "Combat/Effect/Persistent Damage")]
    public class SPersistentDamageEffect : SOffensiveEffect
    {
        private const float CriticalEffectModifier = 1.25f;

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue,
            bool isCritical)
        {
            if (isCritical)
                effectValue *= CriticalEffectModifier;

            UtilsCombatStats.AddPersistentDamage(effectTarget.CombatStats,effectValue);
            return new SkillComponentResolution(this,effectValue);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Persistent;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Persistent;
        }

        public override string GetEffectValueText(float effectValue)
        {
            return effectValue.ToString("####");
        }
    }
}
