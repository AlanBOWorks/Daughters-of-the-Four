using CombatSystem._Core;
using CombatSystem.Entity;
using CombatSystem.Team;
using UnityEngine;

namespace CombatSystem.Player.UI
{
    public class UDualOffTempoTrackersHandler : UDualTeamOffStructureInstantiateHandler<UTempoTrackerHolder>, 
        ITempoEntityPercentListener, ITempoDedicatedEntityStatesListener
    {
        public void OnEntityTick(in CombatEntity entity, in float currentTick, in float percentInitiative)
        {
            if (GetActiveElementsDictionary().ContainsKey(entity))
                GetActiveElementsDictionary()[entity].TickTempo(in currentTick, in percentInitiative);
        }
        public override void OnIterationCall(in UTempoTrackerHolder element, in CombatEntity entity,
            in TeamStructureIterationValues values)
        {
            if(element == null) return;

            UtilsTempoInfosHandler.HandleHandler(in element, in entity, in values, out var repositionIndex);
            RepositionLocalHeight(in element, in repositionIndex);
        }


        public const float HeightElementSeparation = 16 * 2 + 10;
        private const float ElementMarginTop = 2;
        public void RepositionLocalHeight(in UTempoTrackerHolder element,in int index)
        {
            var rectTransform = element.GetComponent<RectTransform>();
            var position = rectTransform.localPosition;
            int marginIndex = index;

            float rectHeight = rectTransform.rect.height;
            position.y = -(rectHeight + ElementMarginTop) * marginIndex; //negative means going down

            rectTransform.localPosition = position;
        }

        public void OnTrinityEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            
        }

        public void OnOffEntityRequestSequence(CombatEntity entity, bool canAct)
        {
            //todo make activeAnimation 
        }

        public void OnTrinityEntityFinishSequence(CombatEntity entity)
        {
        }

        public void OnOffEntityFinishSequence(CombatEntity entity)
        {
            UtilsTempoInfosHandler.HandleOnFinishSequence(this, in entity);
        }
    }
}
