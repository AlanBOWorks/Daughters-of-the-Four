using __ProjectExclusive.Player;
using CombatEntity;
using CombatSkills;
using CombatSystem.Events;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "De-MASTER - N [DeBuff]",
        menuName = "Combat/Effect/DE-Buff MASTER", order = 101)]
    public class SDeBuffMasterStat : SBuffMasterStat
    {
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatingEntity receiver,
            ref SkillComponentResolution resolution)
        {
            systemEvents.OnReceiveOffensiveEffect(receiver,ref resolution);
        }

        [Button]
        protected override void UpdateAssetName()
        {
            name = "DeMASTER - " + GetBuffType().ToString() + " [DeBuff]";
            UtilsAssets.UpdateAssetName(this);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.DeBuff;
        public override Color GetDescriptiveColor()
        {
            return PlayerCombatSingleton.SkillInteractionColors.Debuff;
        }
    }
}
