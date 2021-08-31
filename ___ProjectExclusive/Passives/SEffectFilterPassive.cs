using Characters;
using CombatConditions;
using CombatEffects;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Passives
{
    [CreateAssetMenu(fileName = "Filter TYPE - N [Passives]",
        menuName = "Combat/Passives/Filter")]
    public class SEffectFilterPassive : SPassiveInjector
    {
        [TitleGroup("Effect")]
        [SerializeField] private SEffectBase effectTrigger;
        [SerializeField] private float percentVariation;
        [SerializeField, Tooltip("If this passive acts when the user does the effect or when it receives")] 
        private bool isActiveType = false;

        [TitleGroup("Condition")]
        [SerializeField] private UserOnlyConditionParam conditionParam;

        public override void InjectPassive(CombatingEntity entity)
        {
            entity.PassivesHolder.EffectFilters.AddFilter(this,isActiveType);
        }

        public void DoVariation(IEffectBase effectCheck, CombatingEntity target, ref float additionValue)
        {
            if((SEffectBase) effectCheck != effectTrigger) return;
            if(!conditionParam.CanBeUse(target)) return;

            additionValue += percentVariation;
        }

        protected override string AssetPrefix()
        {
            string type;
            if (isActiveType)
                type = "Filter ACTION - ";
            else
                type = "Filter REACTION - ";

            return type;
        }
    }
}
