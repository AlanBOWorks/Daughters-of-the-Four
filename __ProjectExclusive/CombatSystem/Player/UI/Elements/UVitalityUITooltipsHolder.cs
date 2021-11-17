using System;
using System.Collections.Generic;
using CombatEntity;
using CombatSkills;
using CombatSystem;
using CombatSystem.Events;
using UnityEngine;

namespace __ProjectExclusive.Player.UI
{
    public class UVitalityUITooltipsHolder : UCanvasPivotOverEntityListener,
        IDamageReceiverListener<ISkillParameters,CombatingEntity>
    {
        private Dictionary<CombatingEntity, UPivotOverEntity> _dictionary;

        private void Start()
        {
            CombatSystemSingleton.EventsHolder.Subscribe(this);
        }

        public override void OnPooledElement(CombatingEntity user, UPivotOverEntity pivotOverEntity)
        {
            var healthTooltipHolder = GetHealthHolder(pivotOverEntity);
            healthTooltipHolder.Inject(user);
        }

        private static UHealthStateUIHolder GetHealthHolder(UPivotOverEntity pivotOverEntity) =>
            pivotOverEntity.GetReferences().GetHealthStateHolder(); 

        public override void InjectDictionary(Dictionary<CombatingEntity, UPivotOverEntity> dictionary)
        {
            _dictionary = dictionary;
        }

        public void OnShieldDamage(ISkillParameters element, CombatingEntity receiver)
        {
            
        }

        public void OnHealthDamage(ISkillParameters element, CombatingEntity receiver)
        {
            var pivotOverEntity = _dictionary[receiver];
            var healthHolder = GetHealthHolder(pivotOverEntity);
            healthHolder.DoDamageToHealth();
        }

        public void OnMortalityDamage(ISkillParameters element, CombatingEntity receiver)
        {
        }
    }
}
