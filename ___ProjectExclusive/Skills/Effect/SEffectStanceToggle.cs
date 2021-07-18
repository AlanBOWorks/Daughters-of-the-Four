using _CombatSystem;
using Characters;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Skills
{
    [CreateAssetMenu(fileName = "Stance Toggle - N [Preset]",
        menuName = "Combat/Effects/Stance Toggle")]
    public class SEffectStanceToggle : SEffectBase
    {
        [SerializeField] private TeamCombatData.Stance targetStance = TeamCombatData.Stance.Neutral;

        public override void DoEffect(CombatingEntity user, CombatingEntity target, float effectModifier = 1)
        {
            if(Random.value >= effectModifier) return;
            
            UtilsArea.ToggleStance(target,targetStance);
            target.Events.InvokeAreaChange();
        }
    }
}
