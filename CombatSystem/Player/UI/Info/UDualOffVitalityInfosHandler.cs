using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UDualOffVitalityInfosHandler : UDualTeamOffStructureInstantiateHandler<UVitalityInfo>, IDamageDoneListener
    {
        public void OnShieldLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
           
        }

        public void OnHealthLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            
        }

        public void OnMortalityLost(in CombatEntity performer, in CombatEntity target, in float amount)
        {
            
        }

        public void OnDamageReceive(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTarget(in target);
        }

        public void OnKnockOut(in CombatEntity performer, in CombatEntity target)
        {
            UpdateTarget(in target);
        }

        private void UpdateTarget(in CombatEntity target)
        {
            if (GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }


        public override void OnIterationCall(in UVitalityInfo element, in CombatEntity entity, in TeamStructureIterationValues values)
        {
            if(element == null) return;

            UtilsVitalityInfosHandler.HandleHandler(in element, in entity, in values, out var repositionIndex);
            RepositionElementByIndex(in element, in repositionIndex);
        }

        private const float ElementMarginTop = 2;
        private static void RepositionElementByIndex(in UVitalityInfo element, in int index)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            var position = rectTransform.localPosition;
            int marginIndex = index;

            float rectHeight = rectTransform.rect.height;
            position.y = -(rectHeight + ElementMarginTop) * marginIndex; //negative means going down

            rectTransform.localPosition = position;
        }
    }
}
