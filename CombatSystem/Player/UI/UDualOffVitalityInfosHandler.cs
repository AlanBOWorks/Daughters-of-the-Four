using CombatSystem.Entity;
using CombatSystem.Stats;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UDualOffVitalityInfosHandler : UDualTeamOffStructureInstantiateHandler<UVitalityInfo>, IDamageDoneListener
    {
        public void OnShieldLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if (GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnHealthLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if (GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnMortalityLost(in CombatEntity target, in CombatEntity performer, in float amount)
        {
            if (GetActiveElementsDictionary().ContainsKey(target))
                GetActiveElementsDictionary()[target].UpdateToCurrentStats();
        }

        public void OnKnockOut(in CombatEntity target, in CombatEntity performer)
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
