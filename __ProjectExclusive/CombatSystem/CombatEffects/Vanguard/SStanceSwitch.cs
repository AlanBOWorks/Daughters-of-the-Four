using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatTeam;
using Sirenix.OdinInspector;
using Stats;
using UnityEngine;
using Utils;

namespace CombatEffects
{
    [CreateAssetMenu(fileName = "N [Effect]",
        menuName = "Combat/Effect/Stance Switch", order = 40)]
    public class SStanceSwitch : SEffect
    {
        [SerializeField] private EnumTeam.TeamStance targetStance;
        

        protected override void DoEffectOn(SkillValuesHolders values, CombatingEntity effectTarget, float effectValue, bool isCritical)
        {
            effectTarget.Team.OnStanceChange(targetStance);
            CombatSystemSingleton.EventsHolder.OnStanceChange(effectTarget.Team, targetStance);
        }

        [Button]
        private void UpdateAssetName()
        {
            name = "TEAM - " + targetStance.ToString() + " Stance Switch [Effect]";
            UtilsAssets.UpdateAssetName(this);
        }
    }
}
