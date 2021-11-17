using System.Collections.Generic;
using CombatEntity;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UVitalityUITooltipsHolder : UCanvasPivotOverEntityListener
    {
        private Dictionary<CombatingEntity, UPivotOverEntity> _dictionary;

        public override void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity)
        {
            var references = pivotOverEntity.GetReferences();
            var healthTooltipHolder = references.GetHealthStateHolder();
            healthTooltipHolder.Inject(user);
        }

        public override void InjectDictionary(Dictionary<CombatingEntity, UPivotOverEntity> dictionary)
        {
            _dictionary = dictionary;
        }
    }
}
