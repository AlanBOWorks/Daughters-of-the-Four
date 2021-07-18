using _CombatSystem;
using Characters;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Initiative Modifier- N [Preset]",
        menuName = "Combat/Effects/Initiative Modifier")]
    public class SEffectInitiativeModifier :SEffectBase
    {
        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            float initiativeAddition = effectModifier * ( user.CombatStats.BuffPower);
#if UNITY_EDITOR
            Debug.Log($"Initiative Effect: {target.CharacterName} => {initiativeAddition}");
#endif

            UtilsCombatStats.AddInitiative(target.CombatStats,initiativeAddition);
            TempoHandler.CallUpdateOnInitiativeBar(target);
            target.Events.InvokeTemporalStatChange();
        }
    }
}
