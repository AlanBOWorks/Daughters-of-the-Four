using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSystem;
using CombatSystem.Events;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UConcentrationUITooltipsHolder : UCanvasPivotOverEntityListener,
        ITempoListener<CombatingEntity>
    {
        private void Start()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        private Dictionary<CombatingEntity, UPivotOverEntity> _dictionary;

        private static UActionsStateUIHolder GetActionsHolder(UPivotOverEntity pivot) =>
            pivot.GetReferences().GetUActionsStateHolder();
        private UActionsStateUIHolder GetActionsHolder(CombatingEntity entity) =>
            GetActionsHolder(_dictionary[entity]);

        public override void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity)
        {
            var actionsTooltipHolder = GetActionsHolder(pivotOverEntity);
            actionsTooltipHolder.Inject(user);
        }

        public override void InjectDictionary(Dictionary<CombatingEntity, UPivotOverEntity> dictionary)
        {
            _dictionary = dictionary;
        }

        public void OnFirstAction(CombatingEntity element)
        {
            OnFinishAction(element);
        }

        public void OnFinishAction(CombatingEntity element)
        {
            var actionsTooltipHolder = GetActionsHolder(element);
            actionsTooltipHolder.AnimateCurrentActions();
        }

        public void OnFinishAllActions(CombatingEntity element)
        {
        }
    }
}
