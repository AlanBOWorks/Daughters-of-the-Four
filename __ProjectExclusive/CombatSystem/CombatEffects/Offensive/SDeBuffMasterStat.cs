using CombatEntity;
using Sirenix.OdinInspector;
using UnityEngine;
using Utils;

namespace CombatEffects
{

    [CreateAssetMenu(fileName = "De-MASTER - N [DeBuff]",
        menuName = "Combat/Effect/DE-Buff MASTER", order = 101)]
    public class SDeBuffMasterStat : SBuffMasterStat
    {
        
        protected override void CallEvents(CombatingEntity user, CombatingEntity effectTarget,
            SkillComponentResolution resolution)
        {
            user.EventsHolder.OnPerformSupportAction(effectTarget,ref resolution);
            if(user == effectTarget) return;

            effectTarget.EventsHolder.OnReceiveOffensiveAction(user,ref resolution);
        }

        [Button]
        protected override void UpdateAssetName()
        {
            name = "DeMASTER - " + GetBuffType().ToString() + " [DeBuff]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
