using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.Events;
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
        
        protected override void DoEventCall(SystemEventsHolder systemEvents, CombatEntityPairAction entities,
            ref SkillComponentResolution resolution)
        {
            CombatSystemSingleton.EventsHolder.OnStanceChange(entities.Target.Team, targetStance);
        }

        protected override SkillComponentResolution DoEffectOn(CombatingEntity user, CombatingEntity effectTarget, float effectValue,
            bool isCritical)
        {
            effectTarget.Team.OnStanceChange(targetStance);
            return new SkillComponentResolution(this, 1);
        }


        [Button]
        private void UpdateAssetName()
        {
            name = "TEAM - " + targetStance.ToString() + " Stance Switch [Effect]";
            UtilsAssets.UpdateAssetName(this);
        }

        public override EnumSkills.SkillInteractionType GetComponentType() => EnumSkills.SkillInteractionType.Stance;
    }
}
