using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(fileName = "ActionModifier Effect - N [Preset]",
        menuName = "Combat/Effects/Action Modifier")]
    public class SEffectActionModifier : SEffectBase
    {
        private const float BuffStatLowerCap = 1f; 
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float statAddition = user.CombatStats.BuffPower - BuffStatLowerCap;
            if (statAddition < 0) statAddition = 0;
            int modification = (int) (effectModifier + statAddition);
#if UNITY_EDITOR
            Debug.Log($"Addition Effect: {target.CharacterName} => {modification}");
#endif
            UtilsCombatStats.AddActionAmount(target.CombatStats,modification);

            target.Events.InvokeTemporalStatChange();
        }
    }
}
